using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Digi_Switch.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        SolidColorBrush primary;
        SolidColorBrush textBorder;
        SolidColorBrush bad;

        public About()
        {
            InitializeComponent();

            Logo.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}/Pages/Logo.png"));

            primary    = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.primary       ));
            bad        = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.bad           ));
            textBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString(PageSettings.textAndBorders));
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ByTxt.Foreground             = textBorder;
            VersionTxt.Foreground        = textBorder;
            DescriptionHeader.Foreground = textBorder;
            Description.Foreground       = textBorder;
            CookieLink.Foreground        = textBorder;

            ImportantTxt.Foreground = bad;

            AboutBackground.Background       = primary;
            DescriptionBackground.Background = primary;
        }
    }
}
