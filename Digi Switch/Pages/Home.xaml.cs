using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Digi_Switch.CustomControls;
using System.IO;
using System.Windows.Threading;

namespace Digi_Switch.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        SolidColorBrush primary;
        SolidColorBrush secondary;
        SolidColorBrush textBorder;
        SolidColorBrush good;
        SolidColorBrush medium;
        SolidColorBrush bad;

        private List<MonitoredItem> MonitoredItems = new List<MonitoredItem>();
        private DispatcherTimer timer = new DispatcherTimer();

        public Home()
        {
            InitializeComponent();
            Load();
            DataManager.Init();

            Load();

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadBrushes();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Save();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            MonitoredItem item = new MonitoredItem()
            {
                Margin = new Thickness(10,5,10,5)
            };

            MonitoredItems.Add(item);
            MonitorStack.Children.Add(item);

            timer.Stop();
            DataManager.Stop();

            SaveBtn.Visibility = Visibility.Visible;
            CancelBtn.Visibility = Visibility.Visible;
            StartBtn.Visibility = Visibility.Collapsed;
        }

        private void Save()
        {
            string data = "";
            foreach (MonitoredItem item in MonitoredItems)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    data += $"{item.URLBox.Text}~{item.fullscreen}~{item.cookie}~{item.CookieTxt.Text}~{item.TimeBox.Text}\n";
                }
            }

            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DigiSwitch\\");

            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DigiSwitch\\Monitor.txt", data);
        }

        private void Load()
        {
            try
            {
                string[] lines = File.ReadAllLines($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DigiSwitch\\Monitor.txt");
                string[] splitLine;

                MonitoredItems.Clear();
                MonitorStack.Children.Clear();

                foreach (string line in lines)
                {
                    splitLine = line.Split('~');

                    MonitoredItem item = new MonitoredItem()
                    {
                        Margin = new Thickness(10, 5, 10, 5)
                    };

                    item.url = splitLine[0];
                    item.fullscreen = Convert.ToBoolean(splitLine[1]);
                    item.cookie = Convert.ToBoolean(splitLine[2]);
                    item.CookieTxt.Text = splitLine[3];
                    item.URLBox.Text = splitLine[0];
                    item.TimeBox.Text = splitLine[4];
                    item.switcher = TimeSpan.FromSeconds(Convert.ToDouble(splitLine[4]));

                    item.FullScreenBox.IsChecked = item.fullscreen;
                    item.CookieBox.IsChecked = item.cookie;

                    item.PidPop();

                    MonitoredItems.Add(item);
                    MonitorStack.Children.Add(item);
                }

                DataManager.Sync(ref MonitoredItems);

                timer.Start();
            }
            catch { }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Load();

            SaveBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            StartBtn.Visibility = Visibility.Visible;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Load();

            SaveBtn.Visibility = Visibility.Collapsed;
            CancelBtn.Visibility = Visibility.Collapsed;
            StartBtn.Visibility = Visibility.Visible;
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StartBtn.Content.ToString() == "Stop")
                DataManager.Stop();
            else
                DataManager.Start();

            StartBtn.Background = StartBtn.Content.ToString() == "Stop" ? good : bad;
            StartBtn.Content = StartBtn.Content.ToString() == "Stop" ? "Start" : "Stop";
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DataManager.Stop();
        }

        private void LoadBrushes()
        {
            primary = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.primary));
            secondary = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.secondary));
            textBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.textAndBorders));
            good = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.good));
            medium = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.medium));
            bad = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.bad));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainBorder.Background = primary;
            HeaderBorder.Background = secondary;
            
            HeaderBorder.BorderBrush = textBorder;
            URLTxt.Foreground = textBorder;
            IntrivalTxt.Foreground = textBorder;
            SettingsTxt.Foreground = textBorder;
            SaveBtn.Foreground = textBorder;
            CancelBtn.Foreground = textBorder;
            StartBtn.Foreground = textBorder;
            AddBtn.Foreground = textBorder;

            SaveBtn.Background = good;
            CancelBtn.Background = medium;
            StartBtn.Background = good;
            AddBtn.Background = good;

            HeaderShadow.Color = (Color)ColorConverter.ConvertFromString(PageSettings.highlight);
        }
    }
}
