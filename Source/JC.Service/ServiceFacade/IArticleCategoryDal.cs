using JC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.Service.ServiceFacade
{
    public interface IArticleCategoryDal
    {
        /// <summary>
        /// 新增文章分类
        /// </summary>
        /// <returns></returns>
        ResultEntity AddArticleCategory(ArticleCategory articleCategory);

        /// <summary>
        /// 获取所有文章分类
        /// </summary>
        /// <returns></returns>
        IDictionary<int, string> GetAllArticleCategories();
    }
}
