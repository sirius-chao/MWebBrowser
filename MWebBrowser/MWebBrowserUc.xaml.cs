using System.Net.Mime;
using System.Windows;
using CefSharp.Wpf;
using System.Windows.Controls;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
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
          
            this.Loaded += MWebBrowserUc_Loaded;
        }

        private void MWebBrowserUc_Loaded(object sender, RoutedEventArgs e)
        {
            InitData();
            TabItemAdd("http://www.baidu.com");
        }

        private void InitData()
        {
            if (Application.Current.MainWindow is MMainWindow mw)
            {
                TabParent.PartHeaderParentGrid.MouseLeftButtonDown += mw.HeaderClickOrDragMove;
            }
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
            WebItemUc uc = new WebItemUc{ TargetUrl = obj?.ToString()};
            TabItem item = new TabItem {Header = "新标签页", Content = uc };
            TabParent.Items.Add(item);
            TabParent.SelectedItem = item;
            TabParent.SetHeaderPanelWidth();
        }
    }
}
