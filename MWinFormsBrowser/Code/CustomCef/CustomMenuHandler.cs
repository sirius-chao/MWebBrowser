using CefSharp;
using MWinFormsCore.Code;

namespace MWinFormsCore.CustomCef
{
    public class CustomMenuHandler : IContextMenuHandler
    {
        private const int ShowDevTools = 26501;
        private const int CloseDevTools = 26502;

        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.AddItem((CefMenuCommand)ShowDevTools, "Show DevTools");
            model.AddItem((CefMenuCommand)CloseDevTools, "Close DevTools");
        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (chromiumWebBrowser is CustomWebBrowser cefWebBrowser)
            {
                var parent =  UserControlHelper.FindParent<BrowserUserControl>(cefWebBrowser);
                if ((int)commandId == ShowDevTools)
                {
                    parent.Invoke(() => { parent.ShowDevToolsDocked(); });
                }
                if ((int)commandId == CloseDevTools)
                {
                    parent.Invoke(() => { parent.CloseDevToolsDocked(); });
                }
            }
            return false;
        }

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {

        }

        bool IContextMenuHandler.RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
