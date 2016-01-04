using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JC.Model;

namespace Utils
{
    /// <summary>
    /// web响应内容操作接口
    /// </summary>
    public interface IWebContentParser
    {
        /// <summary>
        /// 解析web响应内容，并获取文章对象列表
        /// </summary>
        /// <param name="articleCategoryId">所属文章分类</param>
        /// <param name="webResponseContent">web请求响应的内容</param>
        /// <returns></returns>
        IList<Article> ParserHtmlToArticle(int articleCategoryId, string webResponseContent);
    }
}
