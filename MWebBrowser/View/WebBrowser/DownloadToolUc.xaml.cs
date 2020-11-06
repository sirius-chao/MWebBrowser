using CefSharp;
using MWebBrowser.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MWebBrowser.View.WebBrowser
{
    /// <summary>
    /// Interaction logic for DownloadToolUc.xaml
    /// </summary>
    public partial class DownloadToolUc : UserControl
    {
        private readonly Dictionary<int,DownloadToolItemViewModel> _downloadDict;
        public DownloadToolUc()
        {
            _downloadDict = new Dictionary<int, DownloadToolItemViewModel>();
            InitializeComponent();
        }

        private void CloseDownloadTool_OnClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ShowAll_OnClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void DownloadFile(bool isUpdate, DownloadItem downloadItem)
        {
            if (!isUpdate)
            {
                if (_downloadDict.ContainsKey(downloadItem.Id)) return;
                var viewModel = new DownloadToolItemViewModel
                {
                    FileName = downloadItem.SuggestedFileName,
                };
                _downloadDict.Add(downloadItem.Id, viewModel);
                
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Visibility = Visibility.Visible;
                    var item = new DownloadToolItemUc { DataContext = viewModel };
                    ItemsParent.Children.Insert(0, item);
                }));

            }
            else
            {
                if (!_downloadDict.ContainsKey(downloadItem.Id)) return;
                var item = _downloadDict[downloadItem.Id];
                item.CurrentSizeStr = item.ConvertFileSize(downloadItem.ReceivedBytes);
                item.TotalSizeStr = downloadItem.TotalBytes <= 0 ? "未知" : item.ConvertFileSize(downloadItem.TotalBytes);
                item.TotalSize = downloadItem.TotalBytes > downloadItem.ReceivedBytes
                    ? downloadItem.TotalBytes
                    : downloadItem.ReceivedBytes;
                item.CurrentSize = downloadItem.ReceivedBytes;
            }
        }
    }
}
