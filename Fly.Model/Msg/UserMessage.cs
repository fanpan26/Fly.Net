using Fly.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Msg
{
    public class UserMessage
    {
        //msgtype,fromuser,touser,artid,msg,addtime,isread

        public int msgid { get; set; }
        public MsgTemplateType msgtype { get; set; }
        public int fromuser { get; set; }
        public int touser { get; set; }
        public int artid { get; set; }
        public string msg { get; set; }
        public DateTime addtime { get; set; } = DateTime.Now;
    }
}
