using System.Linq;
using System.Reflection;

namespace Cys_Model.DataBase
{
    public static class DbSeed
    {
        private static readonly DbContext _context; 
        static DbSeed()
        {
            _context = new DbContext();
        }
        public static void InitData()
        {
            //创建数据库
            _context.Db.DbMaintenance.CreateDatabase();

            //创建表，反射获取指定数据表
            var modelTypes = from table in Assembly.GetExecutingAssembly().GetTypes()
                where table.IsClass && table.Namespace == "Cys_Model.Tables" select table;

            foreach (var t in modelTypes.ToList())
            {
                if (_context.Db.DbMaintenance.IsAnyTable(t.Name)) continue;
                _context.Db.CodeFirst.InitTables(t);
            }
        }
    }
}
