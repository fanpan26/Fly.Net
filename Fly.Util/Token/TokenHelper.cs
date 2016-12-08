using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Util.Token
{
    public class TokenHelper
    {
        static string SecretKey = "Fly.XXXPZ";
        /// <summary>
        /// 生成token 根据用户名信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CreateToken(object obj)
        {
            string token = JWT.JsonWebToken.Encode(obj, SecretKey, JWT.JwtHashAlgorithm.HS384);
            return token;
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string DecodeToken(string token)
        {
            if (string.IsNullOrEmpty(token)) { return ""; }
            string obj = JWT.JsonWebToken.Decode(token, SecretKey);
            return obj;
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T DecodeToken<T>(string token)
        {
            string obj = DecodeToken(token);
            return JWT.JsonWebToken.JsonSerializer.Deserialize<T>(obj);
        }
    }
}
