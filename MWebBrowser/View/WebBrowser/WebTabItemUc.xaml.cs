using CefSharp;
using Cys_Controls.Code;
using MWebBrowser.Code.CustomCef;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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


        private readonly System.Timers.Timer _zoomToolTimer = new System.Timers.Timer(1000);
        private int _zoomWaitingCount = -1;

        public WebTabItemUc()
        {
            ViewModel = new WebTabItemViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();
            InitWebBrowser();
            InitSearchText();
            InitWebMenu();
        }


        private void CefWebBrowser_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                ViewModel.ZoomStaysOpen = false;
                return;
            }
            try
            {
                if (e.Delta > 0)
                {
                    ZoomIn();
                }
                else if (e.Delta < 0)
                {
                    ZoomOut();
                }
                _zoomWaitingCount = 0;
                _zoomToolTimer.Elapsed -= ZoomToolTimer_Elapsed;
                _zoomToolTimer.Elapsed += ZoomToolTimer_Elapsed;
                _zoomToolTimer.AutoReset = true;
                _zoomToolTimer.Enabled = true;
                e.Handled = true;
            }
            catch (Exception ex)
            {
               
            }
        }

        private void ZoomIn()
        {
            if (this.CefWebBrowser.ZoomLevel < 4)
            {
                CefWebBrowser.ZoomInCommand.Execute(null);
            }
            ViewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private void ZoomOut()
        {
            if (this.CefWebBrowser.ZoomLevel > -4)
            {
                CefWebBrowser.ZoomOutCommand.Execute(null);
            }
            ViewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private void ZoomReset()
        {
            CefWebBrowser.ZoomResetCommand.Execute(null);
            // CefWebBrowser.SetZoomLevel(0);
            SetSearchZoomStatus();
        }

        private void ZoomToolTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_zoomWaitingCount > 2)
            {
                _zoomToolTimer?.Stop();
                ViewModel.ZoomIsChecked = false;
                ViewModel.ZoomStaysOpen = false;
                _zoomWaitingCount = -1;
                return;
            }

            if (_zoomWaitingCount > -1)
            {
                _zoomWaitingCount++;
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

        private void InitSearchText()
        {
            SearchText.ZoomInCommand = new BaseCommand<object>((obj) =>
            {
                ZoomIn();
            });
            SearchText.ZoomOutCommand = new BaseCommand<object>((obj) =>
            {
                ZoomOut();
            });
            SearchText.ZoomResetCommand = new BaseCommand<object>((obj) =>
            {
                ZoomReset();
            });
        }

        private void InitWebMenu()
        {
            WebMenu.ZoomInEvent += ZoomIn;
            WebMenu.ZoomOutEvent += ZoomOut;
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
                ViewModel.ZoomIsChecked = true;
                if (CefWebBrowser.ZoomLevel > -1)
                {
                    ViewModel.ZoomRatio = "90%";
                }
                else if (CefWebBrowser.ZoomLevel <= 1)
                {
                    var radio = Math.Round((CefWebBrowser.ZoomLevel+5) / 5 * 100);
                    ViewModel.ZoomRatio = $"{radio}%";
                }
            }
            else if (CefWebBrowser.ZoomLevel > 0)
            {
                ViewModel.ZoomLevelType = ZoomType.In;
                ViewModel.ZoomIsChecked = true;
                var radio = Math.Round((1 + CefWebBrowser.ZoomLevel) * 100, 2);
                ViewModel.ZoomRatio = $"{radio}%";
            }
            else
            {
                ViewModel.ZoomLevelType = ZoomType.None;
                ViewModel.ZoomIsChecked = false;
            }
        }
    }
}
