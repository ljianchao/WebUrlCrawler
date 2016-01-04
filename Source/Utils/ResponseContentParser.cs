using System.Collections.Generic;
using JC.Model;
using JC.LogicBussiness.Facade;
using JC.LogicBussiness.Imp;
using log4net;
using System;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// 解析web响应的内容
    /// </summary>
    public class ResponseContentParser
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(ResponseContentParser));

        /// <summary>
        /// 解析器
        /// </summary>
        private IWebContentParser parser;

        /// <summary>
        /// 文章所属分类
        /// </summary>
        private int articleCategoryId;

        private IArticleProxy articleProxy;

        /// <summary>
        /// 需处理url请求的总数量
        /// </summary>
        private int urlCount;

        public ResponseContentParser(int articleCategoryId, int urlCount, IWebContentParser webContentParser)
        {
            if (urlCount <= 0)
                throw new ArgumentException("请求的url的数量必须大于0");

            this.articleCategoryId = articleCategoryId;
            this.urlCount = urlCount;
            this.parser = webContentParser;

            this.articleProxy = new ArticleImpl();
        }

        /// <summary>
        /// 解析响应的web内容
        /// </summary>
        public void ParseContent()
        {
            DateTime startTime = DateTime.Now;
            logInfo.InfoFormat("[parser]开始处理请求时间：{0}, 总共需要处理的数量为：{1}", startTime, urlCount);

            int handledUrlCount = 0;

            while (true)
            {
                while (HandleData.responseContentQueue.Count > 0)
                {
                    string content = HandleData.responseContentQueue.Dequeue();

                    TakeUrls(content);

                    Interlocked.Increment(ref handledUrlCount);
                    logInfo.InfoFormat("[parser]已处理请求的数量：{0}", handledUrlCount);
                }

                if (handledUrlCount == this.urlCount)
                    break;

            }

            DateTime endTime = DateTime.Now;
            logInfo.InfoFormat("[parser]结束处理请求时间：{0}, 总共处理的数量为：{1}, 总共耗时：{2}",
                endTime, handledUrlCount, DateUtils.DateDiffForMillisecond(startTime, endTime));
        }

        /// <summary>
        /// 解析内容，并插入到数据库中
        /// </summary>
        /// <param name="content"></param>
        private void TakeUrls(string content)
        {
            try
            {
                IList<Article> articleList = parser.ParserHtmlToArticle(articleCategoryId, content);
                if (articleList != null)
                {
                    foreach (var article in articleList)
                    {
                        articleProxy.AddArticle(article);
                    }
                }
            }
            catch (Exception ex)
            {
                logInfo.ErrorFormat("解析web响应内容，并插入到数据库中异常，原因：{0}", ex.Message);
            }           
        }
    }
}
