using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Util.ValidateCode
{
    //生成随机参数
    public sealed class ValidateCodeHelper
    {
        private static Random r = new Random();
        /// <summary>
        /// Dictionary简单模拟验证码
        /// </summary>
        private static SortedDictionary<String, String> _validateCodes = new SortedDictionary<string, string> {
            { "1+1=？","2"},
            { "35+49=？","84"},
            { "200-100=？","100"},
            { "24*2=？","48"},
            { "12+3+4=","19"},
            { "你想回答正确吗？","想"},
            { "这个社区DEMO用什么语言写的？",".net"},
            { ".NET开源吗？（开or不开）","开"},
            { "中华人民共和国哪年成立的？","1949"},
        };

        public static string GetCode()
        {
            int result = r.Next(0, _validateCodes.Count - 1);
            return _validateCodes.ElementAt(result).Key;
        }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static bool Validate(string code, string answer)
        {
            return _validateCodes[code].Equals(answer);
        }
    }
}
