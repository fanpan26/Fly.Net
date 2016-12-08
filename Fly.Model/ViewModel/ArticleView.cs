using Fly.Model.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.ViewModel
{
    public class ArticleView : ArticleBase
    {
        public int userid { get; set; }
        public string nickname { get; set; }
        public string photo { get; set; }
        public bool istop { get; set; }
        public bool iscream { get; set; }
        public bool isover { get; set; }
        public int reward { get; set; }
    }
}
