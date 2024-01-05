using CefSharp;
using CefSharp.Wpf;
using Cys_Controls.Code;
using Cys_CustomControls.Controls;
using System;
using System.Windows.Input;

namespace MWebBrowserWindow.Code.CustomCef
{
    public class CustomWindowWebBrowser: ChromiumWebBrowser
    {
        public Action<bool, DownloadItem> DownloadCallBackEvent;

        public Action AfterLoadEvent;
        public CustomWindowWebBrowser()
        {
            this.LoadingStateChanged += CustomWindowWebBrowser_LoadingStateChanged;
            this.IsBrowserInitializedChanged += CustomWindowWebBrowser_IsBrowserInitializedChanged;
        }

        private void CustomWindowWebBrowser_IsBrowserInitializedChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsBrowserInitialized) return;
            //this.ShowDevTools();
            Cef.UIThreadTaskFactory.StartNew(() =>
            {
                string error = "";
                var requestContext = this.GetBrowser().GetHost().RequestContext;
                requestContext.SetPreference("profile.default_content_setting_values.plugins", 1, out error);
            });
        }

        private void CustomWindowWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
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
