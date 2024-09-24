using MWebBrowser.Code.CustomCef;
using MWebBrowser.Code.Helpers;
using MWebBrowser.View;
using System.Windows;
using System.Windows.Controls;

namespace MWebBrowser.Code.Helpers
{
    public static class F11Helper
    {
        private static F11Window f11Window;
        public static void F11(Grid parent, CustomWebBrowser CefWebBrowser)
        {
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                if (f11Window != null)
                {
                    ExitFullscreen(CefWebBrowser);
                    parent.Children.Add(CefWebBrowser);
                }
                else
                {
                    parent.Children.Remove(CefWebBrowser);
                    EnterFullscreen(CefWebBrowser);
                }
            });
        }

        private static void EnterFullscreen(CustomWebBrowser CefWebBrowser)
        {
            f11Window = new F11Window();
            f11Window.Parent.Children.Add(CefWebBrowser);
            f11Window.Show();
            Application.Current.MainWindow.Hide();
        }

        private static void ExitFullscreen(CustomWebBrowser CefWebBrowser)
        {
            f11Window.Parent.Children.Remove(CefWebBrowser);
            Application.Current.MainWindow.Show();
            f11Window.Close();
            f11Window = null;
        }
    }
}
