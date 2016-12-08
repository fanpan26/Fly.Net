using Fly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model
{
    public class User
    {
        public int userid { get; set; }
        public string nickname { get; set; }
        public string photo { get; set; }
        public string account { get; set; }
        public string pwd { get; set; }
        public DateTime addtime { get; set; }
        public string province { get; set; }
        private string _city;
        public string city { get { return _city; } set { _city = value ?? "未知"; } }

        private string _sign;
        public string sign { get { return _sign; } set { _sign = value ?? "这家伙很懒，没有写签名"; } }
        public int score { get; set; }
        public int sex { get; set; }
    }

    public class UserVisitor : User
    {
        public string addtimestr
        {
            get
            {
                return addtime.ToTimeText();
            }
        }
    }
}
