using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Experimental;
using MWinFormsCore.Code.CustomCef;
using System.Runtime.InteropServices;

namespace MWPFCore.Code.CustomCef
{
    public class CustomWebBrowser: ChromiumWebBrowser
    {
        public Action<bool, DownloadItem> DownloadCallBackEvent;
        public Action AfterLoadEvent;
        public Action<string> OpenNewTabEvent;
        public Action<int> MouseWheelEvent;

        private ChromiumWidgetNativeWindow messageInterceptor;

        public CustomWebBrowser()
        {
            this.LoadingStateChanged += CustomWebBrowser_LoadingStateChanged;
            this.IsBrowserInitializedChanged += CustomWebBrowser_IsBrowserInitializedChanged;
            this.LifeSpanHandler = new CustomLifeSpanHandler();
            this.RequestHandler = new CustomRequestHandler();
            this.DownloadHandler = new CustomDownloadHandler(DownloadCallBackEvent);
            this.RequestContext = new RequestContext();
            this.MouseWheel += CustomWebBrowser_MouseWheel;
        }

        private void CustomWebBrowser_MouseWheel(object? sender, MouseEventArgs e)
        {
            
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
