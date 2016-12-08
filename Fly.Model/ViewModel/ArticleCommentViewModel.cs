using Fly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.ViewModel
{
    public class ArticleCommentViewModel
    {
        private const string deleteMsg = "#该评论已被管理员删除";
        public int id { get; set; }
        public int artid { get; set; }
        public string title { get; set; }
        public int userid { get; set; }
        public string nickname { get; set; }
        public string photo { get; set; }
        public int publisher { get; set; }
        public int toid { get; set; }
        public string comment { get; set; }

        public string comments
        {
            get
            {
                if (isdel)
                {
                    return deleteMsg;
                }
                else
                {
                    if (toid > 0)
                    {
                        comment = comment.Replace("@" + tonickname, "");
                        return $"{comment}//@{tonickname} {tocomment}";
                    }
                    return comment;
                }
            }
        }
        public DateTime addtime { get; set; }
        public int touserid { get; set; }
        public string tonickname { get; set; }

        private string _tocomment;
        public string tocomment
        {
            get
            {
                return todel ? deleteMsg : _tocomment;
            }
            set { _tocomment = value; }
        }
        public int recount { get; set; }
        public string addtimestr
        {
            get
            {
                return addtime.ToTimeText();
            }
        }
        public bool selfrecommend { get; set; }
        public bool author
        {
            get
            {
                return publisher == userid;
            }
        }

        public bool isbest { get; set; }
        public bool isself { get; set; }

        public bool isdel { get; set; }

        public bool todel { get; set; }

        public bool isadmin { get; set; }

    }
}
