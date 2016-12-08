using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Fly.Util.Encrypt
{

    /// <summary>    /// DES加密/解密类。    /// </summary>  public class EncryptHelper
    {
        public EncryptHelper()
        {
        }

        public static string StringToMD5(string str)
        {
            //获取要加密的字段，并转化为Byte[]数组  
            byte[] data = System.Text.Encoding.Unicode.GetBytes(str.ToCharArray());
            //建立加密服务  
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //加密Byte[]数组  
            byte[] result = md5.ComputeHash(data);

            return BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
        }

    }
}
