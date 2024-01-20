using CefSharp;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using MWebBrowser.View;
using MWebBrowser.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MWebBrowser.Code.CefWebOperate
{
    public class CefWebZoom
    {
        private readonly System.Timers.Timer zoomToolTimer = new System.Timers.Timer(1000);
        private int zoomWaitingCount = -1;
        private readonly double zoomLevelIncrement = 0.1;

        private readonly WebMenuUc webMenuUc;
        private readonly WebTabControlViewModel viewModel;
        private WebTabItemUc currentWebTabItem;
        private readonly MSearchText mSearchText;

        private readonly double minusZoom = -7.6;
        private readonly double positiveZoom = 8;
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

        public void WebMouseWheelZoom(int delta)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            {
                viewModel.ZoomStaysOpen = false;
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
                zoomWaitingCount = 0;
                zoomToolTimer.Elapsed -= ZoomToolTimer_Elapsed;
                zoomToolTimer.Elapsed += ZoomToolTimer_Elapsed;
                zoomToolTimer.AutoReset = true;
                zoomToolTimer.Enabled = true;
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

        private async void ZoomIn()
        {
            double zoomLevel = await currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
            if (zoomLevel < positiveZoom)
            {
                double currentLevel = zoomLevel + zoomLevelIncrement;
                currentWebTabItem.CefWebBrowser.SetZoomLevel(currentLevel);
            }
            else
            {
                currentWebTabItem.CefWebBrowser.SetZoomLevel(positiveZoom);
            }
            viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }

        private async void ZoomOut()
        {
            double zoomLevel = await currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
            if (zoomLevel > minusZoom)
            {
                double currentLevel = zoomLevel - zoomLevelIncrement;
                currentWebTabItem.CefWebBrowser.SetZoomLevel(currentLevel);
            }
            else
            {
                currentWebTabItem.CefWebBrowser.SetZoomLevel(minusZoom);
            }
            viewModel.ZoomStaysOpen = true;
            SetSearchZoomStatus();
        }


        private void ZoomReset()
        {
            currentWebTabItem.CefWebBrowser.SetZoomLevel(0);
            SetSearchZoomStatus();
        }

        private async void SetSearchZoomStatus()
        {
            double zoomLevel = await currentWebTabItem.CefWebBrowser.GetZoomLevelAsync();
            if (null == currentWebTabItem) return;
            if (zoomLevel < 0)
            {
                viewModel.ZoomLevelType = ZoomType.Out;
                viewModel.ZoomIsChecked = true;
                double minZoomLevel = -7.6;
                double maxZoomLevel = 1;
                double minPercentage = 25;
                double maxPercentage = 100;
                double percentage = minPercentage + (maxPercentage - minPercentage) * (zoomLevel - minZoomLevel) / (maxZoomLevel - minZoomLevel);
                viewModel.ZoomRatio = $"{Math.Round(percentage, 2)}%";
            }
            else if (zoomLevel == 0)
            {
                viewModel.ZoomLevelType = ZoomType.None;
                viewModel.ZoomIsChecked = true;
                viewModel.ZoomRatio = $"{100}%";
            }
            else if (zoomLevel > 0)
            {
                viewModel.ZoomLevelType = ZoomType.In;
                viewModel.ZoomIsChecked = true;
                var radio = Math.Round((1 + zoomLevel) * 100, 2);
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
