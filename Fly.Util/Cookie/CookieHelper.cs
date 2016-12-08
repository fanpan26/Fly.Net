using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fly.Util.Cookie
{
    public class CookieHelper
    {
        //简单的写cookie
        public static void WriteCookie(string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["FLY"];
            if (cookie == null)
            {
                cookie = new HttpCookie("FLY");
            }
            cookie.Values.Set(key, value);
            //保存7天
            cookie.Expires = DateTime.Now.AddDays(7);
            HttpContext.Current.Response.SetCookie(cookie);
        }
        //读cookie
        public static string ReadCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["FLY"];

            if (cookie != null && cookie[key].ToString() != "")
            {
                return cookie[key].ToString();
            }
            return "";
        }
    }
}
