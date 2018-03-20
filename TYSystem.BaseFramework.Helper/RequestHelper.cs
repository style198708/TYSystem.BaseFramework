using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using TYSystem.BaseFramework.Common;
using TYSystem.BaseFramework.Extension;
using TYSystem.BaseFramework.Serializer;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// WEB请求帮助类
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// 获取浏览器Browser，Id，Version
        /// </summary>
        /// <returns></returns>
        public static string GetBrowser()
        {
            //return HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Id + " " + HttpContext.Current.Request.Browser.Version;
            return Compatible.HttpContext.Current.Request.Headers[HeaderNames.UserAgent];
        }

        /// <summary>
        /// 判断浏览器是否为POST请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return Compatible.HttpContext.Current.Request.Method.Equals("POST");
        }
        /// <summary>
        /// 判断浏览器是否为GET请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return Compatible.HttpContext.Current.Request.Method.Equals("GET");
        }
        /// <summary>
        /// 获取服务器名称
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetServerString(string strName)
        {
            string result;
            if (Compatible.HttpContext.Current.Request.Headers[strName].FirstOrDefault() == null)
            {
                result = "";
            }
            else
            {
                result = Compatible.HttpContext.Current.Request.Headers[strName].FirstOrDefault();
            }
            return result;
        }
        /// <summary>
        /// 获取当前主机名和端口
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            var request = Compatible.HttpContext.Current.Request;
            string result;
            if (request != null && request.Host.Value.Contains(":"))
            {
                result = string.Format("{0}:{1}", request.Host.Host, request.Host.Port.ToString());
            }
            else
            {
                result = request.Host.Value;
            }
            return result;
        }
        /// <summary>
        /// 获取当前主机名
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            if (Compatible.HttpContext.Current != null && Compatible.HttpContext.Current.Request != null && Compatible.HttpContext.Current.Request.Host != null)
            {
                return Compatible.HttpContext.Current.Request.Host.Host;
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取当前地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            if (Compatible.HttpContext.Current != null && Compatible.HttpContext.Current.Request != null && Compatible.HttpContext.Current.Request.Host != null)
            {
                return Compatible.HttpContext.Current.Request.Scheme + ";//" + Compatible.HttpContext.Current.Request.Host + Compatible.HttpContext.Current.Request.PathBase + Compatible.HttpContext.Current.Request.Path + Compatible.HttpContext.Current.Request.QueryString;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取当前原始地址
        /// </summary>
        /// <returns></returns>
        public static string GetRawUrl()
        {
            if (Compatible.HttpContext.Current != null && Compatible.HttpContext.Current.Request != null && Compatible.HttpContext.Current.Request.Host != null)
            {
                return Compatible.HttpContext.Current.Request.Scheme + ";//" + Compatible.HttpContext.Current.Request.Host + Compatible.HttpContext.Current.Request.PathBase + Compatible.HttpContext.Current.Request.Path + Compatible.HttpContext.Current.Request.QueryString;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取浏览器上一次请求地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrlReferrer()
        {
            if (Compatible.HttpContext.Current != null && Compatible.HttpContext.Current.Request != null && Compatible.HttpContext.Current.Request.Headers[HeaderNames.Referer].FirstOrDefault() != null)
            {
                return Compatible.HttpContext.Current.Request.Headers[HeaderNames.Referer].FirstOrDefault();
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取参数错误信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string getResultInfo(object param)
        {
            if (Compatible.HttpContext.Current != null && Compatible.HttpContext.Current.Request != null && Compatible.HttpContext.Current.Request.Query["debug"] == "true")
            {
                if (param == null)
                {
                    return "param is null！";
                }
                else
                {
                    return SerializerJson.JsonSerializeObject(param);
                }
            }
            return string.Empty;
        }

        #region 获取参数
        /// <summary>
        /// 获取地址栏参数值
        /// </summary>
        /// <param name="strName">键值</param>
        /// <returns>对应数据</returns>
        public static string GetQueryString(string strName)
        {
            return RequestHelper.GetQueryString(strName, false);
        }
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            string result;
            if (Compatible.HttpContext.Current.Request.Query[strName].IsNullOrEmpty())
            {
                result = "";
            }
            else
            {
                if (sqlSafeCheck && !ValidateHelper.IsSafeSqlString(Compatible.HttpContext.Current.Request.Query[strName]))
                {
                    result = "";
                }
                else
                {
                    result = Compatible.HttpContext.Current.Request.Query[strName];
                }
            }
            return result;
        }

        /// <summary>
        /// 获取地址栏参数数量
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return Compatible.HttpContext.Current.Request.Form.Count() + Compatible.HttpContext.Current.Request.Query.Count;
        }

        /// <summary>
        /// 获取表单数据值
        /// </summary>
        /// <param name="strName">键值</param>
        /// <returns>对应数据</returns>
        public static string GetFormString(string strName)
        {
            return RequestHelper.GetFormString(strName, false);
        }
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {

            string result;
            if (Compatible.HttpContext.Current.Request.Form[strName].IsNullOrEmpty())
            {
                result = "";
            }
            else
            {
                if (sqlSafeCheck && !ValidateHelper.IsSafeSqlString(Compatible.HttpContext.Current.Request.Form[strName]))
                {
                    result = "";
                }
                else
                {
                    result = Compatible.HttpContext.Current.Request.Form[strName];
                }
            }
            return result;
        }

        /// <summary>
        /// 获取地址栏、表单对应数据
        /// </summary>
        /// <param name="strName">Key</param>
        /// <returns></returns>
        public static string GetString(string strName)
        {
            return RequestHelper.GetString(strName, false);
        }
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            string result;
            if ("".Equals(RequestHelper.GetQueryString(strName)))
            {
                result = RequestHelper.GetFormString(strName, sqlSafeCheck);
            }
            else
            {
                result = RequestHelper.GetQueryString(strName, sqlSafeCheck);
            }
            return result;
        }

        public static int GetQueryInt(string strName)
        {
            return Compatible.HttpContext.Current.Request.Query[strName].ToInt();
        }
        public static int GetQueryInt(string strName, int defValue)
        {
            return Compatible.HttpContext.Current.Request.Query[strName].ToInt(defValue);
        }
        public static int GetFormInt(string strName, int defValue)
        {
            return Compatible.HttpContext.Current.Request.Form[strName].ToInt(defValue);
        }
        public static int GetInt(string strName, int defValue)
        {
            int result;
            if (RequestHelper.GetQueryInt(strName, defValue) == defValue)
            {
                result = RequestHelper.GetFormInt(strName, defValue);
            }
            else
            {
                result = RequestHelper.GetQueryInt(strName, defValue);
            }
            return result;
        }
        public static float GetQueryFloat(string strName, float defValue)
        {
            return Compatible.HttpContext.Current.Request.Query[strName].ToFloat(defValue);
        }
        public static float GetFormFloat(string strName, float defValue)
        {
            return Compatible.HttpContext.Current.Request.Form[strName].ToFloat(defValue);
        }
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return Compatible.HttpContext.Current.Request.Query[strName].ToDecimal(defValue);
        }
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return Compatible.HttpContext.Current.Request.Form[strName].ToDecimal(defValue);
        }


        public static float GetFloat(string strName, float defValue)
        {
            float result;
            if (RequestHelper.GetQueryFloat(strName, defValue) == defValue)
            {
                result = RequestHelper.GetFormFloat(strName, defValue);
            }
            else
            {
                result = RequestHelper.GetQueryFloat(strName, defValue);
            }
            return result;
        }
        #endregion

        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    //_Response.AddHeader("Accept-Ranges", "bytes");
                    //_Response.HeaderEncoding = Encoding.UTF8;
                    //_Response.Buffer = false;
                    _Response.Headers.Add(HeaderNames.AcceptRanges, "bytes");
                    _Response.Headers.Add(HeaderNames.AcceptEncoding, Encoding.UTF8.ToString());

                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes     
                    //int sleep = 200; //每秒5次 即5*10K bytes每秒     
                    int sleep = (int)Math.Floor(1000 * (decimal)pack / _speed) + 1;
                    //if (_Request.Headers["Range"] != null)
                    if (!_Request.Headers[HeaderNames.Range].IsNullOrEmpty())
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers[HeaderNames.Range].FirstOrDefault().Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.Headers.Add(HeaderNames.ContentLength,(fileLength - startBytes).ToString());

                    if (startBytes != 0)
                    {
                        _Response.Headers.Add(HeaderNames.ContentRange, string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.Headers.Add(HeaderNames.Connection,"Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.Headers.Add(HeaderNames.ContentDisposition, "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((decimal)(fileLength - startBytes) / pack) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        //if (_Response.IsClientConnected)
                        if (Compatible.HttpContext.Current.RequestAborted.IsCancellationRequested ==false)
                        {
                            //_Response.BinaryWrite(br.ReadBytes(pack));
                            _Response.WriteAsync(br.ReadBytes(pack).ToString());
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="dir"></param>
        /// <param name="fileName"></param>
        public static void ResponseFile(string url, string _fullPath, string fileName)
        {
            //if (!System.IO.Directory.Exists(dir))
            //{
            //    System.IO.Directory.CreateDirectory(dir);
            //}

            // float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                Myrq.ContentType = "application/x-www-form-urlencoded";

                System.IO.Stream st = myrp.GetResponseStream();

                if (myrp.ContentEncoding.ToLower().Contains("gzip"))
                {
                    st = new GZipStream(st, CompressionMode.Decompress);
                }

                StreamReader sr = new StreamReader(st, Encoding.UTF8);
                System.IO.Stream so = new System.IO.FileStream(_fullPath, System.IO.FileMode.Create);
                StreamWriter sw = new StreamWriter(so, Encoding.UTF8);
                string text = sr.ReadToEnd();
                sw.Write(text);
                //long totalDownloadedByte = 0;
                //byte[] by = new byte[1024];
                //int osize = sr.Read(by, 0, (int)by.Length);
                //while (osize > 0)
                //{
                //    totalDownloadedByte = osize + totalDownloadedByte;
                //    so.Write(by, 0, osize);
                //    osize = st.Read(by, 0, (int)by.Length);
                //    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                //}
                sr.Close();
                sw.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static string GetHTMLPage(string url)
        {
            string resultstring = string.Empty;
            WebResponse result = null;
            try
            {
                WebRequest req = WebRequest.Create(url);
                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);
                resultstring = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            return resultstring;
        }
        public static bool GetHTMLPage(string url, ref string ip, ref string errMsg)
        {
            WebResponse result = null;

            try
            {
                WebRequest req = WebRequest.Create(url);
                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream);
                string responeText = sr.ReadToEnd();
                int i = responeText.IndexOf("[") + 1;
                string tempip = responeText.Substring(i, 15);
                ip = tempip.Replace("]", "").Replace(" ", "");
            }
            catch (Exception exp)
            {
                errMsg = exp.Message;
                return false;
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            return true;
        }


        /// <summary>
        /// 获取当前所有参数
        /// </summary>
        /// <returns></returns>
        public static string GetRequestParams()
        {
            if (Compatible.HttpContext.Current != null && Compatible.HttpContext.Current.Request != null && Compatible.HttpContext.Current.Request.QueryString != null)
            {
                string resultValue = string.Empty;
                IQueryCollection nameValueList = Compatible.HttpContext.Current.Request.Query;
                if (nameValueList != null && nameValueList.Count > 0)
                {

                    foreach (var item in nameValueList.Keys)
                    {
                        resultValue += "," + item + ":" + nameValueList[item];
                    }
                }
                string[] nameKeys = Compatible.HttpContext.Current.Request.Form.Keys.ToArray();
                if (nameKeys != null && nameKeys.Length > 0)
                {
                    foreach (var item in nameKeys)
                    {
                        resultValue += "," + item + ":" + Compatible.HttpContext.Current.Request.Form[item];
                    }
                }
                if (resultValue.Length > 0)
                {
                    return resultValue.Substring(1);
                }
            }
            return string.Empty;
        }
    }
}