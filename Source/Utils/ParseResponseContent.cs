using System.Collections.Generic;
using JC.Model;
using JC.LogicBussiness.Facade;
using JC.LogicBussiness.Imp;
using log4net;

namespace Utils
{
    /// <summary>
    /// 解析web响应的内容
    /// </summary>
    public class ParseResponseContent
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(ParseResponseContent));

        /// <summary>
        /// 响应内容队列
        /// </summary>
        public static readonly Queue<string> responseContentQueue = new Queue<string>();

        /// <summary>
        /// 解析器
        /// </summary>
        private IWebContentParser parser;

        /// <summary>
        /// 文章所属分类
        /// </summary>
        private int articleCategoryId;

        private IArticleProxy articleProxy;

        public ParseResponseContent(int articleCategoryId, IWebContentParser webContentParser)
        {
            this.articleCategoryId = articleCategoryId;
            this.parser = webContentParser;

            this.articleProxy = new ArticleImpl();
        }

        /// <summary>
        /// 解析响应的web内容
        /// </summary>
        public void ParseContent()
        {
            while (true)
            {
                while (responseContentQueue.Count > 0)
                {
                    string content = responseContentQueue.Dequeue();

                    TakeUrls(content);
                }
            }
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
            catch (System.Exception ex)
            {
                logInfo.ErrorFormat("解析web响应内容，并插入到数据库中异常，原因：{0}", ex.Message);
            }           
        }
    }
}
