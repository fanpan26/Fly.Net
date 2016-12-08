using Fly.IService;
using Fly.Model.Result;
using Fly.Service.Factory;
using Fly.Util.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly.UI.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public JsonResult Upload(HttpPostedFileBase file)
        {
            var upload = FileUploadHelper.Upload(file, Server.MapPath("/upload/"));
            //这个业务应该在upload里处理
            var result = Json(new JsonResultModel { status = upload == null ? 1 : 0, msg = upload == null ? "上传失败，请重试" : "", data = upload }, JsonRequestBehavior.DenyGet);
            return result;
        }

        [Filters.FlyUserAuthorize]
        public JsonResult UploadPhoto(HttpPostedFileBase file, int userid = 0)
        {
            var upload = FileUploadHelper.Upload(file, Server.MapPath("/upload/"));
            if (upload != null)
            {
                string url = upload.GetType().GetProperty("src").GetValue(upload).ToString();
                IUserService userService = FlyFactory.CreateUserService();
                userService.Update(new Model.User
                {
                    userid = userid,
                    photo = url
                }, Model.Enum.UpdateUserType.Photo);
            }
            //这个业务应该在upload里处理
            var result = Json(new JsonResultModel { status = upload == null ? 1 : 0, msg = upload == null ? "上传失败，请重试" : "", data = upload }, JsonRequestBehavior.DenyGet);
            return result;
        }
    }
}