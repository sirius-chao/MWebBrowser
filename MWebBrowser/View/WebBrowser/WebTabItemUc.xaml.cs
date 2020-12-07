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
using Cys_Controls.Code;

namespace MWebBrowser.View.WebBrowser
{
    /// <summary>
    /// Interaction logic for WebTabItem.xaml
    /// </summary>
    public partial class WebTabItemUc : UserControl
    {
        public CustomWebBrowser CefWebBrowser;
        public WebTabItemViewModel ViewModel;

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
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;
            try
            {
                if (e.Delta > 0)
                {
                    CefWebBrowser.ZoomInCommand.Execute(null);
                }
                else if (e.Delta < 0)
                {
                    CefWebBrowser.ZoomOutCommand.Execute(null);
                }

                SetSearchZoomStatus();
                e.Handled = true;
            }
            catch (Exception ex)
            {
               
            }
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
                CefWebBrowser.ZoomResetCommand.Execute(null);
                // CefWebBrowser.SetZoomLevel(0);
                SetSearchZoomStatus();
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
            NavigationStackPanel.DataContext = CefWebBrowser;
            CefWebBrowser.IsBrowserInitializedChanged += CefWebBrowser_IsBrowserInitializedChanged;
            WebParentGrid.Children.Add(CefWebBrowser);
            this.CefWebBrowser.TitleChanged += CefWebBrowser_TitleChanged;
            this.CefWebBrowser.PreviewKeyDown += CefWebBrowser_PreviewKeyDown;
            this.CefWebBrowser.ZoomLevelIncrement = _zoomLevelIncrement;
            this.CefWebBrowser.PreviewMouseWheel += CefWebBrowser_PreviewMouseWheel;

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

        private void NavigationBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.CefWebBrowser.Back();
        }

        private void NavigationForward_OnClick(object sender, RoutedEventArgs e)
        {
            this.CefWebBrowser.Forward();
        }

        private void NavigationRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            this.CefWebBrowser.Reload();
        }

        private void Search_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var pattern = @"^(http://|https://)?((?:[A-Za-z0-9]+-[A-Za-z0-9]+|[A-Za-z0-9]+)\.)+([A-Za-z]+)[/\?\:]?.*$";
            var match = Regex.Match(ViewModel.CurrentUrl, pattern);

            if (!match.Success) return;
            if (string.IsNullOrEmpty(ViewModel.CurrentUrl)) return;
            Load(ViewModel.CurrentUrl);

            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                //使search框失去焦点
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            });
        }

        private void SetSearchZoomStatus()
        {
            if (CefWebBrowser.ZoomLevel < 0)
            {
                ViewModel.ZoomLevelType = ZoomType.Out;
            }
            else if (CefWebBrowser.ZoomLevel > 0)
            {
                ViewModel.ZoomLevelType = ZoomType.In;
            }
            else
            {
                ViewModel.ZoomLevelType = ZoomType.None;
            }
        }
    }
}