using System;
using System.Data.Common;
using System.Threading;

namespace JC.DataAccess
{
    /// <summary>
    /// 这是一个用来控制关闭在transaction pooling中的connection
    /// 的帮助类。实际上，我们只能在每个人都使用connection完成
    /// 工作后，才能关闭connection，因此，我们需要进行引用计数
    /// </summary>
    /// <remarks>
    /// 用户code不应该直接使用这个类-它通常被DAAB提供者的作者
    /// 在内部使用DAAB的方法时进行控制connections.
    /// 
    /// </remarks>
    public class DatabaseConnectionWrapper : IDisposable
    {
        /// <summary>
        /// 引用计数
        /// </summary>
        private int refCount;

        /// <summary>
        /// 我们正在管理的底层connection
        /// </summary>
        public DbConnection Connection { get; set; }

        public DatabaseConnectionWrapper(DbConnection connection)
        {
            Connection = connection;
            refCount = 1;
        }

        /// <summary>
        /// wrapper是否已经释放了connection对象？
        /// </summary>
        public bool IsDisposed
        {
            get { return refCount == 0; }
        }

        #region IDisposable Members

        /// <summary>
        /// 递减connection的引用数量，如果refcount等于0，关闭底层connection
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //以原子操作的形式递减指定变量的值并存储结果
                int count = Interlocked.Decrement(ref refCount);
                if (count == 0)
                {
                    //关闭连接
                    Connection.Dispose();
                    Connection = null;
                    //请求系统不要调用指定对象的终结器
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// 递增被封装的connection引用的数量
        /// </summary>
        /// <returns></returns>
        public DatabaseConnectionWrapper AddRef()
        {
            Interlocked.Increment(ref refCount);
            return this;
        }

        #endregion
    }
}
