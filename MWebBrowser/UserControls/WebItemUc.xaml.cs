using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp;
using MWebBrowser.Code.CustomCef;

namespace MWebBrowser.UserControls
{
    /// <summary>
    /// WebItemUc.xaml 的交互逻辑
    /// </summary>
    public partial class WebItemUc : UserControl
    {
        public CustomWebBrowser CefWebBrowser;

        public string TargetUrl;//网页链接Url

        private string CurrentUrl;
        public WebItemUc()
        {
            InitializeComponent();
            InitWebBrowser();
            this.Loaded += WebItemUc_Loaded;
        }

        private void WebItemUc_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }

        private void InitWebBrowser()
        {
            CefWebBrowser = new CustomWebBrowser();
            CefWebBrowser.IsBrowserInitializedChanged += CefWebBrowser_IsBrowserInitializedChanged;
            WebParentGrid.Children.Add(CefWebBrowser);
            
        }

        private void CefWebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                //防止多次SetPreference，在Browser初始化完成后处理一次
                if (!CefWebBrowser.IsBrowserInitialized) return;


                if (!string.IsNullOrEmpty(TargetUrl))
                {
                    Load(TargetUrl);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void WebItemUc_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            string pattern = @"^(http://|https://)?((?:[A-Za-z0-9]+-[A-Za-z0-9]+|[A-Za-z0-9]+)\.)+([A-Za-z]+)[/\?\:]?.*$";
            var match = Regex.Match(SearchText.Text, pattern);

            if (!match.Success) return;
            if (!string.IsNullOrEmpty(CurrentUrl) && CurrentUrl == SearchText.Text) return;
            CurrentUrl = SearchText.Text;
            Load(SearchText.Text);
        }

        public void Load(string url)
        {
            CefWebBrowser.Load(url);
        }

    }
}
