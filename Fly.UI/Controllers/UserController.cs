using Fly.IService;
using Fly.Model;
using Fly.Model.Enum;
using Fly.Model.Result;
using Fly.Model.ViewModel;
using Fly.Service.Factory;
using Fly.UI.Filters;
using Fly.Util;
using Fly.Util.Config;
using Fly.Util.Cookie;
using Fly.Util.ValidateCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly.UI.Controllers
{
    public class UserController : Controller
    {

        IUserService userService = FlyFactory.CreateUserService();

        // GET: User
        [FlyUserAuthorize]
        public ActionResult Index(int userid = 0)
        {

            var user = userService.QueryOne(userid);
            ViewBag.Visitors = userService.GetVisitors(userid);

            return View(user);
        }

        public ActionResult Home(int userid = 0)
        {
            if (userid == 0) {
                return RedirectToAction("PageNotFound", "Error");
            }
            IVisitorService visitorService = FlyFactory.CreateVisitorService();
            visitorService.Add(new Visitor
            {
                MainId = userid,
                VisitorId = FlyFactory.CreateUserProvider().GetUserId(),
                VisitorType = Model.Enum.VisitorType.User
            });
            var result = userService.QueryMultiple(null, new { userid = userid });
           // ViewBag.UserViewModel = result;
            return View(result);
        }

        [FlyUserAuthorize]
        public ActionResult Set(int userid = 0)
        {
           var user = userService.QueryOne(userid);
            return View(user);
        }
        public ActionResult Message()
        {
            return View();
        }

        //登录
        public ActionResult Login()
        {
            return View();
        }
        //注册 
        public ActionResult Reg()
        {
            ViewBag.Code = ValidateCodeHelper.GetCode();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(RegisterViewModel reginfo)
        {
           
            //验证没有通过
            if (reginfo.vertify.Length > 0) {
                return Json(new JsonResultModel { status = 1, msg = reginfo.vertify }, JsonRequestBehavior.DenyGet);
            }
            //注册逻辑
            //执行注册
            var result = userService.Register(new Model.User
            {
                nickname = reginfo.username,
                account = reginfo.email,
                pwd = reginfo.pass
            });

            if (result.success)
            {
                //默认登录状态，写登录内容
                return Json(new JsonResultModel
                {
                    status = 0,
                    //跳到首页
                    data = new { action = ConfigHelper.GetDefaultRedirectTo(), user = result.user }
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResultModel
                {
                    status = 1,
                    msg = result.msg
                }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UserLogin(User user)
        {
            User userResult = userService.Login(user.account, user.pwd);
            if (userResult != null)
            {
                return Json(new JsonResultModel
                {
                    status = 0,
                    msg = "登录成功",
                    data = new { action = "/", user = userResult }
                }, JsonRequestBehavior.DenyGet);
            }
            return Json(new JsonResultModel
            {
                status = 1,
                msg = "用户名或密码错误，请重试"
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [FlyUserAuthorize]
        [ValidateInput(false)]
        public JsonResult Reply(int jid, int toid = 0, int touserid = 0, int authorid = 0, string content = "", int userid = 0)
        {
            ICommentService service = FlyFactory.CreateCommentService();
            bool result = service.Add(new Model.Article.Comment
            {
                ArtId = jid,
                Content = content,
                ToCommentId = toid,
                UserId = userid,
                ToUserId = toid > 0 ? touserid : authorid
            });
            return Json(new JsonResultModel
            {
                status = result ? 0 : 1,
                msg = result ? "成功" : "评论失败，请重试"
            }, JsonRequestBehavior.DenyGet);
        }

        [FlyUserAuthorize]
        public JsonResult Recommand(int commentid, int userid = 0,int artid=0,int touserid=0)
        {
            ICommentService service = FlyFactory.CreateCommentService();
            bool result = service.AddCommentRecomand(commentid, userid,artid,touserid);
            return Json(new JsonResultModel
            {
                status = result ? 0 : 1,
                msg = result ? "成功" : "操作失败，请重试"
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [FlyUserAuthorize]
        public JsonResult UserMessage(int userid = 0)
        {
            IUserMessageService service = FlyFactory.CreateUserMessageService();
            var result = service.QueryMessages(userid);
            return Json(new JsonResultModel { status = 0, data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [FlyUserAuthorize]
        public JsonResult DeleteMessage(int userid = 0, int id = 0)
        {
            IUserMessageService service = FlyFactory.CreateUserMessageService();
            var result = service.DeleteMessage(userid, id);
            return Json(new JsonResultModel { status = result ? 0 : 1, data = result }, JsonRequestBehavior.DenyGet);
        }

        [FlyUserAuthorize]
        public JsonResult QueryUserArticles(int pageIndex, int userid = 0)
        {
            var result = userService.QueryUserArticles(pageIndex, userid,20);
            return Json(new JsonResultModel { status = 0, data = new { rows = result.List, count = result.TotalCount,pages=result.TotalPageCount } }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [FlyUserAuthorize]
        public JsonResult UpdateInfo(User user, UpdateUserType type, int userid = 0)
        {
            user.userid = userid;
            bool result = userService.Update(user, type);
            return Json(new JsonResultModel { status = result ? 0 : 1, msg = result ? "" : "更新失败，请重试" }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [FlyUserAuthorize]
        public JsonResult UpdatePwd(string nowpass,string pass,string repass, int userid = 0)
        {
            if (pass != repass) {
                return Json(new JsonResultModel { status = 1, msg = "两次输入的密码不一致" }, JsonRequestBehavior.DenyGet);
            }
            bool result = userService.UpdatePwd(nowpass, pass,userid);
            if (result) {
                CookieHelper.WriteCookie(FlyConst.FLY_USER_TOKEN_KEY, "");
            }
            return Json(new JsonResultModel { status = result ? 0 : 1, msg = result ? "" : "原密码有误" }, JsonRequestBehavior.DenyGet);
        }
    }
}