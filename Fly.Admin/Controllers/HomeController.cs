using Fly.IService;
using Fly.Model.Result;
using Fly.Service.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly.Admin.Controllers
{
    public class HomeController : Controller
    {
        IArticleService service = FlyFactory.CreateArticleService();
        public ActionResult Index(int pageIndex = 1, int pageSize = 20)
        {
            var data = service.QueryPageList(new Model.Common.PageSearchOptions
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            return View(data);
        }

        public JsonResult UpdateArticleTop(int artid, bool value, int toUserId)
        {
            bool result = service.UpdateArticleTop(artid, toUserId, value);
            return Json(new JsonResultModel { status = result ? 0 : 1, msg = result ? "" : "设置失败" }, JsonRequestBehavior.DenyGet);
        }
        public JsonResult UpdateArticleCream(int artid, bool value, int toUserId)
        {
            bool result = service.UpdateArticleCream(artid, toUserId, value);
            return Json(new JsonResultModel { status = result ? 0 : 1, msg = result ? "" : "设置失败" }, JsonRequestBehavior.DenyGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}