using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class HandleData
    {
        /// <summary>
        /// 即将处理url队列
        /// </summary>
        public static readonly Queue<CrawlerItem> handingUrlQueue = new Queue<CrawlerItem>();

        /// <summary>
        /// 响应内容队列
        /// </summary>
        public static readonly Queue<string> responseContentQueue = new Queue<string>();
    }
}
