using Cys_Model.DataBase;
using Cys_Model.Model;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cys_DataRepository
{
    public static class BaseRepository<T> where T : class, new()
    {
        private static readonly ISqlSugarClient _db; 
        static BaseRepository()
        {
            _db = new DbContext().Db;
        }


        #region Add

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static async Task<int> Add(T entity)
        {
            var insert = _db.Insertable(entity);
            return await insert.ExecuteReturnIdentityAsync();
        }

        #endregion

        #region Delete

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static async Task<bool> Delete(T entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定Id的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteById(object id)
        {
            return await _db.Deleteable<T>(id).ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region Query

        /// <summary>
        /// 根据实体查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<List<T>> Query(QueryParam<T> param)
        {
            var sqb = _db.Queryable<T>().Select(param.SelectExp)
                .OrderByIF(param.IsOrderBy, param.OrderExp, param.OrderByType);
            if (param.Top > 0)
            {
                sqb = sqb.Take(param.Top);
            }
            return await sqb.WhereIF(param.WhereExp != null, param.WhereExp).ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns></returns>
        public static async Task<PageModel<T>> QueryPage(QueryPageParam<T> param)
        {
            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<T>().Select(param.SelectExp)
                .OrderByIF(param.IsOrderBy, param.OrderExp, param.OrderByType)
                .WhereIF(param.WhereExp != null, param.WhereExp)
                .ToPageListAsync(param.PageNum, param.PageSize, totalCount);
            int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / param.PageSize.ObjToDecimal())).ObjToInt();
            return new PageModel<T>() { dataCount = totalCount, pageCount = pageCount, pageNum = param.PageNum, PageSize = param.PageSize, data = list };
        }
        #endregion
    }
}
