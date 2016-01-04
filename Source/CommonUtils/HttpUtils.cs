using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    /// <summary>
    /// http请求封装工具
    /// </summary>
    public class HttpUtils
    {
        /// <summary>
        /// 获取http请求的字符串
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <returns>服务器响应的字符串</returns>
        public static string GetReponseString(string url)
        {
            string strContent = string.Empty;
            try
            {
                HttpWebRequest myReq = null;
                HttpWebResponse myRes = null;
                myReq = (HttpWebRequest)WebRequest.Create(url);
                myRes = (HttpWebResponse)myReq.GetResponse();
                StreamReader sr = new StreamReader(myRes.GetResponseStream());
                strContent = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strContent;
        }
    }
}
