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
            setting.SearchEngineItemInfos ??= new List<SearchEngineItemInfo>();
            return setting;
        }
    }
}
