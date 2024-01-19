using CefSharp;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using Cys_Model.Tables;
using Cys_Resource.Code;
using Cys_Services;
using MWebBrowser.Code;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
using System.Threading.Tasks;
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
        private readonly double _zoomLevelIncrement = 0.1;

        private HistoryServices _historyServices;
        public WebTabControlUc()
        {
            InitializeComponent();
            InitWebTabControl();
            _historyServices = new HistoryServices();
            _viewModel = new WebTabControlViewModel();
            this.DataContext = _viewModel;
            this.Loaded += MWebBrowserUc_Loaded;
            WebTabControl.SelectionChanged += WebTabControl_SelectionChanged;
            FavoritesMenu.GetWebUrlEvent += () => _viewModel;
            FavoritesMenu.OpenNewTabEvent += TabItemAdd;
            FavoritesMenu.RefreshFavoritesBarEvent += FavoritesBar.RefreshFavoritesBar;
            FavoritesBar.GetWebUrlEvent += () => _viewModel;
            FavoritesBar.OpenNewTabEvent += TabItemAdd;
        }

        private void MWebBrowserUc_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsInDesignMode())
                return;
            InitCommand();
            InitData();
            InitSearchCommand();
            InitWebMenu();
            TabItemAdd("https://www.cnblogs.com/mchao/p/14086441.html");
        }

        #region InitData

        private void InitWebMenu()
        {
            WebMenu.ZoomInEvent += ZoomIn;
            WebMenu.ZoomOutEvent += ZoomOut;
            WebMenu.ExecuteMenuEvent += ExecuteMenuFunction;
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

            DownloadTool.ShowDownloadTabEvent += ShowDownloadTab;
        }

        private void InitCommand()
        {
            WebTabControl.TabItemAddCommand = new BaseCommand<object>(TabItemAdd);
            WebTabControl.TabItemRemoveCommand = new BaseCommand<object>(RemoveItemCommand);
        }
        #endregion

        #region DownloadTool

        private void ShowDownloadTab()
        {
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

        private void Print()
        {
            _currentWebTabItem.CefWebBrowser.Print();
        }

        #region SettingTool

        private void ShowSettingTab()
        {
            bool having = false;
            foreach (var temp in WebTabControl.Items)
            {
                if (temp is TabItem)
                {
                    if (((TabItem)temp).Content is SettingUc)
                    {
                        having = true;
                        WebTabControl.SelectedItem = temp;
                        break;
                    }
                }
            }
            if (!having)
            {
                SettingUc Setting = new SettingUc();
                var item = new TabItem { Content = Setting };
                item.SetValue(HeaderedContentControl.HeaderProperty, "设置");
                WebTabControl.Items.Add(item);
                WebTabControl.SelectedItem = item;
                WebTabControl.SetHeaderPanelWidth();
            }
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
            if (!(WebTabControl.SelectedItem is TabItem item)) return;
            if (!(item.Content is WebTabItemUc webTabItemUc)) return;
            int virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
            webTabItemUc.CefWebBrowser_PreviewKeyDown(virtualKey);
        }

        /// <summary>
        /// 添加新的TabItem
        /// </summary>
        /// <param name="obj"></param>
        public void TabItemAdd(object obj)
        {
            try
            {
                DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    var uc = new WebTabItemUc { ViewModel = { CurrentUrl = obj?.ToString() } };
                    uc.SetCurrentEvent += SetCurrentSelectedInfo;
                    uc.CefWebBrowser.DownloadCallBackEvent += DownloadTool.DownloadFile;
                    uc.CefWebBrowser.AfterLoadEvent += AfterLoad;
                    uc.CefWebBrowser.OpenNewTabEvent += TabItemAdd;
                    uc.CefWebBrowser.MouseWheelEvent += WebMouseWheel;
                    #region TabItem

                    var item = new TabItem { Content = uc };
                    var titleBind = new Binding { Source = uc.DataContext, Path = new PropertyPath("Title") };
                    item.SetBinding(HeaderedContentControl.HeaderProperty, titleBind);
                    var faviconBind = new Binding { Source = uc.DataContext, Path = new PropertyPath("Favicon") };
                    item.SetBinding(AttachedPropertyClass.ImageSourceProperty, faviconBind);
                    WebTabControl.Items.Add(item);
                    WebTabControl.SelectedItem = item;
                    WebTabControl.SetHeaderPanelWidth();
                });
                #endregion
            }
            catch (Exception ex)
            {

            }
        }
        public void RemoveItemCommand(object obj)
        {
            if (obj is TabItem item)
            {
                WebTabControl.Items.Remove(item);

                if (item.Content is WebTabItemUc webTabItem)
                {
                    webTabItem.Dispose();
                }
            }
            WebTabControl.SetHeaderPanelWidth();

            if (WebTabControl.Items.Count <= 0)
            {
                WebTabControl.CloseTabEvent?.Invoke();
            }
        }
        private async void AfterLoad()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    //_viewModel.Title = _currentWebTabItem.CefWebBrowser.Title;
                    _viewModel.CurrentUrl = _currentWebTabItem.CefWebBrowser.Address;
                });

                var model = new HistoryModel { Url = _viewModel.CurrentUrl, VisitTime = DateTime.Now, FormVisit = 0, Title = _viewModel.Title };
                await _historyServices.AddHistory(model);
            }
            catch (Exception ex)
            {
            }
        }
        private void ExecuteMenuFunction(string obj)
        {
            switch (obj)
            {
                case "0":
                    TabItemAdd("http://www.baidu.com");
                    break;
                case "4":
                    FavoritesMenu.FavoritesButton.IsChecked = true;
                    break;
                case "5":
                    History.HistoryButton.IsChecked = true;
                    break;
                case "6":
                    ShowDownloadTab();
                    break;
                case "10":
                    Print();
                    break;
                case "15":
                    ShowSettingTab();
                    break;
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
            _viewModel.Title = _currentWebTabItem.ViewModel.Title;
        }

        #endregion

        #region 搜索框
        private void InitSearchCommand()
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
            _currentWebTabItem.Load(_viewModel.CurrentUrl);
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                //使search框失去焦点
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            });
        }

        #endregion

        #region 缩放
        private void WebMouseWheel(int delta)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                _viewModel.ZoomStaysOpen = false;
                return;
            }
            try
            {
                if (delta > 0)
                {
                    ZoomIn();
                }
                else if (delta < 0)
                {
                    ZoomOut();
                }
                _zoomWaitingCount = 0;
                _zoomToolTimer.Elapsed -= ZoomToolTimer_Elapsed;
                _zoomToolTimer.Elapsed += ZoomToolTimer_Elapsed;
                _zoomToolTimer.AutoReset = true;
                _zoomToolTimer.Enabled = true;
            }
            catch (Exception ex)
            {

            }
        }
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

        private async void ZoomIn()
        {
            double zoomLevel = (int)await _currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
            if (zoomLevel < 8)
            {
                var task = _currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
                await task.ContinueWith(previous =>
                {
                    if (previous.Status == TaskStatus.RanToCompletion)
                    {
                        var currentLevel = previous.Result;
                        _currentWebTabItem.CefWebBrowser.SetZoomLevel(currentLevel + _zoomLevelIncrement);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected failure of calling CEF->GetZoomLevelAsync", previous.Exception);
                    }
                }, TaskContinuationOptions.ExecuteSynchronously); 
            }
            else
            {
                _currentWebTabItem.CefWebBrowser.SetZoomLevel(8);
            }
            _viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private async void ZoomOut()
        {
            double zoomLevel = await _currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
            if (zoomLevel > -7.6)
            {
                var task = _currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
                await task.ContinueWith(previous =>
                {
                    if (previous.Status == TaskStatus.RanToCompletion)
                    {
                        var currentLevel = previous.Result;
                        _currentWebTabItem.CefWebBrowser.SetZoomLevel(currentLevel - _zoomLevelIncrement);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected failure of calling CEF->GetZoomLevelAsync", previous.Exception);
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);
            }
            else
            {
                _currentWebTabItem.CefWebBrowser.SetZoomLevel(-7.6);
            } 
            _viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private async void ZoomReset()
        {
            _currentWebTabItem.CefWebBrowser.SetZoomLevel(0);
            SetSearchZoomStatus();
        }

        private async void SetSearchZoomStatus()
        {
            double zoomLevel = await _currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
            if (null == _currentWebTabItem) return;
            if (zoomLevel < 0)
            {
                _viewModel.ZoomLevelType = ZoomType.Out;
                _viewModel.ZoomIsChecked = true;
                double minZoomLevel = -7.6;
                double maxZoomLevel = 1;
                double minPercentage = 25;
                double maxPercentage = 100;
                double percentage = minPercentage + (maxPercentage - minPercentage) * (zoomLevel - minZoomLevel) / (maxZoomLevel - minZoomLevel);
                _viewModel.ZoomRatio = $"{Math.Round(percentage, 2)}%";
            }
            else if(zoomLevel==0)
            {
                _viewModel.ZoomLevelType = ZoomType.None;
                _viewModel.ZoomIsChecked = true;
                _viewModel.ZoomRatio = $"{100}%";
            }
            else if (zoomLevel > 0)
            {
                _viewModel.ZoomLevelType = ZoomType.In;
                _viewModel.ZoomIsChecked = true;
                var radio = Math.Round((1 + zoomLevel) * 100, 2);
                _viewModel.ZoomRatio = $"{radio}%";
            }
            else
            {
                _viewModel.ZoomLevelType = ZoomType.None;
                _viewModel.ZoomIsChecked = false;
            }
            WebMenu.ZoomCallBack(_viewModel.ZoomRatio);
        }
        #endregion
    }
}