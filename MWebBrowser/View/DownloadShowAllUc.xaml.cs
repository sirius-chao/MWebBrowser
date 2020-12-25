using System;
using System.Diagnostics;
using System.Linq;
using Cys_Common;
using MWebBrowser.ViewModel;
using System.Windows.Controls;
using MWebBrowser.Code.Configure;

namespace MWebBrowser.View
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
            InitData();
        }

        private void InitData()
        {
            FileList.Children.Clear();
            foreach (var info in GlobalInfo.DownloadSetting.DownloadItemInfos)
            {
                var viewModel = new DownloadShowAllItemViewModel
                {
                    FileName = info.FileName,
                    Url = info.Url,
                    FilePath = info.FilePath,
                };
                var uc = new DownloadShowAllItemUc {DataContext = viewModel,RemoveFileEvent = RemoveFile};
                FileList.Children.Add(uc);
            }
        }

        private void RemoveFile(DownloadShowAllItemUc fileControl)
        {
            if (fileControl.DataContext is DownloadShowAllItemViewModel viewModel)
            {
                if (GlobalInfo.DownloadSetting.DownloadItemInfos.Exists(x => viewModel.FileName == x.FileName))
                {
                    var file = GlobalInfo.DownloadSetting.DownloadItemInfos.Single(x => viewModel.FileName == x.FileName);
                    GlobalInfo.DownloadSetting.DownloadItemInfos.Remove(file);
                }
            }
            FileList.Children.Remove(fileControl);
        }

        private void Clear_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            GlobalInfo.DownloadSetting.DownloadItemInfos.Clear();
            FileList.Children.Clear();
        }

        private void OpenDownFilePath_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var filePath = KnownFolderHelper.GetDownload();
                var psi = new ProcessStartInfo() { FileName = filePath, UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
