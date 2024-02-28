using CefSharp;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using Cys_Model.Tables;
using Cys_Resource.Code;
using Cys_Services;
using MWebBrowser.Code;
using MWebBrowser.Code.CefWebOperate;
using MWebBrowser.Code.Helpers;
using MWebBrowser.ViewModel;
using System;
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
        private readonly WebTabControlViewModel viewModel;
        private WebTabItemUc currentWebTabItem;
        private HistoryServices historyServices;
        private CefWebZoom cefWebZoom;
        private CefWebSearch cefWebSearch;
        public WebTabControlUc()
        {
            InitializeComponent();
            InitWebTabControl();
            historyServices = new HistoryServices();
            viewModel = new WebTabControlViewModel();
            this.DataContext = viewModel;
            this.Loaded += MWebBrowserUc_Loaded;
            WebTabControl.SelectionChanged += WebTabControl_SelectionChanged;
            FavoritesMenu.GetWebUrlEvent += () => viewModel;
            FavoritesMenu.OpenUrlCurrentEvent += OpenUrlByCurrentTab;
            FavoritesMenu.RefreshFavoritesBarEvent += FavoritesBar.RefreshFavoritesBar;
            FavoritesBar.GetWebUrlEvent += () => viewModel;
            FavoritesBar.OpenUrlCurrentEvent += OpenUrlByCurrentTab;
        }

        public void SetCurrentWebTabItemSize(bool setHeight)
        {
            currentWebTabItem.RowBottom.Height = new GridLength(setHeight ? 40 : 0);
        }
        private void MWebBrowserUc_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsInDesignMode())
                return;
            InitCommand();
            InitData();
            OpenUrl("https://www.baidu.com/s?ie=utf-8&f=3&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E6%90%9C%E7%8B%97%E8%BE%93%E5%85%A5%E6%B3%95&fenlei=256&rsv_pq=0xa88a560e000d9ff0&rsv_t=d67ai6Q9iEfzJfak294KtSxhI5eqP%2F0dHPREfZ2%2BeoZwwZQaIOzg2k%2FUUrCb&rqlang=en&rsv_enter=1&rsv_dl=ts_1&rsv_sug3=5&rsv_sug1=4&rsv_sug7=100&rsv_sug2=1&rsv_btype=i&prefixsug=sou%2526%252339%253Bg&rsp=1&inputT=6684&rsv_sug4=7511");
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
            cefWebZoom = new CefWebZoom(WebMenu, viewModel, SearchText);
            cefWebSearch = new CefWebSearch(viewModel);
            DownloadTool.ShowDownloadTabEvent += ShowDownloadTab;
            WebMenu.ExecuteMenuEvent += ExecuteMenuFunction;
        }

        private void InitCommand()
        {
            WebTabControl.TabItemAddCommand = new BaseCommand<object>(OpenDefault);
            WebTabControl.TabItemRemoveCommand = new BaseCommand<object>(RemoveCurrentItem);
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

        private void Print() => currentWebTabItem.CefWebBrowser.Print();

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

        public void OpenUrlByCurrentTab(object obj)
        {
            currentWebTabItem.Load(obj?.ToString());
        }

        public void OpenUrl(object obj)
        {
            AddNewTabItem(obj, false);
        }

        public void OpenDefault(object obj)
        {
            AddNewTabItem(obj, true);
        }
        /// <summary>
        /// 添加新的TabItem
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewTabItem(object obj, bool firstNew)
        {
            try
            {
                DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    var uc = new WebTabItemUc { ViewModel = { FirstNew = firstNew, CurrentUrl = firstNew ? null : obj?.ToString() } };
                    uc.SetCurrentEvent += SetCurrentSelectedInfo;
                    uc.CefWebBrowser.AfterLoadEvent += AfterLoad;
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
        public void RemoveCurrentItem(object obj)
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
        private async void AfterLoad(bool isFirstLoad)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    viewModel.CurrentUrl = currentWebTabItem.CefWebBrowser.Address;
                    if (string.IsNullOrEmpty(currentWebTabItem.CefWebBrowser.Address) && isFirstLoad)
                    {
                        RemoveCurrentItem(this.WebTabControl.SelectedValue);
                        return;
                    }
                });
                currentWebTabItem.CefWebBrowser.SetDownloadHandler(DownloadTool.DownloadFile);
                currentWebTabItem.CefWebBrowser.OpenUrlEvent += OpenUrl;
                currentWebTabItem.CefWebBrowser.MouseWheelEvent += WebMouseWheel;
                var model = new HistoryModel { Url = viewModel.CurrentUrl, VisitTime = DateTime.Now, FormVisit = 0, Title = viewModel.Title };
                await historyServices.AddHistory(model);
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
                    OpenDefault(null);
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
            if (WebTabControl.SelectedItem is TabItem { Content: WebTabItemUc webTabItemUc })
            {
                currentWebTabItem = webTabItemUc;
                cefWebZoom.SetWebTabItem(currentWebTabItem);
                cefWebSearch.SetWebTabItem(currentWebTabItem);
                SetCurrentSelectedInfo();
            }
        }

        /// <summary>
        /// 设置当前选中信息
        /// </summary>
        private void SetCurrentSelectedInfo()
        {
            viewModel.CurrentUrl = currentWebTabItem.ViewModel.CurrentUrl;
            viewModel.Title = currentWebTabItem.ViewModel.Title;
        }

        #endregion

        private void WebMouseWheel(int delta) => cefWebZoom.WebMouseWheelZoom(delta);

        #region search box
        private void NavigationBack_OnClick(object sender, RoutedEventArgs e) => cefWebSearch.NavigationBack();
        private void NavigationForward_OnClick(object sender, RoutedEventArgs e) => cefWebSearch.NavigationForward();
        private void NavigationRefresh_OnClick(object sender, RoutedEventArgs e) => cefWebSearch.NavigationRefresh();
        private void Search_OnKeyDown(object sender, KeyEventArgs e) => cefWebSearch.SearchOnKeyDown(e);
        #endregion
    }
}