using MWebBrowserForm;
using System.Windows;

namespace MWebBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitCefSharp.InitializeCefSharp();
        }
    }
}
