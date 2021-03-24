using System.Windows.Input;
using Cys_Controls.Code;

namespace MWebBrowser.ViewModel
{
    public class DownloadShowAllItemViewModel : BaseViewModel
    {
        private string _fileName;
        public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged("FileName"); } }

        private string _filePath;
        public string FilePath { get => _filePath; set { _filePath = value; OnPropertyChanged("FilePath"); } }

        private string _url;
        public string Url { get => _url; set { _url = value; OnPropertyChanged("Url"); } }
    }
}
