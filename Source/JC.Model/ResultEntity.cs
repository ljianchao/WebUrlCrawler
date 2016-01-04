using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.Model
{
    public class ResultEntity
    {
        /// <summary>
        /// 返回状态，操作是否成功
        /// </summary>
        public bool ExcutRetStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 返回结果集，如果有需要返回的，就返回结果集
        /// </summary>
        public object ObjRet
        {
            get;
            set;
        }

        /// <summary>
        /// 错误时，封装的错误信息
        /// </summary>
        public string StrErrMsg
        {
            get;
            set;
        }
    }
}
