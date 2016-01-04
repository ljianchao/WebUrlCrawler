using JC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.Service.ServiceFacade
{
    public interface IArticleDal
    {
        /// <summary>
        /// 新增文章
        /// </summary>
        /// <returns></returns>
        ResultEntity AddArticle(Article article);
    }
}
