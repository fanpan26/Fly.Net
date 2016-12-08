using Fly.IService;
using Fly.Model.Msg;
using Fly.Service.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Service.Message
{
   public class MessageSender
    {
        IUserMessageService service = FlyFactory.CreateUserMessageService();
        /// <summary>
        /// 发送一条消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(UserMessage msg)
        {
            service.Add(msg);
        }
    }
}
