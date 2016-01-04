using System.Collections.Generic;
using System.Data.Common;
using System.Transactions;

namespace JC.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class TransactionScopeConnections
    {
        static readonly Dictionary<Transaction, Dictionary<string, DatabaseConnectionWrapper>> transactionConnections =
            new Dictionary<Transaction, Dictionary<string, DatabaseConnectionWrapper>>();

        public static DatabaseConnectionWrapper GetConnection(Database db)
        {
            Transaction currentTransaction = Transaction.Current;

            if (currentTransaction == null)
                return null;

            Dictionary<string, DatabaseConnectionWrapper> connectionList;
            DatabaseConnectionWrapper connection;

            lock (transactionConnections)
            {
                if (!transactionConnections.TryGetValue(currentTransaction, out connectionList))
                {
                    connectionList = new Dictionary<string, DatabaseConnectionWrapper>();
                    transactionConnections.Add(currentTransaction, connectionList);

                    currentTransaction.TransactionCompleted += OnTransactionCompleted;
                }
            }

            lock (connectionList)
            {
                if (!connectionList.TryGetValue(db.ConnectionString, out connection))
                {
                    DbConnection dbConnection = db.GetNewOpenConnection();
                    connection = new DatabaseConnectionWrapper(dbConnection);
                    connectionList.Add(db.ConnectionString, connection);
                }

                connection.AddRef();
            }

            return connection;
            
        }

        private static void OnTransactionCompleted(object sender, TransactionEventArgs e)
        {
            Dictionary<string, DatabaseConnectionWrapper> connectionList;

            lock (transactionConnections)
            {
                if (!transactionConnections.TryGetValue(e.Transaction, out connectionList))
                {
                    return;
                }

                transactionConnections.Remove(e.Transaction);
            }

            lock (connectionList)
            {
                foreach (DatabaseConnectionWrapper connectionWrapper in connectionList.Values)
                {
                    connectionWrapper.Dispose();
                }
            }
        }
    }
}
