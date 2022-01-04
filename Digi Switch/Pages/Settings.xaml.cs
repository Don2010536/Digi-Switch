using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Digi_Switch.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        SolidColorBrush primary;
        SolidColorBrush secondary;
        SolidColorBrush textBorder;
        SolidColorBrush highlight;
        SolidColorBrush good;
        SolidColorBrush medium;
        SolidColorBrush bad;

        DispatcherTimer timer = new DispatcherTimer();

        public Settings()
        {
            InitializeComponent();
            Load();
            
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;

            TopCheck.IsChecked = DataManager.topMost;
            LoadBrushes();
            Load();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SaveTxt.Visibility = Visibility.Collapsed;
        }

        private void Load()
        {
            LogBox.Document.Blocks.Clear();
            LogBox.AppendText(DataManager.log);
            LogBox.ScrollToEnd();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            DataManager.topMost = (bool)(sender as CheckBox).IsChecked;
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DigiSwitch\\Settings.txt", DataManager.topMost.ToString());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PrimaryRect.Background = primary;
            PrimaryRect.BorderBrush = textBorder;
            PrimaryTxt.Text = PageSettings.primary;

            SecondaryRect.Background = secondary;
            SecondaryRect.BorderBrush = textBorder;
            SecondaryTxt.Text = PageSettings.secondary;

            TextRect.Background = textBorder;
            TextRect.BorderBrush = textBorder;
            TextTxt.Text = PageSettings.textAndBorders;

            GoodRect.Background = good;
            GoodRect.BorderBrush = textBorder;
            GoodTxt.Text = PageSettings.good;

            MediumRect.Background = medium;
            MediumRect.BorderBrush = textBorder;
            MediumTxt.Text = PageSettings.medium;

            BadRect.Background = bad;
            BadRect.BorderBrush = textBorder;
            BadTxt.Text = PageSettings.bad;

            HighlightRect.Background = highlight;
            HighlightRect.BorderBrush = textBorder;
            HighlightTxt.Text = PageSettings.highlight;

            ColorTxt.Foreground = textBorder;
            SettingsTxt.Foreground = textBorder;
            LogTxt.Foreground = textBorder;
            LogBox.Foreground = textBorder;
            LogBox.BorderBrush = textBorder;
            TopCheck.Foreground = textBorder;
            TopCheck.Background = textBorder;
            SaveTxt.Foreground = textBorder;

            SaveBtn.Background = good;
            SaveBtn.Foreground = textBorder;

            ResetBtn.Background = medium;
            ResetBtn.Foreground = textBorder;

            MainBorder.Background = primary;
            LogBox.Background = secondary;

            SaveTxt.Visibility = Visibility.Collapsed;

            LogBoxDropShadow.Color = (Color)ColorConverter.ConvertFromString(PageSettings.highlight);
            UpdateLayout();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (Verify())
            {
                PageSettings.primary = PrimaryTxt.Text;
                PageSettings.secondary = SecondaryTxt.Text;
                PageSettings.textAndBorders = TextTxt.Text;
                PageSettings.highlight = HighlightTxt.Text;
                PageSettings.good = GoodTxt.Text;
                PageSettings.medium = MediumTxt.Text;
                PageSettings.bad = BadTxt.Text;

                PageSettings.Save();
            }
            Reload();

            SaveTxt.Visibility = Visibility.Visible;
            timer.Start();
        }

        private bool Verify()
        {
            Color temp;

            try {
                temp = (Color)ColorConverter.ConvertFromString(PrimaryTxt.Text);
                temp = (Color)ColorConverter.ConvertFromString(SecondaryTxt.Text);
                temp = (Color)ColorConverter.ConvertFromString(TextTxt.Text);
                temp = (Color)ColorConverter.ConvertFromString(HighlightTxt.Text);
                temp = (Color)ColorConverter.ConvertFromString(GoodTxt.Text);
                temp = (Color)ColorConverter.ConvertFromString(MediumTxt.Text);
                temp = (Color)ColorConverter.ConvertFromString(BadTxt.Text);

                return true;
            } catch {
                new Dialogs.UnexpectedValue().ShowDialog();
                return false; 
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            PageSettings.Reset();
            PageSettings.Load();
            Reload();
        }

        private void Reload()
        {
            LoadBrushes();
            DataManager.main.Menu.Fill = primary;
            DataManager.main.MainGrid.Background = secondary;
            DataManager.main.UpdateLayout();

            Page_Loaded(null, null);
        }

        private void LoadBrushes()
        {
            primary = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.primary));
            secondary = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.secondary));
            textBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.textAndBorders));
            highlight = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.highlight));
            good = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.good));
            medium = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.medium));
            bad = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.bad));
        }

        private void Txt_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                ColorConverter.ConvertFromString((sender as TextBox).Text);
                (sender as TextBox).Foreground = textBorder;
            } catch
            {
                (sender as TextBox).Foreground = bad;
            }
        }
    }
}
