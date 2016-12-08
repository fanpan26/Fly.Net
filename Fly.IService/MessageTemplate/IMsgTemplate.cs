using Fly.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.IService.MessageTemplate
{
    public interface IMsgTemplate
    {
        /// <summary>
        /// 创建一条消息
        /// </summary>
        /// <param name="operateName"></param>
        /// <param name="articleName"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        string CreateMessage(string operateName, string articleName, MsgTemplateType msgType);
        string CreateHref(int artid);
    }
}
