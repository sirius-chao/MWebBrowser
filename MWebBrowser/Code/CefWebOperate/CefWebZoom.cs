using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using MWebBrowser.View;
using MWebBrowser.ViewModel;
using System;
using System.Windows.Input;

namespace MWebBrowser.Code.CefWebOperate
{
    public class CefWebZoom
    {
        private readonly System.Timers.Timer zoomToolTimer = new System.Timers.Timer(1000);
        private int zoomWaitingCount = -1;

        private readonly WebMenuUc webMenuUc;
        private readonly WebTabControlViewModel viewModel;
        private WebTabItemUc currentWebTabItem;
        private readonly MSearchText mSearchText;
        public CefWebZoom(WebMenuUc webMenuUc, WebTabControlViewModel viewModel, MSearchText mSearchText)
        {
            this.webMenuUc = webMenuUc;
            this.viewModel = viewModel;
            this.mSearchText = mSearchText;
            this.webMenuUc.ZoomInEvent += ZoomIn;
            this.webMenuUc.ZoomOutEvent += ZoomOut;
            InitZoomCommand();
        }

        public void SetWebTabItem(WebTabItemUc currentWebTabItem)
        {
            this.currentWebTabItem = currentWebTabItem;
        }

        #region Zoom

        public void WebMouseWheelZoom(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                viewModel.ZoomStaysOpen = false;
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
                zoomWaitingCount = 0;
                zoomToolTimer.Elapsed -= ZoomToolTimer_Elapsed;
                zoomToolTimer.Elapsed += ZoomToolTimer_Elapsed;
                zoomToolTimer.AutoReset = true;
                zoomToolTimer.Enabled = true;
                e.Handled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void InitZoomCommand()
        {
            mSearchText.ZoomInCommand = new BaseCommand<object>((obj) =>
            {
                ZoomIn();
            });
            mSearchText.ZoomOutCommand = new BaseCommand<object>((obj) =>
            {
                ZoomOut();
            });
            mSearchText.ZoomResetCommand = new BaseCommand<object>((obj) =>
            {
                ZoomReset();
            });
        }
        private void ZoomToolTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (zoomWaitingCount > 2)
            {
                zoomToolTimer?.Stop();
                viewModel.ZoomIsChecked = false;
                viewModel.ZoomStaysOpen = false;
                zoomWaitingCount = -1;
                return;
            }

            if (zoomWaitingCount > -1)
            {
                zoomWaitingCount++;
            }
        }

        private void ZoomIn()
        {
            if (currentWebTabItem.CefWebBrowser.ZoomLevel < 4)
            {
                currentWebTabItem.CefWebBrowser.ZoomInCommand.Execute(null);
            }
            viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private void ZoomOut()
        {
            if (currentWebTabItem.CefWebBrowser.ZoomLevel > -4)
            {
                currentWebTabItem.CefWebBrowser.ZoomOutCommand.Execute(null);
            }
            viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private void ZoomReset()
        {
            currentWebTabItem.CefWebBrowser.ZoomResetCommand.Execute(null);
            SetSearchZoomStatus();
        }

        private void SetSearchZoomStatus()
        {
            if (null == currentWebTabItem) return;
            if (currentWebTabItem.CefWebBrowser.ZoomLevel < 0)
            {
                viewModel.ZoomLevelType = ZoomType.Out;
                viewModel.ZoomIsChecked = true;
                if (currentWebTabItem.CefWebBrowser.ZoomLevel > -1)
                {
                    viewModel.ZoomRatio = "90%";
                }
                else if (currentWebTabItem.CefWebBrowser.ZoomLevel <= 1)
                {
                    var radio = Math.Round((currentWebTabItem.CefWebBrowser.ZoomLevel + 5) / 5 * 100);
                    viewModel.ZoomRatio = $"{radio}%";
                }
            }
            else if (currentWebTabItem.CefWebBrowser.ZoomLevel > 0)
            {
                viewModel.ZoomLevelType = ZoomType.In;
                viewModel.ZoomIsChecked = true;
                var radio = Math.Round((1 + currentWebTabItem.CefWebBrowser.ZoomLevel) * 100, 2);
                viewModel.ZoomRatio = $"{radio}%";
            }
            else
            {
                viewModel.ZoomLevelType = ZoomType.None;
                viewModel.ZoomIsChecked = false;
            }
            webMenuUc.ZoomCallBack(viewModel.ZoomRatio);
        }
        #endregion
    }
}
