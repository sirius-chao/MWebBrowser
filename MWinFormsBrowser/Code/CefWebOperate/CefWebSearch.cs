using CefSharp;
using MWinFormsBrowser.Code.Helpers;
using MWinFormsBrowser.View;
using MWinFormsBrowser.ViewModel;
using System.Windows.Input;

namespace MWinFormsBrowser.Code.CefWebOperate
{
    public class CefWebSearch
    {
        private WebTabItemUc currentWebTabItem;
        private readonly WebTabControlViewModel viewModel;
        public void SetWebTabItem(WebTabItemUc currentWebTabItem)
        {
            this.currentWebTabItem = currentWebTabItem;
        }

        public CefWebSearch(WebTabControlViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        #region 搜索框
        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NavigationBack()
        {
            currentWebTabItem?.CefWebBrowser.Back();
        }
        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NavigationForward()
        {
            currentWebTabItem?.CefWebBrowser.Forward();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NavigationRefresh()
        {
            currentWebTabItem?.CefWebBrowser.Reload();
        }

        /// <summary>
        /// 搜索框KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SearchOnKeyDown(KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            currentWebTabItem.Load(viewModel.CurrentUrl);
        }
        #endregion
    }
}
