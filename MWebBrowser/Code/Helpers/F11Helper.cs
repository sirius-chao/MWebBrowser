using MWebBrowser.View;
using MWinFormsCore;
using System.Windows;
using System.Windows.Forms.Integration;

namespace MWebBrowser.Code.Helpers
{
    public static class F11Helper
    {
        private static F11Window f11Window;
        private static Rect originalBounds;

        public static void F11(BrowserUserControl browserUserControl, WindowsFormsHost orgWebFormsHost)
        {
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                if (f11Window != null)
                {
                    ExitFullscreen(browserUserControl, orgWebFormsHost);
                }
                else
                {
                    EnterFullscreen(browserUserControl, orgWebFormsHost);
                }
            });
        }

        private static void EnterFullscreen(BrowserUserControl browserUserControl, WindowsFormsHost orgWebFormsHost)
        {
            originalBounds = new Rect(
                browserUserControl.CefWebBrowser.Margin.Left,
                browserUserControl.CefWebBrowser.Margin.Top,
                browserUserControl.CefWebBrowser.Width,
                browserUserControl.CefWebBrowser.Height
                );
            f11Window = new F11Window();
            orgWebFormsHost.Child.Controls.Remove(browserUserControl);
            f11Window.WebFormsHost.Child = browserUserControl;
            f11Window.Show();
            Application.Current.MainWindow.Hide();
        }

        private static void ExitFullscreen(BrowserUserControl browserUserControl, WindowsFormsHost orgWebFormsHost)
        {
            f11Window.WebFormsHost.Child.Controls.Remove(browserUserControl);
            orgWebFormsHost.Child = browserUserControl;
            f11Window.Close();
            f11Window = null;
            Application.Current.MainWindow.Show();
        }
    }
}
