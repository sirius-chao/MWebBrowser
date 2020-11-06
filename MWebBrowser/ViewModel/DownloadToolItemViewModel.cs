namespace MWebBrowser.ViewModel
{
    public class DownloadToolItemViewModel : BaseViewModel
    {

        private double _currentSize;
        public double CurrentSize { get => _currentSize; set { _currentSize = value; OnPropertyChanged("CurrentSize"); } }

        private double _totalSize;
        public double TotalSize { get => _totalSize; set { _totalSize = value; OnPropertyChanged("TotalSize"); } }

        private string _currentSizeStr;
        public string CurrentSizeStr { get => _currentSizeStr; set { _currentSizeStr = value; OnPropertyChanged("CurrentSizeStr"); } }

        private string _totalSizeStr;
        public string TotalSizeStr { get => _totalSizeStr; set { _totalSizeStr = value; OnPropertyChanged("TotalSizeStr"); } }

        private string _fileName;
        public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged("FileName"); } }


        public string ConvertFileSize(long size)
        {
            if (size > 1024 * 1024 * 1024)
            {
                return $"{size / (1024 * 1024 * 1024)}G";
            }
            if (size > 1024 * 1024)
            {
                return $"{size / (1024 * 1024)}M";
            }
            return size > 1024 ? $"{size / 1024}K" : $"{size}B";
        }
    }
}
