using Fly.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model.Common;
using Fly.Model.Msg;
using Fly.Service.DB;
using Fly.Model.ViewModel;
using Fly.Service.Factory;

namespace Fly.Service.Dapper
{
    public class UserMessageService : IUserMessageService
    {
        /// <summary>
        /// 添加一条用户消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(UserMessage model)
        {
            //自己不给自己发消息
            if (model.fromuser == model.touser) { return true; }
            string sql = "INSERT INTO dbo.fly_user_msg (msgtype,fromuser,touser,artid,msg,addtime,isread) VALUES  (@msgtype ,@fromuser,@touser,@artid ,@msg,@addtime ,0 )";
            return DBHelper.Execute(sql, model);
        }

        public bool DeleteMessage(int userid, int id)
        {
            string sql = $"DELETE FROM dbo.fly_user_msg WHERE touser={userid}" + (id > 0 ? $" AND id={id}" : "");
            return DBHelper.Execute(sql, null);
        }

        public IEnumerable<UserMessage> Query(string sql)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<UserMessageViewModel> QueryMessages(int userid)
        {
            string sql = $"SELECT * FROM V_Fly_UserMessage WHERE touser={userid} order by addtime desc";
            var list = DBHelper.Query<UserMessageViewModel>(sql).ToList();
            //给href和content赋值（msg）
            var template = FlyFactory.CreateMessageTemplate();
            list.ForEach(x =>
            {
                x.msg = template.CreateMessage(x.nickname, x.title, x.msgtype);
                x.href = template.CreateHref(x.artid);
            });
            return list;
        }

        public PageView<UserMessage> QueryPageList(PageSearchOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
