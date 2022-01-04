using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;

namespace Digi_Switch
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : Window
    {
        string url = "";
        string cookie = "";

        public Browser()
        {
            InitializeComponent();
            Web.CoreWebView2InitializationCompleted += Web_CoreWebView2InitializationCompleted;
        }

        private void Web_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Nav();
        }

        public async void Init()
        {
            await Web.EnsureCoreWebView2Async();
            Nav();
        }

        private void Nav()
        {
            CoreWebView2Cookie view2Cookie = Web.CoreWebView2.CookieManager.CreateCookie("Cookie", cookie, url, "/");
            view2Cookie.IsHttpOnly = false;
            view2Cookie.IsSecure = false;
            Web.CoreWebView2.CookieManager.AddOrUpdateCookie(view2Cookie);
            try
            {
                Web.CoreWebView2.Navigate(new UriBuilder(url).Uri.AbsoluteUri);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public bool NavigateAsync(string url, string cookie)
        {
            try
            {
                this.url = url;
                this.cookie = cookie;
                Init();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
