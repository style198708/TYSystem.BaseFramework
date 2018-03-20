using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Drawing;
using System.Xml.Linq;
using System.Diagnostics;
using TYSystem.BaseFramework.Extension;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    public class FileHandler
    {
        private static XmlDocument xmlDoc;

        static FileHandler()
        {
            if (xmlDoc == null)
            {
                xmlDoc = FileHandlerXmlDoc();
            }
        }

        public static XmlDocument FileHandlerXmlDoc()
        {
            try
            {
                xmlDoc = new XmlDocument();
                string fileHandlerPath = Path.Combine(ConfigHelper.GetConfigPath, "BaseConfig.xml");
                xmlDoc.Load(fileHandlerPath);
            }
            catch (Exception ex)
            {
                xmlDoc = null;
                throw new Exception("初始化配置文件BaseConfig.xml错误:" + ex.ToString());
            }
            return xmlDoc;
        }
        public static string SelectSingleNode(string singleNode)
        {
            string value = "";
            try
            {
                value = xmlDoc.SelectSingleNode("//FileHandler/" + singleNode).InnerText;
            }
            catch
            {
            }
            return value;
        }

        /// <summary>
        /// 取配置文件的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetConfigPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    if (SelectSingleNode("IsConfigPath") == "1")
                        return SelectSingleNode("ConfigPath");
                    else
                        return AppDomain.CurrentDomain.BaseDirectory; //AppDomain.CurrentDomain.SetupInformation.ApplicationBase
                }
                else
                    return "";
            }

        }

        #region 项目磁盘路径
        /// <summary>
        /// 取后台项目的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetFramePath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    if (SelectSingleNode("IsConfigPath") == "1")
                        return SelectSingleNode("FramePath");
                    else
                        return AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// 取前台项目BASE的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetBasePath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    if (SelectSingleNode("IsConfigPath") == "1")
                        return SelectSingleNode("BasePath");
                    else
                        return AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// 取前台项目IMAGE的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetImagePath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("ImagePath");
                }
                return "";
            }
        }

        /// <summary>
        /// 取前台项目Files的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetFilePath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("FilePath");
                }
                return "";
            }
        }

        /// <summary>
        /// 取前台项目MEMBER的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetMemberPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("MemberPath");
                }
                return "";
            }
        }

        /// <summary>
        /// 取前台项目CART的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetCartPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("CartPath");
                }
                return "";
            }
        }

        /// <summary>
        /// 取前台项目WWW的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetPortalPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    if (SelectSingleNode("IsConfigPath") == "1")
                        return SelectSingleNode("PortalPath");
                    else
                        return AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// 取前台项目PASSPORT的磁盘位置信息
        /// </summary>
        /// <returns></returns>
        public static string GetPassportPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("PassportPath");
                }
                return "";
            }
        }

        /// <summary>
        /// 专题事件路径
        /// </summary>
        /// <returns></returns>
        public static string GetEventPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("EventPath");
                }
                return "";
            }
        }
        #endregion

        #region ESConfig
        /// <summary>
        /// 获取ESIP
        /// </summary>
        /// <returns></returns>
        public static string GetESHostNamePath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("ESHostNamePath");
                }
                return "";
            }
        }

        /// <summary>
        /// 获取ES端口
        /// </summary>
        /// <returns></returns>
        public static int GetESPortPath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("ESPortPath").ToInt(9200);
                }
                return 9200;
            }
        }

        /// <summary>
        /// 获取ES超时时间
        /// </summary>
        /// <returns></returns>
        public static int GetESTimeOut
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("ESTimeOut").ToInt(60000);
                }
                return 60000;
            }
        }

        /// <summary>
        /// 获取ES日志
        /// </summary>
        /// <returns></returns>
        public static string GetESLogFilePath
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("ESLogFilePath");
                }
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 取当前项目的版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetVersion
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("Version");
                }
                return "v1.0";
            }
        }

        /// <summary>
        /// 取当前项目的JS版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetJsVersion
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("JsVersion");
                }
                return "20161121";
            }
        }

        /// <summary>
        /// 获取登录密钥
        /// </summary>
        /// <returns></returns>
        public static string GetLoginSingKey
        {
            get
            {
                if (xmlDoc == null)
                    xmlDoc = FileHandlerXmlDoc();

                if (xmlDoc != null)
                {
                    return SelectSingleNode("LoginSingKey");
                }
                return "127.0.0.1";
            }
        }











    }
}
