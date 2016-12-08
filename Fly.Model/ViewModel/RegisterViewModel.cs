using Fly.Util.ValidateCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.ViewModel
{
    public class RegisterViewModel
    {
        public string vertify {
            get
            {
                if (pass == null || repass == null)
                {
                    return "密码或确认密码不能为空";
                }
                if (pass.Length < 6)
                {
                    return "密码长度至少6位";
                }
                if (pass != repass)
                {
                    return "两次设定的密码不一致";
                }
                if (string.IsNullOrEmpty(email))
                {
                    return "请输入正确的邮箱地址";
                }
                if (string.IsNullOrEmpty(code))
                {
                    return "验证参数错误";
                }
                if (!ValidateCodeHelper.Validate(code, vercode))
                {
                    return "验证答案有误";
                }
                return "";

            }
        }
        public string email { get; set; }
        public string username { get; set; }
        public string pass { get; set; }
        public string repass { get; set; }
        public string vercode { get; set; }
        public string code { get; set; }
    }
}
