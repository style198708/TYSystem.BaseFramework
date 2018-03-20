using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using TYSystem.BaseFramework.Common;

namespace TYSystem.BaseFramework.Helper
{
    public sealed class HttpHelper
    {
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SendRequestData(string url, string data)
        {
            string result = "";

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data); //转化

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;

                Stream stream = request.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length); //写入参数
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SendRequestData(string url, Dictionary<string, string> list)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection postVars = new System.Collections.Specialized.NameValueCollection();
           
            foreach (var item in list)
            {
                postVars.Add(item.Key, item.Value);
            }
            try
            {
                byte[] bytes = wc.UploadValues(url, "POST", postVars);
                
                return  Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            if (Compatible.HttpContext.Current.Request == null)
            {
                return "";
            }

            try
            {
                string value = Compatible.HttpContext.Current.Request.Query[key];
                if (string.IsNullOrEmpty(value))
                {
                    return "";
                }
                return value;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取参数（输入流方式）
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetParams()
        {
            try
            {
                //byte[] byts = new byte[HttpContent.Current.Request.InputStream.Length];
                var byts = new byte[Convert.ToInt32(Compatible.HttpContext.Current.Request.ContentLength)];
                Compatible.HttpContext.Current.Request.Body.ReadAsync(byts, 0, byts.Length);

                string value = System.Text.Encoding.UTF8.GetString(byts);
                value = System.Net.WebUtility.UrlDecode(value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                Dictionary<string, string> dic = new Dictionary<string, string>();

                string[] arr = value.Split('&');
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] p = arr[i].Split('=');
                    dic.Add(p[0], p[1]);
                }
                return dic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 物理文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //public static string MapPath(string path)
        //{
        //    try
        //    {
        //        return HttpContext.Current.Server.MapPath(path);
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        /// <summary>
        /// 请求响应时SSL证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="method_name"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetHttpWebResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 120000;
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null) response.Close();
            }
        }

        public static string GetResponse(string url)
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentType = "application/json";
            myReq.Timeout = 120000;
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            Stream myStream = HttpWResp.GetResponseStream();
            StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
            while (-1 != sr.Peek())
            {
                string result = sr.ReadLine();
                return result;
            }
            return null;
        }
        public static T GetResponse<T>(string url)
         where T : class, new()
        {

            T result = default(T);
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            Stream myStream = HttpWResp.GetResponseStream();
            StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
            while (-1 != sr.Peek())
            {
                result = Serializer.SerializerJson.DeserializeObject<T>(sr.ReadLine());
                //Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sr.ReadLine());
            }

            return result;
        }

        /// <summary>
        /// 微博post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public static string PostWboRepsonse(string url, string postData, string appkey)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded;";

            //request.Headers.Add(string.Format("Authorization: key={0}", appkey));
            //string postData = "client_id=123456";
            //postData += ("&client_secret=123456");
            //postData += ("&grant_type=authorization_code");
            //postData += ("&redirect_uri=http://www.cfxixi.com");
            //postData += ("&code=123456");
            byte[] data = Encoding.UTF8.GetBytes(postData);
            //  request.ContentLength = byteArray.Length;
            request.Timeout = 10000;
            request.ContentLength = data.Length;
            //往服务器写入数据
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null) response.Close();
            }

        }

        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="method_name"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostJsonResponse(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 120000;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            //往服务器写入数据
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null) response.Close();
            }
        }

        /// <summary>
        /// 对字符串进行URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return HttpUtility.UrlEncode(str);
        } /// <summary>
          /// 对字符串进行URL编码
          /// </summary>
          /// <param name="str"></param>
          /// <returns></returns>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return HttpUtility.UrlDecode(str);

        }
    }
}
