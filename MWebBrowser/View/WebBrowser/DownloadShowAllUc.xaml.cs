using System.Windows.Controls;

namespace MWebBrowser.View.WebBrowser
{
    /// <summary>
    /// Interaction logic for DownloadShowAllUc.xaml
    /// </summary>
    public partial class DownloadShowAllUc : UserControl
    {
        public DownloadShowAllUc()
        {
            InitializeComponent();
            this.Loaded += DownloadShowAllUc_Loaded;
        }

        private void DownloadShowAllUc_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GetHistory(); 
            GetCurrent();
        }

        private void GetHistory()
        {

        }

        private void GetCurrent()
        {

        }
    }
}
