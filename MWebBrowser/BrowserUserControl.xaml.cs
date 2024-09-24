using CefSharp;
using MWebBrowser.Code.CustomCef;
using System.Windows;
using System.Windows.Controls;

namespace MWebBrowser
{
    /// <summary>
    /// Interaction logic for BrowserUserControl.xaml
    /// </summary>
    public partial class BrowserUserControl : UserControl
    {
        public CustomWebBrowser CefWebBrowser;
        public BrowserUserControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            CefWebBrowser = new CustomWebBrowser();
            webBrower.Children.Add(CefWebBrowser);
        }

        public void ShowDevToolsDocked()
        {
            if (devToolsGrid.Visibility == Visibility.Collapsed)
            {
                devToolsGrid.Visibility = Visibility.Visible;
            }

            CefWebBrowser.GetBrowserHost().ShowDevTools();


            // 获取开发者工具的 WebView,wpf源码没提供，暂放弃
            CefWebBrowser.ShowDevTools();
            if (devToolsGrid.Children.Count == 0)
            {
                // 将开发者工具嵌入到指定的 Grid 中
            }
        }

        public void CloseDevToolsDocked()
        {
            devToolsGrid.Children.Clear();
            devToolsGrid.Visibility = Visibility.Collapsed; // 隐藏开发者工具
        }
    }

}
