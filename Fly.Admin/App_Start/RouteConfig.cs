using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //置顶或者取消
            routes.MapRoute(
                name: "top",
                url: "top",
                defaults: new { controller = "Home", action = "UpdateArticleTop", id = UrlParameter.Optional }
            );
            //加精或者取消
            routes.MapRoute(
                name: "cream",
                url: "cream",
                defaults: new { controller = "Home", action = "UpdateArticleCream", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
