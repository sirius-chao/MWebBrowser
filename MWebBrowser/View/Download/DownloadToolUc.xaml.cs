using CefSharp;
using MWebBrowser.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Cys_Common;

namespace MWebBrowser.View
{
    /// <summary>
    /// Interaction logic for DownloadToolUc.xaml
    /// </summary>
    public partial class DownloadToolUc : UserControl
    {
        public Action ShowDownloadTabEvent;
        private readonly Dictionary<int,DownloadToolItemViewModel> _downloadDict;

        private Storyboard _displayToolStoryboard;
        private Storyboard _hideToolStoryboard;
        public DownloadToolUc()
        {
            _downloadDict = new Dictionary<int, DownloadToolItemViewModel>();
            InitializeComponent();
            InitStoryboard();
        }

        private void CloseDownloadTool_OnClick(object sender, RoutedEventArgs e)
        {
            ItemsParent.Children.Clear();
            HideTool();
        }

        private void ShowAll_OnClick(object sender, RoutedEventArgs e)
        {
            ItemsParent.Children.Clear();
            HideTool();
            ShowDownloadTabEvent?.Invoke();
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
                    //this.Visibility = Visibility.Visible;
                    DisplayTool();
                    var item = new DownloadToolItemUc { DataContext = viewModel };
                    ItemsParent.Children.Insert(0, item);
                }));

                if (!GlobalInfo.DownloadSetting.DownloadItemInfos.Exists(x =>
                    x.FileName == downloadItem.SuggestedFileName))
                {
                    GlobalInfo.DownloadSetting.DownloadItemInfos.Add(new DownloadItemInfo
                    {
                        FileName = downloadItem.SuggestedFileName,
                        FilePath = downloadItem.FullPath,
                        Url = downloadItem.Url,
                    });
                }
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

        #region Animation
        private void InitStoryboard()
        {
            _displayToolStoryboard = (Storyboard)ParentGrid.FindResource("DisplayTool");
            _hideToolStoryboard = (Storyboard)ParentGrid.FindResource("HideTool");
        }

        private void DisplayTool()
        {
            _displayToolStoryboard.Begin();
        }

        private void HideTool()
        {
            _hideToolStoryboard.Begin();
        }

        #endregion
    }
}
