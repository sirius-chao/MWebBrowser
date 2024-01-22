using Cys_Common.Common;
using Cys_Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MWebBrowser.ViewModel.Setting.SearchEngine
{
    public class SearchEngineViewModel : BaseViewModel
    {
        public SearchEngineViewModel()
        {
            SearchEngineItemInfos = GlobalInfo.SearchEngineSetting.SearchEngineItemInfos;
            if (searchEngineItemInfos.Count <= 0)
            {
                SearchEngineItemInfos = new List<SearchEngineItemInfo>()
                {
                    new SearchEngineItemInfo() {Name="baidu",ChineseName = "百度",Url="https://www.baidu.com/",Default=false,Describe="百度搜索"},
                    new SearchEngineItemInfo() {Name="bing",ChineseName = "必应",Url="https://www.bing.com/",Default=true,Describe="必应搜索"},
                    new SearchEngineItemInfo() {Name="google",ChineseName = "谷歌",Url="https://www.google.com.hk/",Default=false,Describe="谷歌香港搜索"}
                };
            }
            SelectedItem = SearchEngineItemInfos.FirstOrDefault(x => x.Default);
            saveCommand = new RelayCommand(SaveDefault);
        }
        private List<SearchEngineItemInfo> searchEngineItemInfos;
        public List<SearchEngineItemInfo> SearchEngineItemInfos
        {
            get { return searchEngineItemInfos; }
            set
            {
                searchEngineItemInfos = value;
                OnPropertyChanged(nameof(SearchEngineItemInfos));
            }
        }

        private SearchEngineItemInfo selectedItem;
        public SearchEngineItemInfo SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                return saveCommand;
            }
        }
        private void SaveDefault(object obj)
        {
            SearchEngineItemInfos.ForEach(x => x.Default = false);
            SearchEngineItemInfos.FirstOrDefault(x=>x.Name == SelectedItem.Name).Default = true;
            GlobalInfo.SearchEngineSetting.SearchEngineItemInfos = SearchEngineItemInfos;
        }
    }
}
