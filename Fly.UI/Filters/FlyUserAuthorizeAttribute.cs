using Fly.Model.Result;
using Fly.Service.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FlyUserAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userid = FlyFactory.CreateUserProvider().GetUserId();
            if (userid == 0)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new JsonResultModel { status = 0, data = new { action = "/user/login" } }
                    };
                }
                else {
                    filterContext.Result = new RedirectResult("/user/login");
                }
            }
            else
            {
                filterContext.ActionParameters["userid"] = userid;
            }

            base.OnActionExecuting(filterContext);
        }

    }
}