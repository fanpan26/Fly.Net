using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Article
{
    public class Comment
    {
        public int UserId { get; set; }
        public int ArtId { get; set; }
        public int ToCommentId { get; set; }
        public string Content { get; set; }
        public int ToUserId { get; set; }
    }
}
