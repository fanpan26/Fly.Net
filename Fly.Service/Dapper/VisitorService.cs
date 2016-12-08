using Fly.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly.Model;
using Fly.Model.Common;
using Dapper;
using System.Data;

namespace Fly.Service.Dapper
{
    public class VisitorService : IVisitorService
    {
        /// <summary>
        /// 添加一条访问记录，规则 30分钟内增加，否则不增加，刷新页面无效
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Visitor model)
        {
            if (model.VisitorType == Model.Enum.VisitorType.User && model.MainId == model.VisitorId) {
                //自己访问自己主页，不做记录
                return true;
            }
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Proc_Fly_AddVisitor", new { mainid = model.MainId, visitorid = model.VisitorId, vtype = model.VisitorType });

            //这里结果可能不准，执行的是存储过程
            bool result = DB.DBHelper.Execute(param, false, CommandType.StoredProcedure);
            return result;
        }


        public IEnumerable<Visitor> Query(string sql)
        {
            throw new NotImplementedException();
        }

        public PageView<Visitor> QueryPageList(PageSearchOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
