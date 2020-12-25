using CefSharp;
using MWebBrowser.Code.CustomCef;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            }
        }

        private void CefWebBrowser_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.Title = CefWebBrowser.Title;
            ViewModel.Favicon = GetFavicon();
        }

        private ImageSource GetFavicon()
        {
            try
            {
                var pattern = @"(\w+:\/\/)([^/:]+)(:\d*)?";
                var address = CefWebBrowser.Address;
                var matches = Regex.Matches(address, pattern);
                return matches.Count <= 0 ? null : ImageHelper.GetBitmapFrame($"{matches[0]}/favicon.ico");
            }
            catch (Exception e)
            {
                return ImageHelper.DefaultFavicon;
            }
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

        }

        //private void InitSearchText()
        //{
        //    SearchText.ZoomInCommand = new BaseCommand<object>((obj) =>
        //    {
        //        ZoomIn();
        //    });
        //    SearchText.ZoomOutCommand = new BaseCommand<object>((obj) =>
        //    {
        //        ZoomOut();
        //    });
        //    SearchText.ZoomResetCommand = new BaseCommand<object>((obj) =>
        //    {
        //        ZoomReset();
        //    });
        //}

        //private void InitWebMenu()
        //{
        //    WebMenu.ZoomInEvent += ZoomIn;
        //    WebMenu.ZoomOutEvent += ZoomOut;
        //}

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
