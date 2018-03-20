using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.Common.Helper
{
    /// <summary>
    /// 随机生成帮助类
    /// </summary>
    public class RandomHelper
    {
        private static IdWorker worker = new IdWorker(1, 1);

        object obj = new object();
        IList<string> strList = new List<string>();

        public static string GetRandomID()
        {
            long id = worker.NextId();
            return id.ToString();
        }
        #region 数字随机数
        /// <summary>  
        /// 数字随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandNum(int n)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();

            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());
            }

            return num.ToString();
        }
        #endregion
        #region 数字和字母随机数
        /// <summary> 
        /// 数字和字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandCode(int n)
        {
            //lock (obj)  //防止同一时间生成同样的数字
            //{
            //    Random r = new Random();
            //    int n = r.Next(0, count);
            //    int resultnum = num[n]; //用随机数做集合下标取集合中数字
            //    num.RemoveAt(n);  //去除已经获取了的数字,之后就不会重复了
            //    count--;
            //    return resultnum;
            //}




            char[] arrChar = new char[]{ 
            'a','b','d','c','e','f','g','h','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
            '0','1','2','3','4','5','6','7','8','9', 
            };
            //char[] arrChar = new char[]{ 
            //'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
            //'0','1','2','3','4','5','6','7','8','9', 
            //'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
            //}; 

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

            }

            return num.ToString();
        }


        public string RandCode1(int n)
        {
            lock (obj)  //防止同一时间生成同样的数字
            {
                object objIPStrategy = null;
                List<string> list = new List<string>();

                if (objIPStrategy != null)
                {
                    list = (List<string>)objIPStrategy;
                }


                char[] arrChar = new char[]{ 
            'a','b','d','c','e','f','g','h','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
            '0','1','2','3','4','5','6','7','8','9', 
            };
                //char[] arrChar = new char[]{ 
                //'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
                //'0','1','2','3','4','5','6','7','8','9', 
                //'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
                //}; 

                StringBuilder num = new StringBuilder();

                bool s = true;
                while (s)
                {
                    Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
                    for (int i = 0; i < n; i++)
                    {
                        num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

                    }

                    s = true;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == num.ToString())
                        {
                            num = new StringBuilder();
                            s = false;
                            break;
                        }
                    }

                    if (s == false)
                        s = true;
                    else
                        s = false;
                }


                list.Add(num.ToString());
                return num.ToString();
            }







        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string RandCodeAll(int n)
        {
            char[] arrChar = new char[]{ 
            'a','b','d','c','e','f','g','h','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x','z',
            '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
            };
            //char[] arrChar = new char[]{ 
            //'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
            //'0','1','2','3','4','5','6','7','8','9', 

            //}; 

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

            }

            return num.ToString();
        }

        #endregion
        #region 字母随机数
        /// <summary> 
        /// 字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandLetter(int n)
        {
            char[] arrChar = new char[]{ 
            'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
            //'_', 
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
            };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

            }

            return num.ToString();
        }
        #endregion
        #region 字母随机数,验证码,去除大小写的O
        /// <summary> 
        /// 字母随机数,验证码,去除大小写的O 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandCheckCode(int n)
        {
            char[] arrChar = new char[]{ 
            'a','b','d','c','e','f','g','h','i','j','k','m','n','p','r','q','s','t','u','v','w','z','y','x', 
            //'_', 
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
            };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 日期随机函数
        /// <summary> 
        /// 日期随机函数 
        /// </summary> 
        /// <param name="ra">长度</param> 
        /// <returns></returns> 
        public static string DateRndName(Random ra)
        {
            DateTime now = DateTime.Now;
            return now.ToString("yyMMddHHmmss") + ra.Next(100, 999).ToString();
        }
        #endregion
        #region 生成GUID
        /// <summary> 
        /// 生成GUID 
        /// </summary> 
        /// <returns></returns> 
        public static string GetGuid()
        {
            System.Guid g = System.Guid.NewGuid();
            return g.ToString();
        }
        #endregion

        #region 生成TicketId
        public static string RandTicketId(string DateFmt, int n)
        {
            return DateTime.Now.ToString(DateFmt) + RandNum(n);
        }
        public static string RandTicketId(int n)
        {
            return RandTicketId("yyyyMMddHHmmssfff", n);
        }
        #endregion
    }
}
