using System;
using CefSharp.Wpf;
using Cys_CustomControls.Controls;

namespace MWebBrowser.Code.CustomCef
{
    public class CustomWebBrowser: ChromiumWebBrowser
    {
        public CustomWebBrowser()
        {
            this.LifeSpanHandler = new CustomLifeSpanHandler();
        }

        public void OpenNewTab(string url)
        {
            Dispatcher.Invoke(() =>
            {
                var tabControl = ControlHelper.FindVisualParent<MTabControl>(this);
                tabControl?.TabItemAddCommand?.Execute(url);
            });
        }
    }
}
