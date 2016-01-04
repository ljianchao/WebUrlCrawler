using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JC.DataAccess
{
    /// <summary>
    /// 默认数据库连接类
    /// </summary>
    public static class DefaultDBConn
    {
        public static string DefaultDbConnectionString;

        static DefaultDBConn()
        {
            DefaultDbConnectionString = System.Configuration.ConfigurationManager.AppSettings["SQLiteDBConnection"] ??
                                        string.Format(@"Data Source={0}\Data\{1};Version=3;Pooling=False;Max Pool Size=100;",
                                                        Assembly.GetExecutingAssembly().Location, "articles.db");
        }
    }
}
