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
