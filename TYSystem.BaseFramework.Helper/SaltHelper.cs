using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TYSystem.BaseFramework.Helper
{
    public class SaltHelper
    {
        /// <summary>
        /// 创建一个指定长度的随机salt值
        /// </summary>
        public static string CreateSalt(int saltLenght)
        {
            //生成一个加密的随机数
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[saltLenght];
            rng.GetBytes(buff);
            //返回一个Base64随机数的字符串
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// 返回加密后的字符串
        /// </summary>
        public static string CreatePasswordHash(int saltLenght)
        {
            string strSalt = CreateSalt(saltLenght);
            //把密码和Salt连起来
            string saltAndPwd = String.Concat(232, strSalt);
            //对密码进行哈希
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPwd));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;



            //string hashenPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            ////转为小写字符并截取前16个字符串
            //hashenPwd = hashenPwd.ToLower().Substring(0, 16);
            ////返回哈希后的值
            //return hashenPwd;
        }
    }
}
