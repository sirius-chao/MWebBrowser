using Cys_Model.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cys_DataRepository;
using Cys_Model.Model;
using SqlSugar;

namespace Cys_Services
{
    public class HistoryServices
    {
        private readonly BaseRepository<HistoryModel> _br;
        public HistoryServices()
        {
            _br = new BaseRepository<HistoryModel>();
        }
        public List<HistoryModel> GetHistoryInfo()
        {
            return new List<HistoryModel>();
        }

        public async Task<int> AddHistory(HistoryModel model)
        {
            return await _br.Add(model);
        }

        public async Task<bool> DeleteHistory(int id)
        {
            return await _br.DeleteById(id);
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
            return await _br.QueryPage(param);
        }
    }
}
