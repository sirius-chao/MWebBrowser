using CefSharp;
using Cys_Controls.Code;
using Cys_CustomControls.Code;
using Cys_CustomControls.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MWebBrowser.View.WebBrowser
{
    /// <summary>
    /// Interaction logic for WebTabControlUc.xaml
    /// </summary>
    public partial class WebTabControlUc : UserControl
    {
        public WebTabControlUc()
        {
            InitializeComponent();
            InitWebTabControl();
            this.Loaded += MWebBrowserUc_Loaded;
        }

        private void MWebBrowserUc_Loaded(object sender, RoutedEventArgs e)
        {
            InitCommand();
            InitData();
            TabItemAdd("http://www.baidu.com");
        }

        private void InitWebTabControl()
        {
            WebTabControl.CloseTabEvent += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (Application.Current.MainWindow != null)
                        Application.Current.MainWindow.Close();
                });
            };
        }

        private void InitData()
        {
            if (Application.Current.MainWindow is MMainWindow mw)
            {
                WebTabControl.PartHeaderParentGrid.MouseLeftButtonDown += mw.HeaderClickOrDragMove;
            }
        }

        private void InitCommand()
        {
            WebTabControl.TabItemAddCommand = new BaseCommand<object>(TabItemAdd);
        }

        private void TabItemAdd(object obj)
        {
            try
            {
                var uc = new WebTabItemUc { ViewModel = { CurrentUrl = obj?.ToString() } };
                var item = new TabItem { Content = uc };
                var titleBind = new Binding {Source = uc.DataContext, Path = new PropertyPath("Title") };
                item.SetBinding(HeaderedContentControl.HeaderProperty, titleBind);


                var faviconBind = new Binding { Source = uc.DataContext, Path = new PropertyPath("Favicon") };
                item.SetBinding(AttachedPropertyClass.ImageSourceProperty, faviconBind);

                WebTabControl.Items.Add(item);
                WebTabControl.SelectedItem = item;
                WebTabControl.SetHeaderPanelWidth();
            }
            catch (Exception ex)
            {

            }
        }

        private void WebTabControlUc_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F5) return;
            if (!(WebTabControl.SelectedItem is TabItem item)) return;
            if (!(item.Content is WebTabItemUc webTabItemUc)) return;
            webTabItemUc.CefWebBrowser?.Reload();
        }
    }
}