using System.Collections.Generic;

namespace Cys_Common
{
    public class DownloadSetting
    {
        public List<DownloadItemInfo> DownloadItemInfos { get; set; }
    }

    public class DownloadItemInfo
    {
        public string Time { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Describe { get; set; }
    }
}
