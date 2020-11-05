using System.Windows.Media;
using MWebBrowser.Code.Helpers;

namespace MWebBrowser.ViewModel
{
    public class WebTabItemViewModel: BaseViewModel
    {
        private string _title = "新标签页";
        public string Title { get => _title; set { _title = value; OnPropertyChanged("Title"); } }

        private ImageSource _favicon = ImageHelper.DefaultFavicon;
        public ImageSource Favicon { get => _favicon; set { _favicon = value; OnPropertyChanged("Favicon"); } }

        private string _currentUrl;
        public string CurrentUrl { get => _currentUrl; set { _currentUrl = value; OnPropertyChanged("CurrentUrl"); } }
    }
}
