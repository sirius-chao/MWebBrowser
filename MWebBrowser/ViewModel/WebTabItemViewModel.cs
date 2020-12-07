using System.Windows.Media;
using Cys_Controls.Code;
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

        private ZoomType _zoomLevelType;
        public ZoomType ZoomLevelType { get => _zoomLevelType; set { _zoomLevelType = value; OnPropertyChanged("ZoomLevelType"); } }

        private string _zoomRatio;
        public string ZoomRatio { get => _zoomRatio; set { _zoomRatio = value; OnPropertyChanged("ZoomRatio"); } }

        private bool _zoomIsChecked;
        public bool ZoomIsChecked { get => _zoomIsChecked; set { _zoomIsChecked = value; OnPropertyChanged("ZoomIsChecked"); } }

        private bool _zoomStaysOpen;
        public bool ZoomStaysOpen { get => _zoomStaysOpen; set { _zoomStaysOpen = value; OnPropertyChanged("ZoomStaysOpen"); } }
        
    }
}
