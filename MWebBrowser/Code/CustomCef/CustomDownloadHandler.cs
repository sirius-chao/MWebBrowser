using CefSharp;
using MWebBrowser.Code.Configure;

namespace MWebBrowser.Code.CustomCef
{
    public class CustomDownloadHandler : IDownloadHandler
    {
        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IBeforeDownloadCallback callback)
        {
            if (callback.IsDisposed) return;
            downloadItem.IsInProgress = true;
            var path = GetDownloadFullPath(downloadItem.SuggestedFileName);
            callback.Continue(path, false);
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IDownloadItemCallback callback)
        {

        }


        private string GetDownloadFullPath(string suggestedFileName)
        {
            var configPath = ConfigHelper.Config.DownLoadPath.TrimEnd('\\') + "\\";
            return configPath + suggestedFileName;
        }
    }
}
