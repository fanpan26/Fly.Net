using Fly.IService;
using Fly.Model.Result;
using Fly.Service.Dapper;
using Fly.Service.Factory;
using Fly.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly.UI.Controllers
{
    public class HomeController : Controller
    {
        IArticleService service = FlyFactory.CreateArticleService();
        IVisitorService visitorService = FlyFactory.CreateVisitorService();
        IFlyUserProvider userProvider = FlyFactory.CreateUserProvider();
        ICommentService commenService = FlyFactory.CreateCommentService();

        //首页
        public ActionResult Index(int pageIndex = 1, int pageSize = 20)
        {
            var data = service.QueryPageList(new Model.Common.PageSearchOptions
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            return View(data);
        }


        //问题详情
        public ActionResult Detail(int? id, int userid = 0)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var article = service.QueryOne(id.Value);
            if (article.id == 0)
            {
                //应该去404
                return RedirectToAction("Index");
            }
            //添加访问记录
            visitorService.Add(new Model.Visitor
            {
                MainId = id.Value,
                VisitorId = userProvider.GetUserId(),
                VisitorType = Model.Enum.VisitorType.Article
            });
            ViewBag.ArticleId = id.Value;
            //右侧列表
            ViewBag.RightArticleList = service.QueryTop10(10);
            //答案列表
            return View(article);
        }

        /// <summary>
        /// 获取文章的评论列表，ajax
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult GetArticleComment(int artid, int userid)
        {
           var list = commenService.QueryArticleComment(artid, userid);
            return Json(new JsonResultModel { data = list, status = 0 }, JsonRequestBehavior.AllowGet);
        }

        //发布问题
        public ActionResult Add()
        {
            ICategoryService categoryService = FlyFactory.CreateCategoryService();
            var result = categoryService.Query(null);
            //分类
            ViewBag.Categorys = result;
            return View();
        }

        //提交发布问题表单
        [HttpPost]
        [ValidateAntiForgeryToken]
        [FlyUserAuthorize]
        [ValidateInput(false)]
        public ActionResult Publish(string title, string content, int @class, int experience, string vercode,int userid)
        {
            bool result = service.Add(new Model.ViewModel.ArticleView
            {
                title = title,
                artcontent = content,
                reward = experience,
                category = @class,
                userid = userid
            });
            if (result)
            {
                return Json(new JsonResultModel
                {
                    status = 0,
                    data =new { action="/"}
                }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new JsonResultModel
                {
                    status = 1,
                    msg = "提交失败"
                }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}