using MWebBrowser.Code.Helpers;
using System;
using System.Windows;
using System.Windows.Media;

namespace MWebBrowser.ViewModel
{
    public class HistoryItemViewModel : BaseViewModel
    {
        private Visibility _groupVisible;
        /// <summary>
        /// 是否显示组
        /// </summary>
        public Visibility GroupVisible
        {
            get => _groupVisible;
            set { _groupVisible = value; OnPropertyChanged("GroupVisible"); }
        }
        /// <summary>
        /// 是否显示条数
        /// </summary>
        public Visibility ItemVisible => GroupVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;


        private int _groupSort;
        /// <summary>
        /// 判断是否排在上面 1排在最上方用于分组显示,
        /// </summary>
        public int GroupSort
        {
            get => _groupSort;

            set { _groupSort = value; OnPropertyChanged("GroupSort"); }
        }

        private int _isGroup;

        public int IsGroup
        {
            get => _isGroup;
            set { _isGroup = value; OnPropertyChanged("IsGroup"); }
        }
        

        private string _title;
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged("Title"); }
        }

        private string _url;
        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get => _url;
            set { _url = value; OnPropertyChanged("Url"); }
        }

        private int _id;
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged("Id"); }
        }

        private string _pageId;
        /// <summary>
        /// 分页ID,用于分页时传此ID做分页
        /// </summary>
        public string PageId
        {
            get => _pageId;
            set { _pageId = value; OnPropertyChanged("PageId"); }
        }

        private DateTime _visitTime;
        public DateTime VisitTime
        {
            get => _visitTime;
            set { _visitTime = value; OnPropertyChanged("VisitTime"); }
        }

        public string VisitTimeStr
        {
            get
            {
                string ss = "上午";
                if (VisitTime.Hour > 12)
                {
                    ss = "下午";
                }

                return $"{ss}:{VisitTime:HH:mm}";
            }
        }

        private SolidColorBrush _backColorBrush;
        /// <summary>
        /// 是否显示条数
        /// </summary>
        public SolidColorBrush BackColorBrush
        {
            get => _backColorBrush;
            set { _backColorBrush = value; OnPropertyChanged("BackColorBrush"); }
        }

        private Visibility _dateVisible = Visibility.Visible;
        /// <summary>
        /// 是否日期
        /// </summary>
        public Visibility DateVisible
        {
            get => _dateVisible;
            set { _dateVisible = value; OnPropertyChanged("DateVisible"); }
        }

        private Visibility _closeVisible = Visibility.Collapsed;
        /// <summary>
        /// 是否显示关闭
        /// </summary>
        public Visibility CloseVisible
        {
            get => _closeVisible;
            set { _closeVisible = value; OnPropertyChanged("CloseVisible"); }
        }

        private ImageSource _favicon = ImageHelper.DefaultFavicon;
        public ImageSource Favicon { get => _favicon; set { _favicon = value; OnPropertyChanged("Favicon"); } }
    }
}
