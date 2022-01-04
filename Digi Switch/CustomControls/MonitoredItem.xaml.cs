using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Threading;

namespace Digi_Switch.CustomControls
{
    /// <summary>
    /// Interaction logic for MonitoredItem.xaml
    /// </summary>
    public partial class MonitoredItem : UserControl
    {
        public bool fullscreen = false;
        public bool cookie = false;
        public TimeSpan switcher;
        public string url;
        public int pid = 0;
        public bool selected = false;

        private string green = "#FF71F9A2";
        private string blue  = PageSettings.highlight;
        private DispatcherTimer timer = new DispatcherTimer();

        SolidColorBrush secondary;
        SolidColorBrush bad;
        SolidColorBrush textBorder;

        public MonitoredItem()
        {
            InitializeComponent();
            
            Shadow.Opacity = 0;
            Shadow_Cookie.Opacity = 0;

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;

            secondary  = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.secondary));
            bad        = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.bad           ));
            textBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.textAndBorders));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            PidPop();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Deselect(100);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!selected)
            {
                Deselect(0);
            } else
            {
                Select();
            }
        }

        private void FullScreenBox_Click(object sender, RoutedEventArgs e)
        {
            fullscreen = (bool)FullScreenBox.IsChecked;
        }

        private void CookieBox_Click(object sender, RoutedEventArgs e)
        {
            cookie = (bool)CookieBox.IsChecked;

            if (cookie)
            {
                CookieEntry.Visibility = Visibility.Visible;
            } else
            {
                CookieEntry.Visibility = Visibility.Collapsed;
            }
        }

        private void TimeBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switcher = TimeSpan.FromSeconds(Convert.ToDouble(TimeBox.Text));
            }
            catch
            {
                e.Handled = true;
                TimeBox.Undo();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            DataManager.MonitoredItems.Remove(this);
            Visibility = Visibility.Collapsed;
        }

        private void URLBox_KeyUp(object sender, KeyEventArgs e)
        {
            url = URLBox.Text;
            PidPop();
        }

        private void Box_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as TextBox).BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.textAndBorders));
        }

        private void Box_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBox).BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
        }

        public void PidPop()
        {
            if (url != null && url.Contains(".exe"))
            {
                int i = PIDs.SelectedIndex;
                Process[] processes = Process.GetProcessesByName(url.Replace(".exe", ""));

                PIDs.Items.Clear();
                foreach (Process p in processes)
                {
                    PIDs.Items.Add(p.Id.ToString());
                }

                if (i < PIDs.Items.Count)
                    PIDs.SelectedIndex = i;
                else
                    PIDs.SelectedIndex = -1;

                if (PIDs.SelectedIndex != -1)
                    pid = Convert.ToInt32(PIDs.Text);
                else
                    pid = -1;

                PIDs.Visibility = Visibility.Visible;
                URLBox.SetValue(Grid.ColumnSpanProperty, 1);
                
                timer.Start();
            }
            else
            {
                PIDs.Visibility = Visibility.Collapsed;
                URLBox.SetValue(Grid.ColumnSpanProperty, 2);
                
                timer.Stop();
            }
        }

        private void PIDs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pid = Convert.ToInt32(PIDs.Text);
        }

        public void Select()
        {
            try
            {
                Shadow.Opacity = 100;
                Shadow.Color = (Color)ColorConverter.ConvertFromString(green);

                Shadow_Cookie.Opacity = 100;
                Shadow_Cookie.Color = (Color)ColorConverter.ConvertFromString(green);
            } catch { }
        }

        public void Deselect(int op)
        {
            try
            {
                Shadow.Opacity        = op;
                Shadow_Cookie.Opacity = op;

                Shadow.Color        = (Color)ColorConverter.ConvertFromString(blue);
                Shadow_Cookie.Color = (Color)ColorConverter.ConvertFromString(blue);
            }
            catch { }
        }

        private void URLBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (URLBox.Text.ToLower() == "click here...")
            {
                URLBox.Text = "";
            }
        }

        private void PIDs_DropDownClosed(object sender, EventArgs e)
        {
            if (PIDs.Text != "")
                pid = Convert.ToInt32(PIDs.Text);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainBorder.Background  = secondary;
            MainBorder.BorderBrush = textBorder;

            DeleteBtn.Foreground = textBorder;
            DeleteBtn.Background = bad;

            URLBox.Foreground = textBorder;
            
            TimeBox.Foreground = textBorder;
            
            FullScreenBox.Foreground = textBorder;
            FullScreenBox.Background = textBorder;

            CookieBox.Foreground = textBorder;
            CookieBox.Background = textBorder;

            CookieEntry.Background  = secondary;
            CookieEntry.BorderBrush = textBorder;

            CookieTxt.Foreground = textBorder;
        }
    }
}
