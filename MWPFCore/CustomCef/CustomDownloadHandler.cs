﻿using CefSharp;
using Cys_Common.Code.Configure;
using System;

namespace MWPFCore.Code.CustomCef
{
    public class CustomDownloadHandler : IDownloadHandler
    {
        private readonly Action<bool, DownloadItem> _downloadCallBackEvent;//第一个参数为true为update

        public CustomDownloadHandler(Action<bool, DownloadItem> downloadCallBackEvent)
        {
            _downloadCallBackEvent = downloadCallBackEvent;
        }

        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return true;
        }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IBeforeDownloadCallback callback)
        {
            if (callback.IsDisposed) return;
            _downloadCallBackEvent?.Invoke(false, downloadItem);
            downloadItem.IsInProgress = true;
            var path = GetDownloadFullPath(downloadItem.SuggestedFileName);
            callback.Continue(path, false);
        }


        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IDownloadItemCallback callback)
        {
            _downloadCallBackEvent?.Invoke(true, downloadItem);
        }


        private string GetDownloadFullPath(string suggestedFileName)
        {
            var configPath = ConfigHelper.Config.DownLoadPath.TrimEnd('\\') + "\\";
            return configPath + suggestedFileName;
        }
    }
}
