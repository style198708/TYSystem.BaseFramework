using System;
using System.IO;
using System.Net;
using TYSystem.BaseFramework.Common;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// 功能：Web帮助类
    /// 作者：dylan
    /// </summary>
    public class WebHelper
    {
        #region SendWebRequest

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="url">请求的页面地址</param>
        public static string SendWebRequest(string url)
        {
            return SendWebRequest(url, null);
        }

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="url">请求的页面地址</param>
        /// <param name="proxy">访问代理</param>
        public static string SendWebRequest(string url, WebProxy proxy)
        {
            // 初始化WebRequest
            WebRequest request = WebRequest.Create(url);
            // 设置访问代理
            if (proxy != null)
            {
                request.Proxy = proxy;
            }

            string result = string.Empty;
            try
            {
                // 对接收到的页面内容进行处理
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd().Trim();

                // 关闭对象，释放资源
                reader.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                result = "发送Web请求时出现网络错误。请检查是否设置了访问代理。Url=" + url + " ERROR=" + ex.ToString() + "/r/n";
                result += new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                result = "发送Web请求时出现异常错误。请检查是否设置了访问代理。 ERROR=" + ex.ToString();
            }

            return result;
        }

        #endregion

        #region SendHttpRequest

        /// <summary>
        /// 发送Web请求
        /// </summary>
        /// <param name="url">请求的页面地址</param>
        /// <param name="httpStatusCode">返回的状态码</param>
        public static string SendGetRequest(string url, out int httpStatusCode)
        {
            return SendRequest(url, null, "get", out httpStatusCode);
        }

        /// <summary>
        /// 公共请求方法
        /// </summary>
        /// <param name="url">要请求的url地址</param>
        /// <param name="reqdata">请求参数,id=xx&key=xx</param>
        /// <param name="method">GET,POST</param>
        /// <param name="httpStatusCode">返回的状态码</param>
        /// <param name="proxy">代理,默认为null</param>
        /// <returns></returns>
        public static string SendRequest(string url, string reqdata, string method, out int httpStatusCode, IWebProxy proxy = null)
        {
            string html = "";
            httpStatusCode = 0;
            try
            {
                HttpWebRequest request = null;
                if (url != "" && method != "")
                {
                    //请求
                    if (method.ToLower() == "get")
                    {
                        request = (HttpWebRequest)WebRequest.Create(new Uri(url + (string.IsNullOrEmpty(reqdata) ? "" : "?" + reqdata)));
                        request.Method = method;
                        request.ContentType = "text/html;charset=UTF-8";
                        //Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    }
                    else
                    {
                        request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                        request.Method = method;
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                        reqdata = reqdata ?? "";
                        byte[] buf = System.Text.Encoding.GetEncoding("utf-8").GetBytes(reqdata);
                        using (System.IO.Stream s = request.GetRequestStream())
                        {
                            s.Write(buf, 0, buf.Length);
                            s.Close();
                        }
                    }

                    // 设置访问代理
                    if (proxy != null)
                    {
                        request.Proxy = proxy;
                    }
                    //request.Timeout = 1000 * 60 * 30;//超时时间是30分钟
                    //返回响应
                    HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                    httpStatusCode = (int)res.StatusCode;
                    StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                    html = sr.ReadToEnd();
                    sr.Close();

                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        httpStatusCode = (int)response.StatusCode;
                    }
                    else
                    {
                        // no http status code available
                    }
                }
                else
                {
                    // no http status code available
                }
                html = ex.ToString();
            }
            catch (Exception ex)
            {
                html = ex.ToString();
            }

            return html;
        }

        #endregion

        #region GetClientIP

        /// <summary>
        /// 得到客户端IP地址
        /// </summary>
        /// <returns>IP</returns>
        public static string GetClientIP()
        {
            try
            {
                var Request =Compatible.HttpContext.Current.Request;
                // 如果使用CDN，获取真实IP
                if (!String.IsNullOrEmpty(Request.Headers["HTTP_Cdn_Src_Ip"]))
                {
                    return Request.Headers["HTTP_Cdn_Src_Ip"].ToString();
                }
                // 获取最外层代理IP地址
                if (!String.IsNullOrEmpty(Request.Headers["HTTP_X_FORWARDED_FOR"]))
                {
                    return Request.Headers["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0];
                }
                // 获取远程主机IP地址
                if (!String.IsNullOrEmpty(Request.Headers["REMOTE_ADDR"]))
                {
                    return Request.Headers["REMOTE_ADDR"].ToString();
                }
                // 获取本地主机地址
                if (!String.IsNullOrEmpty(Request.HttpContext.Connection.RemoteIpAddress.ToString()))
                {
                    return Request.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return "000.000.000.000";
            }
            catch
            {
                return "000.000.000.000";
            }
        }

        #endregion
      
    }
}
