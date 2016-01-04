using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JC.Service.ServiceFacade;
using JC.Service.ServiceImp;

namespace JC.LogicBussiness.Imp
{
    public class ArticleImpl : Facade.IArticleProxy
    {
        private IArticleCategoryDal articleCategoryDal = new ArticleCategoryDal();

        private IArticleDal articleDal = new ArticleDal();

        public IDictionary<int, string> GetAllArticleCategories()
        {
            return articleCategoryDal.GetAllArticleCategories();
        }


        public Model.ResultEntity AddArticle(Model.Article article)
        {
            return articleDal.AddArticle(article);
        }
    }
}
