using CefSharp;
using MWebBrowser.Code.CustomCef;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cys_Controls.Code;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for WebTabItem.xaml
    /// </summary>
    public partial class WebTabItemUc : UserControl
    {
        public CustomWebBrowser CefWebBrowser;
        public WebTabItemViewModel ViewModel;
        public Action<object, MouseWheelEventArgs> WebMouseWheelEvent;

        public Action SetCurrentEvent;
        private readonly double _zoomLevelIncrement = 0.2;//默认为0.1


        public WebTabItemUc()
        {
            ViewModel = new WebTabItemViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();
            InitWebBrowser();
        }


        private void CefWebBrowser_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            WebMouseWheelEvent?.Invoke(sender,e);
        }
      

        private void CefWebBrowser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                this.CefWebBrowser.Reload();
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                && (e.Key == Key.D0 || e.Key == Key.NumPad0))
            {
                var uc = ControlHelper.FindVisualParent<WebTabControlUc>(this);
                uc?.SearchText.ZoomResetCommand.Execute(null);
            }
        }

        private void CefWebBrowser_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.Title = CefWebBrowser.Title;
            ViewModel.Favicon = ImageHelper.GetFavicon(CefWebBrowser.Address);
            SetCurrentEvent?.Invoke();
        }
        private void InitWebBrowser()
        {
            CefWebBrowser = new CustomWebBrowser();
            //NavigationStackPanel.DataContext = CefWebBrowser;
            CefWebBrowser.IsBrowserInitializedChanged += CefWebBrowser_IsBrowserInitializedChanged;
            WebParentGrid.Children.Add(CefWebBrowser);
            this.CefWebBrowser.TitleChanged += CefWebBrowser_TitleChanged;
            this.CefWebBrowser.PreviewKeyDown += CefWebBrowser_PreviewKeyDown;
            this.CefWebBrowser.ZoomLevelIncrement = _zoomLevelIncrement;
            this.CefWebBrowser.PreviewMouseWheel += CefWebBrowser_PreviewMouseWheel;
            var s = this.CefWebBrowser.ZoomLevel;

        }

        private void CefWebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {

                if (!CefWebBrowser.IsBrowserInitialized) return;
                CefWebBrowser.Focus();//浏览器初始化完毕后获得焦点
                if (!string.IsNullOrEmpty(ViewModel.CurrentUrl))
                {
                    Load(ViewModel.CurrentUrl);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Load(string url)
        {
            CefWebBrowser.Load(url);
        }
    }
}
