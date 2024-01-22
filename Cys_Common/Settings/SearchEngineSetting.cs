using System.Collections.Generic;
using System.Linq;

namespace Cys_Common.Settings
{

    public class SearchEngineSetting
    {
        public List<SearchEngineItemInfo> SearchEngineItemInfos { get; set; }

        public SearchEngineItemInfo GetDefaultSearchEngineItemInfo()
        {
            return SearchEngineItemInfos.FirstOrDefault(x => x.Default);
        }
    }

    public class SearchEngineItemInfo
    {
        public string Name { get; set; }
        public string ChineseName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool Default { get; set; }
        public string Describe { get; set; }
    }
}
