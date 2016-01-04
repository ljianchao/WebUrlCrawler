using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace JC.DataAccess
{
    public abstract class Database
    {
        readonly String connectionString;
        readonly DbProviderFactory dbProviderFactory;

        /// <summary>
        /// 初始化Database对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbProviderFactory"></param>
        public Database(string connectionString, DbProviderFactory dbProviderFactory)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //TODO:需要封装异常类型
                throw new ArgumentNullException("ExceptionNullOrEmptyString", "connectionString");
            }
            if (dbProviderFactory == null)
            {
                throw new ArgumentNullException("dbProviderFactory");
            }

            this.connectionString = connectionString;
            this.dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// 获取连接字符串信息
        /// </summary>
        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        /// <summary>
        /// 获取DbProviderFactory
        /// </summary>
        public DbProviderFactory DbProviderFactory
        {
            get { return this.dbProviderFactory; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, string.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType,
                                   object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, string.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType,
                                   string sourceColumn,
                                   DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// command命令新增一个Out方向的DbParameter参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public void AddOutParameter(DbCommand command,
                                    string name,
                                    DbType dbType,
                                    int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        public void AddParameter(DbCommand command,
                                 string name,
                                 DbType dbType,
                                 ParameterDirection direction,
                                 string sourceColumn,
                                 DataRowVersion sourceVersion,
                                 object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// command命令新增一个DbParameter对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name">param的名称</param>
        /// <param name="dbType">param的类型</param>
        /// <param name="size">列中数据的最大大小（以字节为单位）</param>
        /// <param name="direction">获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数</param>
        /// <param name="nullable">是否接受 null 值</param>
        /// <param name="precision">Value 属性的最大位数。默认值为 0，它表示数据提供程序设置Value 的精度</param>
        /// <param name="scale">Value 解析为的小数位数</param>
        /// <param name="sourceColumn">该源列映射到 System.Data.DataSet 并用于加载或返回 System.Data.Common.DbParameter.Value</param>
        /// <param name="sourceVersion">获取或设置在加载 System.Data.Common.DbParameter.Value 时使用的 System.Data.DataRowVersion</param>
        /// <param name="value"></param>
        public virtual void AddParameter(DbCommand command,
                                         string name,
                                         DbType dbType,
                                         int size,
                                         ParameterDirection direction,
                                         bool nullable,
                                         byte precision,
                                         byte scale,
                                         string sourceColumn,
                                         DataRowVersion sourceVersion,
                                         object value)
        {
            if (command == null) throw new ArgumentNullException("command");

            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// 创建并获取事务对象（静态方法）
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        static DbTransaction BeginTransaction(DbConnection connection)
        {
            DbTransaction tran = connection.BeginTransaction();
            return tran;
        }

        /// <summary>
        /// 提交事务（静态方法）
        /// </summary>
        /// <param name="tran"></param>
        static void CommitTransaction(IDbTransaction tran)
        {
            tran.Commit();
        }

        /// <summary>
        /// 回滚事务（静态方法）
        /// </summary>
        /// <param name="tran"></param>
        static void RollbackTransaction(IDbTransaction tran)
        {
            tran.Rollback();
        }

        /// <summary>
        /// 创建并获取connection对象
        /// 定义virtual，可以被派生类重写
        /// 被调用时，需要手动控制关闭connection
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection CreateConnection()
        {
            DbConnection newConnection = dbProviderFactory.CreateConnection();
            newConnection.ConnectionString = ConnectionString;

            return newConnection;
        }

        /// <summary>
        /// 创建command对象
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        DbCommand CreateCommandByCommandType(CommandType commandType, string commandText)
        {
            DbCommand command = dbProviderFactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return command;
        }        

        /// <summary>
        /// 构建并返回参数的名称
        /// 定义virtual，可以被派生类重写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string BuildParameterName(string name)
        {
            return name;
        }

        /// <summary>
        /// 构建并返回DbParameter对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected DbParameter CreateParameter(string name)
        {
            DbParameter param = dbProviderFactory.CreateParameter();
            param.ParameterName = BuildParameterName(name);

            return param;
        }

        /// <summary>
        /// 创建一个DbParameter的新实例
        /// </summary>
        /// <param name="name">param的名称</param>
        /// <param name="dbType">param的类型</param>
        /// <param name="size">列中数据的最大大小（以字节为单位）</param>
        /// <param name="direction">获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数</param>
        /// <param name="nullable">是否接受 null 值</param>
        /// <param name="precision">Value 属性的最大位数。默认值为 0，它表示数据提供程序设置Value 的精度</param>
        /// <param name="scale">Value 解析为的小数位数</param>
        /// <param name="sourceColumn">该源列映射到 System.Data.DataSet 并用于加载或返回 System.Data.Common.DbParameter.Value</param>
        /// <param name="sourceVersion">获取或设置在加载 System.Data.Common.DbParameter.Value 时使用的 System.Data.DataRowVersion</param>
        /// <returns></returns>
        protected DbParameter CreateParameter(string name,
                                              DbType dbType,
                                              int size,
                                              ParameterDirection direction,
                                              bool nullable,
                                              byte precision,
                                              byte scale,
                                              string sourceColumn,
                                              DataRowVersion sourceVersion,
                                              object value)
        {
            DbParameter param = CreateParameter(name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// 配置一个给定的DbParameter对象
        /// </summary>
        /// <param name="param">需要配置DbParameter对象</param>
        /// <param name="name">param的名称</param>
        /// <param name="dbType">param的类型</param>
        /// <param name="size">列中数据的最大大小（以字节为单位）</param>
        /// <param name="direction">获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数</param>
        /// <param name="nullable">是否接受 null 值</param>
        /// <param name="precision">Value 属性的最大位数。默认值为 0，它表示数据提供程序设置Value 的精度</param>
        /// <param name="scale">Value 解析为的小数位数</param>
        /// <param name="sourceColumn">该源列映射到 System.Data.DataSet 并用于加载或返回 System.Data.Common.DbParameter.Value</param>
        /// <param name="sourceVersion">获取或设置在加载 System.Data.Common.DbParameter.Value 时使用的 System.Data.DataRowVersion</param>
        /// <param name="value"></param>
        protected virtual void ConfigureParameter(DbParameter param,
                                                  string name,
                                                  DbType dbType,
                                                  int size,
                                                  ParameterDirection direction,
                                                  bool nullable,
                                                  byte precision,
                                                  byte scale,
                                                  string sourceColumn,
                                                  DataRowVersion sourceVersion,
                                                  object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        /// <summary>
        /// 分配一个connection对象给command      
        /// </summary>
        /// <param name="command">command预先包含查询语句</param>
        /// <param name="connection"></param>
        protected static void PrepareCommand(DbCommand command,
                                             DbConnection connection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (connection == null) throw new ArgumentNullException("connection");

            command.Connection = connection;
        }

        /// <summary>
        /// 分配一个DbTransaction对象的connection对象给command
        /// </summary>
        /// <param name="command">command预先包含查询语句</param>
        /// <param name="transaction"></param>
        protected static void PrepareCommand(DbCommand command,
                                             DbTransaction transaction)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (transaction == null) throw new ArgumentNullException("transaction");

            PrepareCommand(command, transaction.Connection);
            command.Transaction = transaction;
        }


        /// <summary>
        /// 构建并返回DbDataAdapter对象
        /// </summary>
        /// <returns></returns>
        protected DbDataAdapter CreateDataAdapter()
        {
            DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();

            return adapter;
        }

        /// <summary>
        /// 获取新的已打开的connection
        /// </summary>
        /// <returns></returns>
        internal DbConnection GetNewOpenConnection()
        {
            DbConnection connection = null;
            try
            {
                connection = CreateConnection();
                connection.Open();
            }
            catch(Exception e)
            {                
                throw e;
            }

            return connection;
        }

        /// <summary>
        /// 获取一个被包装的connection
        /// 如果一个事务正在活动，这个connection对象将不会被disposed
        /// </summary>
        /// <returns></returns>
        protected DatabaseConnectionWrapper GetOpenConnection()
        {
            DatabaseConnectionWrapper connection = TransactionScopeConnections.GetConnection(this);
            return connection ?? GetWrappedConnection();
        }

        /// <summary>
        /// 获取一个被包装的connection，该connection不参与事务操作
        /// </summary>
        /// <returns></returns>
        protected virtual DatabaseConnectionWrapper GetWrappedConnection()
        {
            return new DatabaseConnectionWrapper(GetNewOpenConnection());
        }


        /// <summary>
        /// 执行非查询操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected int DoExecuteNonQuery(DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            try
            {
                int rowAffected = command.ExecuteNonQuery();
                return rowAffected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取IDataReader对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cmdBehavior"></param>
        /// <returns></returns>
        IDataReader DoExecuteReader(DbCommand command,
                                    CommandBehavior cmdBehavior)
        {
            try
            {
                IDataReader reader = command.ExecuteReader(cmdBehavior);
                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="command">为什么采用IDbCommand对象，而不是DbCommand对象？</param>
        /// <returns></returns>
        object DoExecuteScalar(IDbCommand command)
        {
            try
            {
                object returnValue = command.ExecuteScalar();
                return returnValue;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// DbAdapter对象填充DataSet对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        void DoLoadDataSet(IDbCommand command,
                           DataSet dataSet,
                           string[] tableNames)
        {
            if (tableNames == null) throw new ArgumentNullException("tableNames");
            if (tableNames.Length == 0)
            {
                throw new ArgumentException("ExceptionTableNameArrayEmpty", "tableNames");
            }
            for (int i = 0; i < tableNames.Length; i++)
            {
                if (string.IsNullOrEmpty(tableNames[i]))
                {
                    throw new ArgumentException("ExceptionNullOrEmptyString", string.Concat("tableNames[", i, "]"));
                }
            }

            using (DbDataAdapter adapter = CreateDataAdapter())
            {
                ((IDbDataAdapter)adapter).SelectCommand = command;

                try
                {
                    string systemCreatedTableNameRoot = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        string systemCreatedTableName = (i == 0)
                                                                ? systemCreatedTableNameRoot
                                                                : systemCreatedTableNameRoot + i;

                        adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                    }

                    adapter.Fill(dataSet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行command命令，并获取DataSet结果
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table");
            return dataSet;
        }

        /// <summary>
        /// 执行在DbTransaction对象内command命令，并获取DataSet结果
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(DbCommand command,
                                              DbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table", transaction);
            return dataSet;
        }

        /// <summary>
        /// 执行指定CommandType类型的commandText，并返回DataSet对象
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        public virtual DataSet ExecuteDataSet(CommandType commandType,
                               string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command);
            }
        }

        /// <summary>
        /// 执行在DbTransaction对象内command命令，并获取DataSet结果
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(DbTransaction transaction,
                                              CommandType commandType,
                                              string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command, transaction);
            }
        }

        /// <summary>
        /// 执行command命令，并返回受影响行数
        /// </summary>
        /// <param name="command">command包含执行的命令</param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(DbCommand command)
        {
            using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return DoExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 执行在DbTransaction对象内command命令，并返回受影响行数
        /// </summary>
        /// <param name="command">command包含执行的命令</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(DbCommand command,
                                           DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行指定CommandType类型的commandText，并返回受影响行数
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(CommandType commandType,
                                           string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 执行指定CommandType类型的commandText，并返回受影响行数
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(DbTransaction transaction,
                                           CommandType commandType,
                                           string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteNonQuery(command, transaction);
            }
        }

        //public virtual IDataReader ExecuteReader(DbCommand command)
        //{
        //    using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
        //    {
        //        PrepareCommand(command, wrapper.Connection);
        //        IDataReader realReader = DoExecuteReader(command, CommandBehavior.Default);
                
        //    }
        //}

        /// <summary>
        /// 执行command命令，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return DoExecuteScalar(command);
            }
        }

        /// <summary>
        /// 执行DbTransaction中的command命令，
        /// 并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(DbCommand command,
                                            DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteScalar(command);
        }

        /// <summary>
        /// 执行指定CommandType类型的commandText，
        /// 并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(CommandType commandType,
                                            string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteScalar(command);
            }
        }

        /// <summary>
        /// 执行指定CommandType类型的commandText，
        /// 并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(DbTransaction transaction,
                                            CommandType commandType,
                                            string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteScalar(command, transaction);
            }
        }

        /// <summary>
        /// 从DbCommand中加载一个DataSet对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string tableName)
        {
            LoadDataSet(command, dataSet, new[] { tableName });
        }

        /// <summary>
        /// 从DbCommand中加载一个DataSet对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string[] tableNames)
        {
            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                DoLoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// 从事务中的command加载一个DataSet对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string tableName,
                                        DbTransaction transaction)
        {
            LoadDataSet(command, dataSet, new[] { tableName }, transaction);
        }

        /// <summary>
        /// 从事务中的command加载一个DataSet对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="transaction"></param>
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string[] tableNames,
                                        DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoLoadDataSet(command, dataSet, tableNames);
        }

        /// <summary>
        /// 获取parameter的值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual object GetParameterValue(DbCommand command,
                                                string name)
        {
            if (command == null) throw new ArgumentNullException("command");

            return command.Parameters[BuildParameterName(name)].Value;
        }

        /// <summary>
        /// 创建一个SQL查询的command
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommand(string query)
        {
            if (string.IsNullOrEmpty(query)) throw new ArgumentException("ExceptionNullOrEmptyString", "query");

            return CreateCommandByCommandType(CommandType.Text, query);
        }

        /// <summary>
        /// 执行command命令并返回一个IDataReader，通过它可以读取结果
        /// 调用者在调用完毕后，负责关闭IDataReader
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(DbCommand command)
        {
            using (DatabaseConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                IDataReader realReader = DoExecuteReader(command, CommandBehavior.Default);
                return CreateWrappedReader(wrapper, realReader);
            }
        }

        /// <summary>
        /// 创建一个被包装的DataReader对象，用来控制连接对象connection
        /// </summary>
        /// <param name="connection">Connectinon + refCount</param>
        /// <param name="innerReader">被包装的reader</param>
        /// <returns></returns>
        protected virtual IDataReader CreateWrappedReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
        {
            return new RefCountingDataReader(connection, innerReader);
        }

        /// <summary>
        /// 调用者在调用完毕后，负责关闭IDataReader
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(DbCommand command,
                                                 DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteReader(command, CommandBehavior.Default);
        }

        /// <summary>
        /// 调用者在调用完毕后，负责关闭IDataReader
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType commandType,
                                         string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command);
            }
        }

        /// <summary>
        /// 调用者在调用完毕后，负责关闭IDataReader
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbTransaction transaction,
                                         CommandType commandType,
                                         string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command, transaction);
            }
        }

        ///// <summary>
        ///// 返回在command中的parameters的起始索引
        ///// </summary>
        ///// <returns></returns>
        //protected virtual int UserParametersStartIndex()
        //{
        //    return 0;
        //}

        ///// <summary>
        ///// 设置parameter的值
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="parameterName"></param>
        ///// <param name="value"></param>
        //public virtual void SetParameterValue(DbCommand command,
        //                                      string parameterName,
        //                                      object value)
        //{
        //    if (command == null) throw new ArgumentNullException("command");

        //    command.Parameters[BuildParameterName(parameterName)].Value = value ?? DBNull.Value;
        //}

        //public virtual void AssignParameters(DbCommand command, object[] parameterValues)
        //{
        //    int parameterIndexShift = UserParametersStartIndex();   //这个数字依赖于数据库
        //    for (int i = 0; i < parameterValues.Length; i++)
        //    {
        //        IDataParameter parameter = command.Parameters[i + parameterIndexShift];

        //        // There used to be code here that checked to see if the parameter was input or input/output
        //        // before assigning the value to it. We took it out because of an operational bug with
        //        // deriving parameters for a stored procedure. It turns out that output parameters are set
        //        // to input/output after discovery, so any direction checking was unneeded. Should it ever
        //        // be needed, it should go here, and check that a parameter is input or input/output before
        //        // assigning a value to it.
        //        SetParameterValue(command, parameter.ParameterName, parameterValues[i]);
        //    }
        //}
    }
}
