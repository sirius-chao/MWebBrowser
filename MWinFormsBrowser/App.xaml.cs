using MWinFormsBrowser;
using System.Windows;

namespace MWinFormsBrowser
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
