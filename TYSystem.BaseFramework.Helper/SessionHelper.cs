
using TYSystem.BaseFramework.Common;
using TYSystem.BaseFramework.Extension;
using TYSystem.BaseFramework.Serializer;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// 设置Session的帮助类
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 赋值Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(string key, string value)
        {
            string json =SerializerJson.JsonSerializeObject(value);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);
            Compatible.HttpContext.Current.Session.Set(key, serializedResult);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSessionStr(string key)
        {
            Compatible.HttpContext.Current.Session.TryGetValue(key, out byte[] value);
            return System.Text.Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetSessionInt(string key)
        {
            string key0 = GetSessionStr(key);
            return (GetSessionStr(key)).ToInt(0);
        }
    }
}
