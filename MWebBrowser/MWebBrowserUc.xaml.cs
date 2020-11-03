using CefSharp.Wpf;
using System.Windows.Controls;
using Cys_Controls.Code;
using MWebBrowser.UserControls;

namespace MWebBrowser
{
    /// <summary>
    /// MWebBrowserUc.xaml 的交互逻辑
    /// </summary>
    public partial class MWebBrowserUc : UserControl
    {
        ChromiumWebBrowser CefWebBrowser;
        public MWebBrowserUc()
        {
            InitializeComponent();
            InitCommand();
            CefWebBrowser = new ChromiumWebBrowser();
            //CefWebBrowser.IsBrowserInitializedChanged += MWebBrowser_IsBrowserInitializedChanged;
        }

        public void Load(string url)
        {
            //CefWebBrowser.Load(url);

           
        }

        private void InitCommand()
        {
            TabParent.TabItemAddCommand = new BaseCommand<object>(TabItemAdd);
        }

        private void TabItemAdd(object obj)
        {
            TabItem item = new TabItem {Header = "新标签页", Content = new WebItemUc()};
            TabParent.Items.Add(item);
            TabParent.SelectedItem = item;
            TabParent.SetHeaderPanelWidth();
        }

        //private void MWebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    try
        //    {
        //        //防止多次SetPreference，在Browser初始化完成后处理一次
        //        if (!CefWebBrowser.IsBrowserInitialized) return;          
        //        Cef.UIThreadTaskFactory.StartNew(() =>
        //        {
        //            var requestContext = CefWebBrowser.GetBrowser().GetHost().RequestContext;
        //            requestContext.SetPreference("profile.default_content_setting_values.plugins", 1, out _);//修改插件默认参数，使flash自动加载
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
