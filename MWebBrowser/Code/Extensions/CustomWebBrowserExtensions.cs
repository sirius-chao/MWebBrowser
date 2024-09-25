using CefSharp;
using CefSharp.Wpf;
using MWebBrowser.Code.CustomCef;
using MWebBrowser.Code.Helpers;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MWebBrowser.Code.Extensions
{
    public static class CustomWebBrowserExtensions
    {
        public static void AddCustomExtensions(this CustomWebBrowser customWebBrowser)
        {
            var requestContext = customWebBrowser.GetBrowserHost().RequestContext;
            var dir = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\Extensions");
            dir = Path.GetFullPath(dir);
            if (!Directory.Exists(dir))
            {
                return;
            }

            var extensionHandler = new CustomExtensionHandler
            {
                LoadExtensionPopup = (url) =>
                {
                    DispatcherHelper.UIDispatcher.Invoke(new Action(() =>
                    {
                        var extensionWindow = new Window();

                        var extensionBrowser = new ChromiumWebBrowser(url);
                        extensionWindow.Content = extensionBrowser;

                        extensionWindow.Show();
                    }));
                },
                GetActiveBrowser = (extension, isIncognito) =>
                {
                    return customWebBrowser.BrowserCore;
                }
            };
            requestContext.LoadExtensionsFromDirectory(dir,extensionHandler);
        }
    }
}
