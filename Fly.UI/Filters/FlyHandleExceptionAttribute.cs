using Fly.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly.UI.Filters
{
    public class FlyHandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //如果是ajax请求，返回相应的处理
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new JsonResultModel
                    {
                        msg = "内部异常,请联系管理员",
                        status = 1
                    }
                };
                filterContext.ExceptionHandled = true;
            }
            //否则执行自带方法
            base.OnException(filterContext);

        }

    }
}