using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace JC.DataAccess
{
    public interface IDAO : IDisposable
    {
        /// <summary>
        /// 返回DbDataReader对象
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        DbDataReader ExecuteReader(CommandType commandType, string commandText);

        /// <summary>
        /// 返回DbDataReader对象
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DbDataReader ExecuteReader(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters);

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        DataSet ExecuteDataset(CommandType commandType, string commandText);

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DataSet ExecuteDataset(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters);

        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        object ExecuteScalar(CommandType commandType, string commandText);

        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        object ExecuteScalar(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters);

        int ExecuteNonQuery(CommandType commandType, string commandText);

        int ExecuteNonQuery(CommandType commandType, string commandText, params SQLiteParameter[] commandParameters);
    }
}
