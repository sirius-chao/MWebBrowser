using SqlSugar;
using System;
using System.Linq.Expressions;

namespace Cys_Model.Model
{
    public class QueryPageParam<T>
    {
        /// <summary>
        /// 查询结果
        /// </summary>
        public Expression<Func<T, T>> SelectExp { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public Expression<Func<T, bool>> WhereExp { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNum { get; set; } = 1;
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 是否需要排序
        /// </summary>
        public bool IsOrderBy { get; set; } = false;
        /// <summary>
        /// 排序字段
        /// </summary>
        public Expression<Func<T, object>> OrderExp { get; set; }
        /// <summary>
        /// 升序降序
        /// </summary>
        public OrderByType OrderByType { get; set; } = OrderByType.Asc;
    }
}
