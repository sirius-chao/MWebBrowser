using CefSharp;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using Cys_Resource.Code;
using MWebBrowser.Code;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for WebTabControlUc.xaml
    /// </summary>
    public partial class WebTabControlUc : UserControl
    {
        private readonly WebTabControlViewModel _viewModel;
        private WebTabItemUc _currentWebTabItem;
        private readonly System.Timers.Timer _zoomToolTimer = new System.Timers.Timer(1000);
        private int _zoomWaitingCount = -1;
        public WebTabControlUc()
        {
            InitializeComponent();
            InitWebTabControl();
            _viewModel = new WebTabControlViewModel();
            this.DataContext = _viewModel;
            this.Loaded += MWebBrowserUc_Loaded;
            WebTabControl.SelectionChanged += WebTabControl_SelectionChanged;
        }

        private void MWebBrowserUc_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsInDesignMode())
                return;
            InitCommand();
            InitData();
            InitSearchText();
            InitWebMenu();
            TabItemAdd("http://www.baidu.com");
        }

        #region InitData
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

            DownloadTool.ShowDownloadTabEvent += ShowDownloadTab;
        }

        private void InitCommand()
        {
            WebTabControl.TabItemAddCommand = new BaseCommand<object>(TabItemAdd);
        }
        #endregion

        #region DownloadTool

        private void ShowDownloadTab()
        {
            foreach (var tabItem in WebTabControl.Items)
            {
                if (tabItem is TabItem tI)
                {
                    if (tI.Content is DownloadShowAllUc)
                    {
                        WebTabControl.SelectedItem = tabItem;
                        return;
                    }
                }
            }

            GlobalControl.DownloadShowAll ??= new DownloadShowAllUc();
            var item = new TabItem { Content = GlobalControl.DownloadShowAll };
            item.SetValue(HeaderedContentControl.HeaderProperty, "下载");
            var source = Application.Current.FindResource($"DrawingImage.File") as ImageSource;
            item.SetValue(AttachedPropertyClass.ImageSourceProperty, source);
            WebTabControl.Items.Add(item);
            WebTabControl.SelectedItem = item;
            WebTabControl.SetHeaderPanelWidth();
        }
        #endregion

        #region TabControl

        /// <summary>
        /// TabControl KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebTabControlUc_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F5) return;
            if (!(WebTabControl.SelectedItem is TabItem item)) return;
            if (!(item.Content is WebTabItemUc webTabItemUc)) return;
            webTabItemUc.CefWebBrowser?.Reload();
        }

        /// <summary>
        /// 添加新的TabItem
        /// </summary>
        /// <param name="obj"></param>
        private void TabItemAdd(object obj)
        {
            try
            {
                var uc = new WebTabItemUc { ViewModel = { CurrentUrl = obj?.ToString() } };
                uc.CefWebBrowser.DownloadCallBackEvent += DownloadTool.DownloadFile;
                uc.WebMouseWheelEvent += WebMouseWheel;
                #region TabItem

                var item = new TabItem { Content = uc };
                var titleBind = new Binding { Source = uc.DataContext, Path = new PropertyPath("Title") };
                item.SetBinding(HeaderedContentControl.HeaderProperty, titleBind);
                var faviconBind = new Binding { Source = uc.DataContext, Path = new PropertyPath("Favicon") };
                item.SetBinding(AttachedPropertyClass.ImageSourceProperty, faviconBind);
                WebTabControl.Items.Add(item);
                WebTabControl.SelectedItem = item;
                WebTabControl.SetHeaderPanelWidth();
                #endregion

            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// TabControl 选中值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(WebTabControl.SelectedItem is TabItem item)) return;
            if (!(item.Content is WebTabItemUc webTabItemUc)) return;
            _currentWebTabItem = webTabItemUc;
            SetCurrentSelectedInfo();
        }

        /// <summary>
        /// 设置当前选中信息
        /// </summary>
        private void SetCurrentSelectedInfo()
        {
            _viewModel.CurrentUrl = _currentWebTabItem.ViewModel.CurrentUrl;
        }

        #endregion

        #region 搜索框

        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationBack_OnClick(object sender, RoutedEventArgs e)
        {
            _currentWebTabItem?.CefWebBrowser.Back();
        }
        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationForward_OnClick(object sender, RoutedEventArgs e)
        {
            _currentWebTabItem?.CefWebBrowser.Forward();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            _currentWebTabItem?.CefWebBrowser.Reload();
        }

        /// <summary>
        /// 搜索框KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var pattern = @"^(http://|https://)?((?:[A-Za-z0-9]+-[A-Za-z0-9]+|[A-Za-z0-9]+)\.)+([A-Za-z]+)[/\?\:]?.*$";
            var match = Regex.Match(_viewModel.CurrentUrl, pattern);

            if (!match.Success) return;
            if (string.IsNullOrEmpty(_viewModel.CurrentUrl)) return;
            _currentWebTabItem.Load(_viewModel.CurrentUrl);

            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                //使search框失去焦点
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            });
        }

        #endregion


        #region 缩放

        private void ZoomToolTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_zoomWaitingCount > 2)
            {
                _zoomToolTimer?.Stop();
                _viewModel.ZoomIsChecked = false;
                _viewModel.ZoomStaysOpen = false;
                _zoomWaitingCount = -1;
                return;
            }

            if (_zoomWaitingCount > -1)
            {
                _zoomWaitingCount++;
            }
        }

        private void ZoomIn()
        {
            if (_currentWebTabItem.CefWebBrowser.ZoomLevel < 4)
            {
                _currentWebTabItem.CefWebBrowser.ZoomInCommand.Execute(null);
            }
            _viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private void ZoomOut()
        {
            if (_currentWebTabItem.CefWebBrowser.ZoomLevel > -4)
            {
                _currentWebTabItem.CefWebBrowser.ZoomOutCommand.Execute(null);
            }
            _viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private void ZoomReset()
        {
            _currentWebTabItem.CefWebBrowser.ZoomResetCommand.Execute(null);
            // CefWebBrowser.SetZoomLevel(0);
            SetSearchZoomStatus();
        }

        private void SetSearchZoomStatus()
        {
            if (null == _currentWebTabItem) return;
            if (_currentWebTabItem.CefWebBrowser.ZoomLevel < 0)
            {
                _viewModel.ZoomLevelType = ZoomType.Out;
                _viewModel.ZoomIsChecked = true;
                if (_currentWebTabItem.CefWebBrowser.ZoomLevel > -1)
                {
                    _viewModel.ZoomRatio = "90%";
                }
                else if (_currentWebTabItem.CefWebBrowser.ZoomLevel <= 1)
                {
                    var radio = Math.Round((_currentWebTabItem.CefWebBrowser.ZoomLevel + 5) / 5 * 100);
                    _viewModel.ZoomRatio = $"{radio}%";
                }
            }
            else if (_currentWebTabItem.CefWebBrowser.ZoomLevel > 0)
            {
                _viewModel.ZoomLevelType = ZoomType.In;
                _viewModel.ZoomIsChecked = true;
                var radio = Math.Round((1 + _currentWebTabItem.CefWebBrowser.ZoomLevel) * 100, 2);
                _viewModel.ZoomRatio = $"{radio}%";
            }
            else
            {
                _viewModel.ZoomLevelType = ZoomType.None;
                _viewModel.ZoomIsChecked = false;
            }
        }
        #endregion

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
            WebMenu.ZoomInEvent += () =>
            {
                ZoomIn();
            };
            WebMenu.ZoomOutEvent += () =>
            {
                ZoomOut();
            };
        }

        private void WebMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                _viewModel.ZoomStaysOpen = false;
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
    }
}