using Fly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Article
{
    public class ArticleBase
    {
        public int id { get; set; }
       
        public int category { get; set; }
        public string category_name { get; set; }
        public string title { get; set; }
        public string artcontent { get; set; }
        public DateTime addtime { get; set;}

        public String addtimestr
        {
            get
            {
                return addtime.ToTimeText();
            }
        }
        public DateTime updatetime { get; set; }

        public int visitor_count { get; set; }
        public int comment_count { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ArticleModel : ArticleBase
    {
        public User user { get; set; }
        public ArticleSetting setting { get; set; }
    }

   

    public class ArticleSetting
    {
        public bool istop { get; set; }
        public bool isover { get; set; }
        public bool iscream { get; set; }

        public int reward { get; set; }
    }
}
