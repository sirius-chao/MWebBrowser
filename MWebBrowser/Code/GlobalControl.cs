using MWebBrowser.View.WebBrowser;

namespace MWebBrowser.Code
{
    public class GlobalControl
    {
        private static DownloadShowAllUc _downloadShowAll;

        public static DownloadShowAllUc DownloadShowAll => _downloadShowAll ?? new DownloadShowAllUc();
    }
}
