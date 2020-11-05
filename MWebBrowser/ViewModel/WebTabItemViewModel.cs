namespace MWebBrowser.ViewModel
{
    public class WebTabItemViewModel: BaseViewModel
    {
        private string _header = "新标签页";
        public string Header { get => _header; set { _header = value; OnPropertyChanged("Header"); } }
    }
}
