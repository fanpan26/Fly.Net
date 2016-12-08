using Fly.IService.MessageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model.Enum;

namespace Fly.Service.Message
{
    public class MessageTemplate : IMsgTemplate
    {
        public string CreateHref(int artid)
        {
            return $"/detail/{artid}";
        }

        /// <summary>
        /// 创建一条消息
        /// </summary>
        /// <param name="operateName"></param>
        /// <param name="articleName"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public string CreateMessage(string operateName, string articleName, MsgTemplateType msgType)
        {
            string result = string.Empty;
            switch (msgType)
            {
                case MsgTemplateType.NewComment:
                    result = $"<i>{operateName}</i>回答了您的求解<cite>{articleName}</cite>,快去看看吧";
                    break;
                case MsgTemplateType.NewReply:
                    result = $"<i>{operateName}</i>回复了您在<cite>{articleName}</cite>中的解答,快去看看吧";
                    break;
                case MsgTemplateType.CommentRecommand:
                    result = $"<i>{operateName}</i>给您在<cite>{articleName}</cite>中的解答点了一个赞,快去看看吧";
                    break;
                case MsgTemplateType.AdminAddCream:
                    result = $"<i>管理员</i>将您的求解<cite>{articleName}</cite>设置为精帖,快去看看吧";
                    break;
                case MsgTemplateType.AdminAddTop:
                    result = $"<i>管理员</i>将您的求解<cite>{articleName}</cite>设置为置顶,快去看看吧";
                    break;
                case MsgTemplateType.AdminCancelCream:
                    result = $"<i>管理员</i>将您的求解<cite>{articleName}</cite>取消精帖,快去看看吧";
                    break;
                case MsgTemplateType.AdminCancelTop:
                    result = $"<i>管理员</i>将您的求解<cite>{articleName}</cite>取消置顶,快去看看吧";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
