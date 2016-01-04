using System;
using System.Configuration;

namespace GetWebHref
{
    /// <summary>
    /// 应用helper
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// 每次请求相隔时间，单位毫秒
        /// </summary>
        public static int RequestTimeSpan;

        static AppHelper()
        {
            string requestTimeSpan = ConfigurationManager.AppSettings["RequestTimeSpan"];
            RequestTimeSpan = string.IsNullOrEmpty(requestTimeSpan) ? 100 : Convert.ToInt32(requestTimeSpan);
                                            
        }
    }
}
