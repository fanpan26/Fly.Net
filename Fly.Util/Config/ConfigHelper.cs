using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Util.Config
{
    public sealed class ConfigHelper
    {
        //默认头像
        private const string KEY_USER_REGISTER_DEFAULT_LOGO = "UserRegister_DefaultLogo";
        //默认跳转页面
        private const string KEY_USER_REGISTER_REDIRECT_TO = "UserRegister_RedirectTo";

        private static string getValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetDefaultPhoto()
        {
            return getValue(KEY_USER_REGISTER_DEFAULT_LOGO);
        }

        public static string GetDefaultRedirectTo()
        {
            return getValue(KEY_USER_REGISTER_REDIRECT_TO);
        }
    }
}
