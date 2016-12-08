using Fly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.ViewModel
{
   public class UserMessageViewModel
    {
        public int id { get; set; }
        public Enum.MsgTemplateType msgtype { get; set; }
        public string nickname { get; set; }
        public int artid { get; set; }
        public string href
        {
            get;set;
        }
        public DateTime addtime { get; set; }
        public string addtimestr
        {
            get
            {
                return addtime.ToTimeText();
            }
        }

        public string msg
        {
            get;set;
        }
        public string title { get; set; }
    }
}
