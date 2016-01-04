using System;
using System.Collections.Generic;
using System.Linq;
using JC.Model;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;

namespace Utils
{
    public class TestParser : IWebContentParser
    {
        public IList<Article> ParserHtmlToArticle(int articleCategoryId, string webResponseContent)
        {
            IList<Article> articles = new List<Article>();
            try
            {
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument
                {
                    OptionAddDebuggingAttributes = false,
                    OptionAutoCloseOnEnd = true,
                    OptionFixNestedTags = true,
                    OptionReadEncoding = true
                };
                htmlDoc.LoadHtml(webResponseContent);
                var html = htmlDoc.DocumentNode;
                //获取需操作节点对象
                var divHtml = html.CssSelect("div.s8-thread-grid").CssSelect("a");

                Article article = null;

                foreach (var aTagNode in divHtml)
                {
                    var fontTag = aTagNode.CssSelect("font");
                    if (fontTag == null || fontTag.Count() == 0)
                    {
                        continue;
                    }

                    var href = aTagNode.Attributes["href"].Value;
                    if (!Regex.IsMatch(href,UrlMatchRule.matchRule, RegexOptions.Singleline))
                    {
                        continue;
                    }
                    
                    article = new Article();
                    article.ArticleCategoryId = articleCategoryId;
                    article.ArticleName = fontTag.Last().InnerHtml;
                    article.ArticleUrl = href;
                    //TODO：发布时间待获取

                    articles.Add(article);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return articles;
        }
    }
}
