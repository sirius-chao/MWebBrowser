using Cys_DataRepository;
using Cys_Model.Model;
using Cys_Model.Tables;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cys_Services
{
    public class HistoryServices
    {
        public List<HistoryModel> GetHistoryInfo()
        {
            return new List<HistoryModel>();
        }

        public async Task<int> AddHistory(HistoryModel model)
        {
            QueryParam<HistoryModel> qModel = new QueryParam<HistoryModel>
            {
                WhereExp = x => x.Url == model.Url,
                OrderByType = OrderByType.Desc,
                IsOrderBy = true,
                OrderExp = x => x.VisitTime,
                Top = 1,
            };

            var qResult = await BaseRepository<HistoryModel>.Query(qModel);
            if (qResult.Count > 0)
            {
                TimeSpan timeSpan = DateTime.Now - qResult[0].VisitTime;
                if (timeSpan.Minutes < 1)//简单去重
                    return 0;
            }
            return await BaseRepository<HistoryModel>.Add(model);
        }

        public async Task<bool> DeleteHistory(int id)
        {
            return await BaseRepository<HistoryModel>.DeleteById(id);
        }

        public async Task<PageModel<HistoryModel>> GetHistoryList(int pageNum = 1, int pageSize = 20)
        {
            QueryPageParam<HistoryModel> param = new QueryPageParam<HistoryModel>
            {
                PageSize = pageSize,
                PageNum = pageNum,
                IsOrderBy = true,
                OrderExp = it => new { it.VisitTime },
                OrderByType = OrderByType.Desc
            };
            return await BaseRepository<HistoryModel>.QueryPage(param);
        }
    }
}
