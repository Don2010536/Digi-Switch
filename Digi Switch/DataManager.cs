using Digi_Switch.CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Windows;
using System.IO;

namespace Digi_Switch
{
    class DataManager
    {
        public static string log = "";
        public static bool topMost;
        public static Browser browser { get; set; }
        public static DispatcherTimer timer = new DispatcherTimer();
        private static int position = 0;
        public static List<MonitoredItem> MonitoredItems = new List<MonitoredItem>();
        public static MainWindow main;

        public static bool init = false;

        public static void Init()
        {
            if (!init) {
                try
                {
                    topMost = Convert.ToBoolean(File.ReadAllLines($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DigiSwitch\\Settings.txt")[0]);
                } catch { }

                main.Topmost = topMost;
                timer.Tick += Timer_Tick;
                if (browser == null)
                    browser = new Browser();
            }
            init = true;
        }

        public static void Sync(ref List<MonitoredItem> items)
        {
            MonitoredItems = items;
        }

        /// <summary>
        /// Start the timer and make sure the position is set as 0
        /// </summary>
        public static void Start()
        {
            DeselectItems();
            position = 0;
            timer.Interval = TimeSpan.FromSeconds(0);
            timer.Start();
        }

        /// <summary>
        /// Stop the timer and reset the position to 0
        /// </summary>
        public static void Stop()
        {
            DeselectItems();
            position = 0;
            timer.Interval = TimeSpan.FromSeconds(0);
            timer.Stop();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            if (log.Split('\n').Length >= 50)
            {
                log = string.Join('\n', log.Split('\n')[1..]);
            }

            if (MonitoredItems.Count > 0)
            {
                if (MonitoredItems[position].url.Contains(".exe"))
                {
                    if (MonitoredItems[position].pid != -1)
                    {
                        bringToFront(MonitoredItems[position].pid, MonitoredItems[position].fullscreen);

                        log += $"Brought {MonitoredItems[position].url} to foreground.\n";
                    } else
                    {
                        position = position + 1 < MonitoredItems.Count ? position + 1 : 0;

                        Timer_Tick(sender, e);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        ShowBrowser();
                    } catch
                    {
                        browser = new Browser();
                        ShowBrowser();
                    }
                }

                DeselectItems();

                MonitoredItems[position].selected = true;
                MonitoredItems[position].Select();

                timer.Interval = MonitoredItems[position].switcher;

                position = position + 1 < MonitoredItems.Count ? position + 1 : 0;
            }
        }

        private static void DeselectItems()
        {
            foreach (MonitoredItem item in MonitoredItems)
            {
                item.selected = false;
                item.Deselect(0);
            }
        }

        private static void ShowBrowser()
        {
            if (browser.NavigateAsync(MonitoredItems[position].url, MonitoredItems[position].CookieTxt.Text))
            {
                // Shows and activates the window setting the state to maximized if needed
                browser.WindowState = MonitoredItems[position].fullscreen ? WindowState.Maximized : WindowState.Normal;
                browser.Show();
                browser.Activate();

                // Log what url we are showing in the browser
                log += $"Showing {MonitoredItems[position].url} in browser.\n";
            }
        }

        // Moving a window to the front of the ZOrder and maximizing if needed

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern Int32 ShowWindow(IntPtr lpClassName, int nCmdShow);

        [DllImport("USER32.DLL")]
        public static extern bool IsIconic(IntPtr handle);
        [DllImport("USER32.DLL")]
        public static extern bool PostMessageA(IntPtr handle, uint Msg, UIntPtr WPARAM, long LPARAM);
        [DllImport("USER32.DLL")]
        static extern bool SetWindowPos(
            int hWnd,             // Window handle
            int hWndInsertAfter,  // Placement-order handle
            int X,                // Horizontal position
            int Y,                // Vertical position
            int cx,               // Width
            int cy,               // Height
            uint uFlags);

        public static void bringToFront(int pid, bool fullscreen)
        {
            // Get a handle to the application.
            IntPtr handle = Process.GetProcessById(pid).MainWindowHandle;

            // Verify that the app is a running process.
            if (IsIconic(handle))
            {
                ShowWindow(handle, 9);
            }

            // Width height and flag settings
            int width  = fullscreen ? Convert.ToInt32(SystemParameters.PrimaryScreenWidth ) : 0;
            int height = fullscreen ? Convert.ToInt32(SystemParameters.PrimaryScreenHeight) : 0;

            int flags = 0x0002 | 0x0040 | 0x0001 | 0x0010;

            if (fullscreen)
            {
                // Maximize the app
                PostMessageA(handle, 0x0112, (UIntPtr)0xF030, 0);
            }

            // Make the app the topmost non topmost application
            SetWindowPos(handle.ToInt32(),  0, 0, 0, width, height, (uint)flags);
            SetWindowPos(handle.ToInt32(), -1, 0, 0, width, height, (uint)flags);
            SetWindowPos(handle.ToInt32(), -2, 0, 0, width, height, (uint)flags);
        }
    }
}