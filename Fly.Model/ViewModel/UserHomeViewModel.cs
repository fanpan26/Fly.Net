using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.ViewModel
{
    public class UserHomeViewModel
    {
        public User user { get; set; }
        public IEnumerable<ArticleCommentViewModel> commentRecords { get; set; }

        public IEnumerable<ArticleView> articleRecords { get; set; }
    }
}
