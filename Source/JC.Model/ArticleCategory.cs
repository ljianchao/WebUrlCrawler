using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Model
{
    /// <summary>
    /// 文章分类
    /// Writer：刘键超，2015-1-9
    /// </summary>
    [Serializable]
    public class ArticleCategory
    {
        public int ArticleCategoryId { get; set; }
        public int ParentId { get; set; }
        public string ArticleCategoryName { get; set; }
        public DateTime Createtime { get; set; }
    }
}
