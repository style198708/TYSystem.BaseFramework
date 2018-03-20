using TYSystem.BaseFramework.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TYSystem.BaseFramework.Common.Route
{
    /// <summary>
    /// 路由转发配置
    /// </summary>
    public class RouteLoadConfig
    {
        static private XmlDocument _xmlDoc, _xmlDocAdmin;
        public static Dictionary<string, List<string>> RouteConfigList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> RouteConfigLis_Admin = new Dictionary<string, List<string>>();
        static RouteLoadConfig()
        {
            Init();
            Init_Admin();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public static void Init()
        {
            if (_xmlDoc == null || String.IsNullOrEmpty(_xmlDoc.InnerText.Trim()))
            {
                try
                {
                    _xmlDoc = new XmlDocument();
                    string fileHandlerPath = Path.Combine(FileHandler.GetConfigPath, "RouteConfig/LoadConfig.xml");
                    _xmlDoc.Load(fileHandlerPath);

                    foreach (XmlNode itemNode in _xmlDoc.DocumentElement.ChildNodes)
                    {
                        List<string> valuesList = new List<string>();
                        foreach (XmlNode itemValuesList in itemNode.ChildNodes)
                        {
                            valuesList.Add(itemValuesList.InnerText);
                        }
                        RouteConfigList.Add(itemNode.Name, valuesList);
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception("路由文件处理配置文件出错：" + ex.Message);
                }
            }
        }

        public static void Init_Admin()
        {
            if (_xmlDocAdmin == null || String.IsNullOrEmpty(_xmlDocAdmin.InnerText.Trim()))
            {
                try
                {
                    _xmlDocAdmin = new XmlDocument();
                    string fileHandlerPath = Path.Combine(FileHandler.GetConfigPath, "RouteConfig/LoadConfig_Admin.xml");
                    _xmlDocAdmin.Load(fileHandlerPath);

                    foreach (XmlNode itemNode in _xmlDocAdmin.DocumentElement.ChildNodes)
                    {
                        List<string> valuesList = new List<string>();
                        foreach (XmlNode itemValuesList in itemNode.ChildNodes)
                        {
                            valuesList.Add(itemValuesList.InnerText);
                        }
                        RouteConfigLis_Admin.Add(itemNode.Name, valuesList);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Boss系统：文件处理配置文件出错：" + ex.Message);
                }
            }
        }
    }
}
