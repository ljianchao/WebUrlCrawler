using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JC.Model
{

    public class Article
    {
        public int ArticleId { get; set; }

        public int ArticleCategoryId { get; set; }

        public string ArticleName { get; set; }

        public string ArticleUrl { get; set; }

        public DateTime ArticlePublishtime { get; set; }

        public DateTime Createtime { get; set; }
    }
}
