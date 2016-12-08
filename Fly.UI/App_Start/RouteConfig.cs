using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //首页分页配置
            routes.MapRoute(
                name: "index",
                url: "pages/{pageIndex}",
                defaults: new { controller = "Home", action = "Index", pageIndex = UrlParameter.Optional }
            );
            //发布问题
            routes.MapRoute(
                name: "add",
                url: "add",
                defaults: new { controller = "Home", action = "Add" }
            );
            //问题详情
            routes.MapRoute(
             name: "detail",
             url: "detail/{id}",
             defaults: new { controller = "Home", action = "Detail", id = UrlParameter.Optional }
            );
            //用户注册
            routes.MapRoute(
             name: "register",
             url: "register",
             defaults: new { controller = "User", action = "Register"}
            );
            //用户登录
            routes.MapRoute(
             name: "login",
             url: "login",
             defaults: new { controller = "User", action = "UserLogin" }
            );
            //用户评论
            routes.MapRoute(
            name: "reply",
            url: "reply",
            defaults: new { controller = "User", action = "Reply" }
           );
            //用户点赞
            routes.MapRoute(
            name: "good",
            url: "good",
            defaults: new { controller = "User", action = "Recommand" }
           );
            //获取文章评论（答案）
            routes.MapRoute(
            name: "comment",
            url: "comment",
            defaults: new { controller = "Home", action = "GetArticleComment" }
           );
            //获取用户消息 user  usermessage
            routes.MapRoute(
            name: "msg",
            url: "msg",
            defaults: new { controller = "User", action = "UserMessage" }
           );
            //用户删除消息
            routes.MapRoute(
            name: "delmsg",
            url: "delmsg",
            defaults: new { controller = "User", action = "DeleteMessage" }
           );

            //提交发布问题表单
            routes.MapRoute(
               name: "publish",
               url: "publish",
               defaults: new { controller = "Home", action = "Publish" }
           );
            //提交发布问题表单
            routes.MapRoute(
               name: "mine",
               url: "u/{userid}",
               defaults: new { controller = "User", action = "Home" }
           );
            //
            //查询用户的发表问题
            routes.MapRoute(
               name: "uindex",
               url: "api/mine-jie",
               defaults: new { controller = "User", action = "QueryUserArticles" }
           );
            //更新用户信息
            routes.MapRoute(
              name: "updateinfo",
              url: "user/update",
              defaults: new { controller = "User", action = "UpdateInfo" }
          );
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );

        }
    }
}
