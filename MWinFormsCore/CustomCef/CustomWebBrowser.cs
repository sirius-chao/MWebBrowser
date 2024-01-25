using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Experimental;
using CefSharp.WinForms.Handler;
using MWinFormsCore.CustomCef;
using System.Runtime.InteropServices;

namespace MWinFormsCore.CustomCef
{
    public class CustomWebBrowser: ChromiumWebBrowser
    {
        public Action<bool, DownloadItem> DownloadCallBackEvent;
        public Action AfterLoadEvent;
        public Action<string> OpenUrlEvent;
        public Action<int> MouseWheelEvent;
        private ChromiumWidgetNativeWindow messageInterceptor;

        public CustomWebBrowser()
        {
            this.LoadingStateChanged += CustomWebBrowser_LoadingStateChanged;
            this.IsBrowserInitializedChanged += CustomWebBrowser_IsBrowserInitializedChanged;
            this.RequestHandler = new CustomRequestHandler();
            this.DownloadHandler = new CustomDownloadHandler(DownloadCallBackEvent);
            this.RequestContext = new RequestContext();
            this.MenuHandler = new CustomMenuHandler();
            this.KeyboardHandler = new CustomKeyboardHandler();
            InitLifeSpanHandler();
        }

        public void InitLifeSpanHandler()
        {
            this.LifeSpanHandler = CefSharp.WinForms.Handler.LifeSpanHandler
                .Create()
                .OnBeforePopupCreated((chromiumWebBrowser, b, frame, targetUrl, targetFrameName, targetDisposition, userGesture, browserSettings) =>
                {
                    return PopupCreation.Continue;
                })
                .OnPopupCreated((ctrl, targetUrl) =>
                {
                    this.OpenUrlEvent?.Invoke(targetUrl);
                })
                .OnPopupBrowserCreated((ctrl, popupBrowser) =>
                {
                    //The host control maybe null if the popup was hosted in a native Window e.g. Devtools by default
                    if (ctrl == null)
                    {
                        return;
                    }

                    //You can access all the core browser functionality via IBrowser
                    //frames, browwser host, etc.
                    var isPopup = popupBrowser.IsPopup;
                })
                 .OnPopupDestroyed((ctrl, popupBrowser) =>
                 {
                     //If we docked  DevTools (hosted it ourselves rather than the default popup)
                     //Used when the BrowserTabUserControl.ShowDevToolsDocked method is called
                     if (popupBrowser.MainFrame.Url.Equals("devtools://devtools/devtools_app.html"))
                     {
                         //Dispose of the parent control we used to host DevTools, this will release the DevTools window handle
                         //and the ILifeSpanHandler.OnBeforeClose() will be call after.
                         ctrl.Dispose();
                     }
                     else
                     {
                         //If browser is disposed or the handle has been released then we don't
                         //need to remove the tab in this example. The user likely used the
                         // File -> Close Tab menu option which also calls BrowserForm.RemoveTab
                         if (!ctrl.IsDisposed && ctrl.IsHandleCreated)
                         {
                             ctrl.Dispose();
                         }
                     }
                 })
                .Build();
        }
        private void CustomWebBrowser_IsBrowserInitializedChanged(object? sender, EventArgs e)
        {
            if (!this.IsBrowserInitialized) return;
            Cef.UIThreadTaskFactory.StartNew(() =>
            {
                string error = "";
                var requestContext = this.GetBrowser().GetHost().RequestContext;
                requestContext.SetPreference("profile.default_content_setting_values.plugins", 1, out error);
            });

            SetupMessageInterceptor();
        }

        private void CustomWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading)
                return;
            AfterLoadEvent?.Invoke();
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private void SetupMessageInterceptor()
        {
            if (messageInterceptor != null)
            {
                messageInterceptor.ReleaseHandle();
                messageInterceptor = null;
            }

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        IntPtr chromeWidgetHostHandle;
                        if (ChromiumRenderWidgetHandleFinder.TryFindHandle(this, out chromeWidgetHostHandle))
                        {
                            messageInterceptor = new ChromiumWidgetNativeWindow(this, chromeWidgetHostHandle);

                            messageInterceptor.OnWndProc(message =>
                            {
                                const int WM_MOUSEACTIVATE = 0x0021;
                                const int WM_NCLBUTTONDOWN = 0x00A1;
                                const int WM_DESTROY = 0x0002;
                                const int WM_MOUSEWHEEL = 0x020A;

                                // Render process switch happened, need to find the new handle
                                if (message.Msg == WM_DESTROY)
                                {
                                    SetupMessageInterceptor();
                                    return false;
                                }

                                if (message.Msg == WM_MOUSEACTIVATE)
                                {
                                    // http://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/ToolStripManager.cs,1249
                                    var topLevelWindowHandle = message.WParam;
                                    PostMessage(topLevelWindowHandle, WM_NCLBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
                                }

                                if(message.Msg == WM_MOUSEWHEEL)
                                {
                                    int rawDelta = (int)message.WParam.ToInt64();
                                    int delta = rawDelta >> 16; // 使用右移16位来获取高位字
                                    MouseWheelEvent?.Invoke(delta);
                                }
                                return false;
                            });

                            break;
                        }
                        else
                        {
                            // Chrome hasn't yet set up its message-loop window.
                            await Task.Delay(10);
                        }
                    }
                }
                catch
                {
                    // Errors are likely to occur if browser is disposed, and no good way to check from another thread
                }
            });
        }
    }
}
