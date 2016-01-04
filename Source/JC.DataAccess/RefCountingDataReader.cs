using System;
using System.Data;

namespace JC.DataAccess
{
    /// <summary>
    /// IDataReader接口的一种实现
    /// 当reader被closed或disposed时，清理给定的内部DatabaseConnectionWrapper的引用计数
    /// </summary>
    public class RefCountingDataReader : DataReaderWrapper
    {
        private readonly DatabaseConnectionWrapper connectionWrapper;

        public RefCountingDataReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
            : base(innerReader)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (innerReader == null) throw new ArgumentNullException("innerReader");

            connectionWrapper = connection;
            connectionWrapper.AddRef();
        }

        public override void Close()
        {
            if (!IsClosed)
            {
                base.Close();
                connectionWrapper.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsClosed)
                {
                    base.Dispose(true);
                    connectionWrapper.Dispose();
                }
            }
        }
    }
}
