using System.Windows;
using Cys_CustomControls.Controls;

namespace MWebBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string url = "http://www.baidu.com";
            //MWebBrowser.Load(url);                
        }
    }
}
