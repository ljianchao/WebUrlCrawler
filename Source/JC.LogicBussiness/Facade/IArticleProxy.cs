using JC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.LogicBussiness.Facade
{
    public interface IArticleProxy
    {
        /// <summary>
        /// 获取所有文章分类
        /// </summary>
        /// <returns></returns>
        IDictionary<int, string> GetAllArticleCategories();

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        ResultEntity AddArticle(Article article);
    }
}
