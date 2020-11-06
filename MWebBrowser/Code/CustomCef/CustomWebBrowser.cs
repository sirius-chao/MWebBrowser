using CefSharp.Wpf;
using Cys_CustomControls.Controls;
using MWebBrowser.Code.Helpers;
using System;
using System.Windows.Input;
using CefSharp;

namespace MWebBrowser.Code.CustomCef
{
    public class CustomWebBrowser: ChromiumWebBrowser
    {
        public Action<bool, DownloadItem> DownloadCallBackEvent;
        public CustomWebBrowser()
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.LifeSpanHandler = new CustomLifeSpanHandler();
            this.RequestHandler = new CustomRequestHandler();
            this.DownloadHandler = new CustomDownloadHandler(DownloadCallBackEvent);
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
