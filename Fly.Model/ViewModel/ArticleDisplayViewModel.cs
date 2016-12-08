using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.ViewModel
{
   public class ArticleDisplayViewModel
    {
        public int artid { get; set; }
        public string title { get; set; }
        public int artcount { get; set; }
        /// <summary>
        /// 1评论 2 查看
        /// </summary>
        public int arttype { get; set; }
    }
}
