using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace JC.DataAccess
{
    public class SQLiteDAO : IDAO
    {
        public SQLiteDAO(string strConn)
        {
            if (string.IsNullOrEmpty(strConn))
                throw new ArgumentNullException("connection");

            this.conn = this.CreateConnection(strConn);
        }
        /// <summary>
        /// 数据操作中使用的数据库连接
        /// </summary>
        private readonly SQLiteConnection conn;

        /// <summary>
        /// 当开始一个事务时,该对象将被初始化
        /// </summary>
        private SQLiteTransaction trans;

        /// <summary>
        /// 是否已经释放非托管资源
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// 使用指定的字符串获得数据库连接
        /// </summary>
        /// <param name="sqlConstr"></param>
        /// <returns></returns>
        private SQLiteConnection CreateConnection(String sqlConstr)
        {
            SQLiteConnection conn = new SQLiteConnection(sqlConstr);
            conn.Open();
            return conn;
        }

        public DbDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return this.ExecuteReader(commandType, commandText, null);
        }

        public DbDataReader ExecuteReader(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            DbDataReader dataReader = null;
            try
            {
                this.PrepareCommand(cmd, commandType, commandText, commandParameters);
                // Create a reader
                dataReader = cmd.ExecuteReader();
                bool canClear = true;
                foreach (SQLiteParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear) cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataReader;
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return this.ExecuteDataset(commandType, commandText, null);
        }
        
        public DataSet ExecuteDataset(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();

            this.PrepareCommand(cmd, commandType, commandText, commandParameters);

            SQLiteDataAdapter sda = new SQLiteDataAdapter();
            // Create the DataAdapter & DataSet
            sda.SelectCommand = cmd;

            DataSet ds = new DataSet();

            // Fill the DataSet using default values for DataTable names, etc
            sda.Fill(ds);

            cmd.Parameters.Clear();
            // Return the dataset
            return ds;
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            return this.ExecuteScalar(commandType, commandText);
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            this.PrepareCommand(cmd, commandType, commandText, commandParameters);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            return retval;
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(commandType, commandText);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            // Create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            this.PrepareCommand(cmd, commandType, commandText, commandParameters);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            return retval;
        }

        private void PrepareCommand(SQLiteCommand command, CommandType commandType, string commandText, SQLiteParameter[] commandParameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (this.conn.State != ConnectionState.Open)
                this.conn.Open();

            // Associate the connection with the command
            command.Connection = this.conn;
            int defaultTimeOut = 30;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["CommandTimeout"],
                out defaultTimeOut);
            command.CommandTimeout = defaultTimeOut;
            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (this.trans != null)
            {
                if (this.trans.Connection == null)
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = this.trans;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
                this.AttachParameters(command, commandParameters);
            return;
        }

        private void AttachParameters(SQLiteCommand command, SQLiteParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SQLiteParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null)) p.Value = DBNull.Value;
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (this.isDisposed) return;
            if (this.trans != null && this.trans.Connection != null)
            {
                this.trans.Commit();
                this.trans.Dispose();
            }
            if (this.conn != null && this.conn.State != ConnectionState.Closed) this.conn.Close();

            GC.SuppressFinalize(this);
            this.isDisposed = true;
        }
    }
}
