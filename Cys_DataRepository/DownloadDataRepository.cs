using System;
using Cys_Common;
using Cys_Common.Enum;

namespace Cys_DataRepository
{
    public class DownloadDataRepository
    {
        public void SaveSelectStockPlateSetting()
        {
            try
            {
                var setting = GlobalInfo.DownloadSetting;
                if (setting == null) return;
                var fileName = FileDataPath.GetFilePath(DataFileType.Download);
                CommonOperator.SaveDataJson(setting, fileName);
            }
            catch (Exception ex)
            {
               
            }
        }

        public DownloadSetting GetSelectStockPlateSetting()
        {
            var fileName = FileDataPath.GetFilePath(DataFileType.Download);
            var setting = CommonOperator.GetDataJson<DownloadSetting>(fileName);
            setting ??= new DownloadSetting();
            return setting;
        }
    }
}
