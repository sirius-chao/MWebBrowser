using Cys_Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cys_Common.Settings;

namespace Cys_DataRepository
{
    public class SearchEngineDataRepository
    {
        public void SaveSearchEngineSetting()
        {
            try
            {
                var setting = GlobalInfo.SearchEngineSetting;
                if (setting == null) return;
                var fileName = FileDataPath.GetFilePath(DataFileType.SearchEngine);
                CommonOperator.SaveDataJson(setting, fileName);
            }
            catch (Exception ex)
            {

            }
        }

        public SearchEngineSetting GetSearchEngineSetting()
        {
            var fileName = FileDataPath.GetFilePath(DataFileType.SearchEngine);
            var setting = CommonOperator.GetDataJson<SearchEngineSetting>(fileName);
            setting ??= new SearchEngineSetting();
            if (setting.SearchEngineItemInfos == null || setting.SearchEngineItemInfos.Count <= 0)
            {
                setting.SearchEngineItemInfos = new List<SearchEngineItemInfo>()
                {
                    new SearchEngineItemInfo() {Name="baidu",ChineseName = "百度",Url="https://www.baidu.com/",Default=false,Describe="百度搜索"},
                    new SearchEngineItemInfo() {Name="bing",ChineseName = "必应",Url="https://www.bing.com/",Default=true,Describe="必应搜索"},
                    new SearchEngineItemInfo() {Name="google",ChineseName = "谷歌",Url="https://www.google.com.hk/",Default=false,Describe="谷歌香港搜索"}
                };
            }
            return setting;
        }
    }
}
