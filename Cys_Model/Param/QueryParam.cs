using SqlSugar;
using System;
using System.Linq.Expressions;

namespace Cys_Model.Model
{
    public class QueryParam<T>
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

        /// <summary>
        /// 前几条0为无限制
        /// </summary>
        public int Top { get; set; }
    }
}
