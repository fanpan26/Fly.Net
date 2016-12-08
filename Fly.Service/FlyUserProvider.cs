using Fly.IService;
using Fly.Model;
using Fly.Util;
using Fly.Util.Cookie;
using Fly.Util.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Service
{
    public class FlyUserProvider : IFlyUserProvider
    {
        public int GetUserId()
        {
            string token = CookieHelper.ReadCookie(FlyConst.FLY_USER_TOKEN_KEY);
            User user = TokenHelper.DecodeToken<User>(token);
            if (user != null && user.userid > 0)
            {
                return user.userid;
            }
            return 0;
        }
    }
}
