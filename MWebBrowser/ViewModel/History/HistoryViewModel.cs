using Cys_Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MWebBrowser.ViewModel
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly HistoryServices _services;
        private int _pageNum = 1;
        private int _pageSize = 20;
        /// <summary>
        /// 判断是否有更多消息
        /// </summary>
        private bool _hasMore = true;
        /// <summary>
        /// 当前请求次数
        /// </summary>
        private int requestTime = 0;

        #region 歷史消息列表
        private ObservableCollection<HistoryItemViewModel> _historyList;
        /// <summary>
        /// 歷史消息列表
        /// </summary>
        public ObservableCollection<HistoryItemViewModel> HistoryList
        {
            get => _historyList;
            set { _historyList = value; OnPropertyChanged("HistoryList"); }
        }
        #endregion
        public HistoryViewModel()
        {
            _services = new HistoryServices();
            HistoryList = new ObservableCollection<HistoryItemViewModel>();
        }
        public void GetHistoryList()
        {
            if (!_hasMore) return;
            requestTime++;
            var tempRequestTime = requestTime;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (tempRequestTime != requestTime)
                    {
                        return;
                    }
                    ObservableCollection<HistoryItemViewModel> tempList = new ObservableCollection<HistoryItemViewModel>();
                    var result = _services.GetHistoryList(_pageNum, _pageSize);
                    foreach (var item in result.Result.data)
                    {
                        var viewModelItem = new HistoryItemViewModel
                        {
                            GroupVisible = Visibility.Collapsed,
                            Title = item.Title,
                            Url = item.Url,
                            VisitTime = item.VisitTime,
                        };
                        tempList.Add(viewModelItem);
                    }
                    _hasMore = result.Result.data.Count == _pageSize;
                    HistoryList = tempList;
                }
                catch (Exception ex)
                {
                }
            });
        }

        private HistoryItemViewModel NewDayItem(DateTime date)
        {
            DateTime dt = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            dt = dt.AddMilliseconds(1);
            var viewModelItem = new HistoryItemViewModel
            {
                GroupVisible = Visibility.Visible,
                GroupSort = 1,
            };
            return viewModelItem;
        }

        public void ReSet()
        {
            _pageNum = 1;
            _pageSize = 20;
            _hasMore = true;
            requestTime = 0;
        }
    }
}
