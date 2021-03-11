using System;
using SqlSugar;

namespace Cys_Model.Tables
{
    [SugarTable("history", "f10")]
    public class HistoryModel
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime VisitTime { get; set; }
        public int FormVisit { get; set; }
    }
}
