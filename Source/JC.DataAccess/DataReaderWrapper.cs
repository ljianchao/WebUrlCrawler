using System;
using System.Data;

namespace JC.DataAccess
{
    /// <summary>
    /// 采用Wrapper模式，对IDataReader进行包装
    /// </summary>
    public abstract class DataReaderWrapper : MarshalByRefObject, IDataReader
    {
        private readonly IDataReader innerReader;

        protected DataReaderWrapper(IDataReader innerReader)
        {
            this.innerReader = innerReader;
        }

        public IDataReader InnerReader { get { return innerReader; } }

        /// <summary>
        /// 关闭 System.Data.IDataReader 对象
        /// </summary>
        public virtual void Close()
        {
            if (!innerReader.IsClosed)
            {
                innerReader.Close();
            }
        }

        /// <summary>
        /// 返回一个 System.Data.DataTable，它描述 System.Data.IDataReader 的列元数据
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetSchemaTable()
        {
            return innerReader.GetSchemaTable(); 
        }

        /// <summary>
        /// 当读取批处理 SQL 语句的结果时，使数据读取器前进到下一个结果
        /// </summary>
        /// <returns>如果存在多个行，则为 true；否则为 false</returns>
        public virtual bool NextResult()
        {
            return innerReader.NextResult();
        }

        /// <summary>
        /// 使 System.Data.IDataReader 前进到下一条记录。
        /// </summary>
        /// <returns>如果存在多个行，则为 true；否则为 false。</returns>
        public virtual bool Read()
        {           
            return innerReader.Read();
        }

        /// <summary>
        /// 获取一个值，该值指示当前行的嵌套深度。
        /// </summary>
        public virtual int Depth
        {
            get
            {
                return innerReader.Depth;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示数据读取器是否已关闭。
        /// </summary>
        public virtual bool IsClosed
        {
            get
            {
                return innerReader.IsClosed;
            }
        }

        /// <summary>
        ///  通过执行 SQL 语句获取更改、插入或删除的行数
        /// </summary>
        /// <returns>
        /// 已更改、插入或删除的行数；如果没有任何行受到影响或语句失败，则为 0；-1 表示 SELECT 语句
        /// </returns>
        public virtual int RecordsAffected
        {
            get
            {
                return innerReader.RecordsAffected;
            }
        }

        /// <summary>
        /// 获取要查找的字段的名称
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual string GetName(int i)
        {
            return innerReader.GetName(i);
        }

        /// <summary>
        /// 获取指定字段的数据类型信息
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual string GetDataTypeName(int i)
        {
            return innerReader.GetDataTypeName(i);
        }

        /// <summary>
        // 获取与从 System.Data.IDataRecord.GetValue(System.Int32) 返回的 System.Object 类型对应的
        // System.Type 信息
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual Type GetFieldType(int i)
        {
            return innerReader.GetFieldType(i);
        }

        /// <summary>
        /// 返回指定字段的值。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual object GetValue(int i)
        {
            return innerReader.GetValue(i);
        }

        /// <summary>
        /// 使用当前记录的列值来填充对象数组
        /// </summary>
        /// <param name="values">要将属性字段复制到的 System.Object 的数组</param>
        /// <returns>数组中 System.Object 的实例的数目</returns>
        public virtual int GetValues(object[] values)
        {
            return innerReader.GetValues(values);
        }

        /// <summary>
        /// 返回命名字段的索引
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual int GetOrdinal(string name)
        {
            return innerReader.GetOrdinal(name);
        }

        /// <summary>
        /// 获取指定列的布尔值形式的值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual bool GetBoolean(int i)
        {
            return innerReader.GetBoolean(i);
        }

        /// <summary>
        ///  获取指定列的 8 位无符号整数值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual byte GetByte(int i)
        {
            return innerReader.GetByte(i);
        }

        //
        // 摘要: 
        //     从指定的列偏移量将字节流作为数组从给定的缓冲区偏移量开始读入缓冲区。
        //
        // 参数: 
        //   i:
        //     从零开始的列序号。
        //
        //   fieldOffset:
        //     字段中的索引，从该索引位置开始读取操作。
        //
        //   buffer:
        //     要将字节流读入的缓冲区。
        //
        //   bufferoffset:
        //     开始读取操作的 buffer 索引。
        //
        //   length:
        //     要读取的字节数。
        //
        // 返回结果: 
        //     读取的实际字节数。
        //
        // 异常: 
        //   System.IndexOutOfRangeException:
        //     传递的索引位于 0 至 System.Data.IDataRecord.FieldCount 的范围之外。
        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// 获取指定列的字符值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual char GetChar(int i)
        {
            return innerReader.GetChar(i);
        }

        //
        // 摘要: 
        //     从指定的列偏移量将字符流作为数组从给定的缓冲区偏移量开始读入缓冲区。
        //
        // 参数: 
        //   i:
        //     从零开始的列序号。
        //
        //   fieldoffset:
        //     行中的索引，从该索引位置开始读取操作。
        //
        //   buffer:
        //     要将字节流读入的缓冲区。
        //
        //   bufferoffset:
        //     开始读取操作的 buffer 索引。
        //
        //   length:
        //     要读取的字节数。
        //
        // 返回结果: 
        //     读取的实际字符数。
        //
        // 异常: 
        //   System.IndexOutOfRangeException:
        //     传递的索引位于 0 至 System.Data.IDataRecord.FieldCount 的范围之外。
        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// 返回指定字段的 GUID 值。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual Guid GetGuid(int i)
        {
            return innerReader.GetGuid(i);
        }

        /// <summary>
        /// 获取指定字段的 16 位有符号整数值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual short GetInt16(int i)
        {
            return innerReader.GetInt16(i);
        }

        /// <summary>
        /// 获取指定字段的 32 位有符号整数值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual int GetInt32(int i)
        {
            return innerReader.GetInt32(i);
        }

        /// <summary>
        /// 获取指定字段的 64 位有符号整数值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual long GetInt64(int i)
        {
            return innerReader.GetInt64(i);
        }

        /// <summary>
        /// 获取指定字段的单精度浮点数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual float GetFloat(int i)
        {
            return innerReader.GetFloat(i);
        }

        /// <summary>
        /// 获取指定字段的双精度浮点数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual double GetDouble(int i)
        {
            return innerReader.GetDouble(i);
        }

        /// <summary>
        /// 获取指定字段的字符串值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual string GetString(int i)
        {
            return innerReader.GetString(i);
        }

        /// <summary>
        /// 获取指定字段的固定位置的数值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual decimal GetDecimal(int i)
        {
            return innerReader.GetDecimal(i);
        }

        /// <summary>
        /// 获取指定字段的日期和时间数据值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual DateTime GetDateTime(int i)
        {
            return innerReader.GetDateTime(i);
        }

        /// <summary>
        /// 返回指定的列序号的 System.Data.IDataReader
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual IDataReader GetData(int i)
        {
            return innerReader.GetData(i);
        }

        /// <summary>
        /// 返回是否将指定字段设置为空
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual bool IsDBNull(int i)
        {
            return innerReader.IsDBNull(i);
        }

        /// <summary>
        /// 获取当前行中的列数
        /// </summary>
        /// <returns>
        /// 如果未放在有效的记录集中，则为 0；如果放在了有效的记录集中，则为当前记录的列数。默认值为 -1。
        /// </returns>
        public virtual int FieldCount
        {
            get
            {
                return innerReader.FieldCount;
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                return innerReader[i];
            }
        }

        object IDataRecord.this[string name]
        {
            get
            {
                return innerReader[name];
            }
        }

        /// <summary>
        /// 执行应用程序定义的freeing,releasing,or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 当释放的时候，关闭容器中的DataReader对象
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!innerReader.IsClosed)
                {
                    innerReader.Dispose();
                }
            }
        }
    }
}
    