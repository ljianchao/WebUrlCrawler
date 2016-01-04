using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.Model
{
    public class ResultQueryEntity<T>
    {
        /// <summary>
        /// 结果
        /// </summary>
        public T QueryResult { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string StrErrCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string StrErrMsg { get; set; }
    }
}
