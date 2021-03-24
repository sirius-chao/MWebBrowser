using System;
using System.Windows.Controls;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for DownloadShowAllItemUc.xaml
    /// </summary>
    public partial class DownloadShowAllItemUc : UserControl
    {
        public Action<DownloadShowAllItemUc> RemoveFileEvent;
        public DownloadShowAllItemUc()
        {
            InitializeComponent();
        }

        private void RemoveFile_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoveFileEvent?.Invoke(this);
        }
    }
}
