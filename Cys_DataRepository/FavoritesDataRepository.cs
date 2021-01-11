using Cys_Common;
using Cys_Common.Enum;
using Cys_Model;
using System;
using System.Collections.Generic;

namespace Cys_DataRepository
{
    public class FavoritesDataRepository
    {
        public void SaveFavoritesSetting()
        {
            try
            {
                var setting = GlobalInfo.FavoritesSetting;
                if (setting == null) return;
                var fileName = FileDataPath.GetFilePath(DataFileType.Favorites);
                CommonOperator.SaveDataJson(setting, fileName);
            }
            catch (Exception ex)
            {

            }
        }

        public FavoritesSetting GetFavoritesSetting()
        {
            var fileName = FileDataPath.GetFilePath(DataFileType.Favorites);
            var setting = CommonOperator.GetDataJson<FavoritesSetting>(fileName);
            setting ??= new FavoritesSetting();
            setting.FavoritesInfos ??= new List<TreeNode>();
            if (setting.FavoritesInfos.Count <= 0)
            {
                setting.FavoritesInfos.Add(new TreeNode()
                {
                    ParentId = -1,
                    NodeId = 0,
                    NodeName = "收藏夹",
                    Type = 1,
                    Level = 0,
                });
            }
            return setting;
        }
    }
}