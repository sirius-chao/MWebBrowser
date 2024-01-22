using CefSharp;
using CefSharp.Handler;

namespace MWinFormsCore.CustomCef
{
    public class CustomKeyboardHandler: KeyboardHandler
    {
        public Action<int> KeyboardCallBack;
        protected override bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (chromiumWebBrowser.IsBrowserInitialized)
            {
                KeyboardCallBack?.Invoke(windowsKeyCode);
            }
            return base.OnKeyEvent(chromiumWebBrowser, browser, type, windowsKeyCode, nativeKeyCode, modifiers, isSystemKey);
        }
    }
}
