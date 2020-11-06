namespace MWebBrowser.ViewModel
{
    public class DownloadToolItemViewModel: BaseViewModel
    {
        private string _currentSizeStr;
        public string CurrentSizeStr { get => _currentSizeStr; set { _currentSizeStr = value; OnPropertyChanged("CurrentSizeStr"); } }

        private string _totalSizeStr;
        public string TotalSizeStr { get => _totalSizeStr; set { _totalSizeStr = value; OnPropertyChanged("TotalSizeStr"); } }

        private string _fileName;
        public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged("FileName"); } }
    }
}
