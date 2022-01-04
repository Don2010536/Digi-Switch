using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digi_Switch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush primary;
        SolidColorBrush secondary;

        double w = 0;
        double h = 0;

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists(DirLocations.settingsPath))
            {
                try
                {
                    Topmost = Convert.ToBoolean(File.ReadAllLines(DirLocations.settingsPath)[0]);
                } catch
                {
                    File.WriteAllText(DirLocations.settingsPath, DataManager.topMost.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DigiSwitch");
                File.Create(DirLocations.settingsPath);
            }

            DataManager.main = this;
            PageSettings.Load();
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender as Button, new Pages.Home());
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender as Button, new Pages.Settings());
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender as Button, new Pages.About());
        }

        private void BtnClick(Button button, object Content)
        {
            foreach (Button btn in MainGrid.Children.OfType<Button>())
                btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.textAndBorders));

            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.highlight));
            ContentPanel.Content = null;
            ContentPanel.Navigate(Content);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnClick(HomeBtn, new Pages.Home());
            PageSettings.Load();

            primary = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.primary));
            secondary = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.secondary));

            Menu.Fill = primary;
            MainGrid.Background = secondary;

            w = Width;
            h = Height;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                try { window.Close(); }
                catch { }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Menu_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if(Maximize.Content.ToString() == "▲")
            {
                w = Width;
                h = Height;

                Maximize.Content = "▼";
                WindowState = WindowState.Maximized;
            } else
            {
                Maximize.Content = "▲";
                WindowState = WindowState.Normal;
                Width = w;
                Height = h;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Maximize.Content = "▼";
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            w = Width;
            h = Height;
        }
    }
}
