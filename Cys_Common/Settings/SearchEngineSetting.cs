using System.Collections.Generic;

namespace Cys_Common.Settings
{

    public class SearchEngineSetting
    {
        public List<SearchEngineItemInfo> SearchEngineItemInfos { get; set; }
    }

    public class SearchEngineItemInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Describe { get; set; }
    }
}
