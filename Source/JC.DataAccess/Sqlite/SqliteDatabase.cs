using System;
using System.Data.Common;
using System.Data.SQLite;

namespace JC.DataAccess.Sqlite
{
    public class SqliteDatabase : Database
    {
        public SqliteDatabase(string connectionString)
            : base(connectionString, System.Data.SQLite.SQLiteFactory.Instance)
        {
            
        }

        private static SQLiteCommand CheckIfSqliteCommand(DbCommand command)
        {
            SQLiteCommand sqliteCommand = command as SQLiteCommand;
            if (sqliteCommand == null)
            {
                throw new ArgumentException("ExceptionCommandNotSqliteCommand", "command");
            }

            return sqliteCommand;
        }
    }
}
