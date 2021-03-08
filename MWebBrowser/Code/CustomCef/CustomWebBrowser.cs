using CefSharp;
using CefSharp.Wpf;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using System;
using System.Windows.Input;

namespace MWebBrowser.Code.CustomCef
{
    public class CustomWebBrowser: ChromiumWebBrowser
    {
        public Action<bool, DownloadItem> DownloadCallBackEvent;

        public Action AfterLoadEvent;
        public CustomWebBrowser()
        {
            this.LoadingStateChanged += CustomWebBrowser_LoadingStateChanged;
        }

        private void CustomWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading)
                return;
            AfterLoadEvent?.Invoke();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.LifeSpanHandler = new CustomLifeSpanHandler();
            this.RequestHandler = new CustomRequestHandler();
            this.DownloadHandler = new CustomDownloadHandler(DownloadCallBackEvent);

            //博客园友：【侠女多风尘】给出每个webBrowser单独缩放级别处理
            this.RequestContext = new RequestContext();
        }

        public void OpenNewTab(string url)
        {
            Dispatcher.Invoke(() =>
            {
                var tabControl = ControlHelper.FindVisualParent<MTabControl>(this);
                tabControl?.TabItemAddCommand?.Execute(url);
            });
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            try
            {
                foreach (var character in e.Text)
                {
                    // 字符重发
                    this.GetBrowser().GetHost()?.SendKeyEvent((int)WM.CHAR, (int)character, 0);
                }
            }
            catch (Exception ex)
            {
            }
            e.Handled = true;
        }
    }
}
