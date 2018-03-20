using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// SERVER获取IP的帮助类
    /// 获取客户端IP,服务器端IP
    /// </summary>
    public class IpHelper
    {
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns></returns>
        protected static bool IsNumeric(string str)
        {
            if (str != null && System.Text.RegularExpressions.Regex.IsMatch(str, @"^-?\d+$"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 从IP转换为Int32
        /// </summary>
        /// <param name="IpValue"></param>
        /// <returns></returns>
        public static UInt32 IPToUInt32(string IpValue)
        {
            string[] IpByte = IpValue.Split('.');
            Int32 nUpperBound = IpByte.GetUpperBound(0);
            if (nUpperBound != 3)
            {
                IpByte = new string[4];
                for (Int32 i = 1; i <= 3 - nUpperBound; i++)
                    IpByte[nUpperBound + i] = "0";
            }

            byte[] TempByte4 = new byte[4];
            for (Int32 i = 0; i <= 3; i++)
            {
                if (IsNumeric(IpByte[i]))
                    TempByte4[3 - i] = (byte)(Convert.ToInt32(IpByte[i]) & 0xff);
            }

            return BitConverter.ToUInt32(TempByte4, 0);
        }


        /// <summary>
        /// 是否是测试环境IP
        /// </summary>
        /// <returns></returns>
        public static bool IsLocalIP()
        {
            string serverIP = GetServerIP();
            if (serverIP == "127.0.0.1" || serverIP == "192.168.2.199" || serverIP == "172.168.18.106") return true;
            return false;
        }

        /// <summary>
        /// 获取服务器端的IP
        /// </summary>
        public static string GetServerIP()
        {
            if (Common.Compatible.HttpContext.Current != null)
            {
                string result = Common.Compatible.HttpContext.Current.Request.Headers["Local_Addr"].FirstOrDefault();
                if (string.IsNullOrEmpty(result))
                    return string.Empty;
                return result;
            }
            try
            {
                return GetServerDnsIP();
            }
            catch
            {

            }
            return string.Empty;
        }

        /// <summary>
        /// 获取DNSIP
        /// </summary>
        /// <returns></returns>
        public static string GetServerDnsIP()
        {
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
       
        /// <summary>
        /// 获取web客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIp()
        {
            string userIP = "0.0.0.0";
            try
            {
                if (Common.Compatible.HttpContext.Current == null
            || Common.Compatible.HttpContext.Current.Request == null
            || Common.Compatible.HttpContext.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault() == null)
                    return userIP;
                string CustomerIP = "";
                //CDN加速后取到的IP simone 090805
                CustomerIP = Common.Compatible.HttpContext.Current.Request.Headers["Cdn-Src-Ip"].FirstOrDefault();
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }
                CustomerIP = Common.Compatible.HttpContext.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!String.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;
                if (Common.Compatible.HttpContext.Current.Request.Headers["HTTP_VIA"].FirstOrDefault() != null)
                {
                    CustomerIP = Common.Compatible.HttpContext.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (CustomerIP == null)
                        CustomerIP = Common.Compatible.HttpContext.Current.Request.Headers["REMOTE_ADDR"].FirstOrDefault();
                }
                else
                {
                    CustomerIP = Common.Compatible.HttpContext.Current.Request.Headers["REMOTE_ADDR"].FirstOrDefault();
                }


                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    //return Common.Compatible.HttpContext.Current.Request.UserHostAddress;
                    return "0.0.0.0";
                return CustomerIP;
            }
            catch
            {
                userIP = "0.0.0.0";
            }
            return userIP;
        }

    }

}
