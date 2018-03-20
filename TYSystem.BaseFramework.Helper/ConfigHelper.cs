using System;
using System.Xml;
using TYSystem.BaseFramework.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// ConfigPath帮助类
    /// 主要是为了实现在File下面的配置路径并生成XML实体
    /// </summary>
    public class ConfigHelper
    {

        #region  System.Configuration.ConfigurationManager 读取配置文件

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="key">健值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetValue(string key, string defaultValue)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[key];
            if (value.IsNullOrEmpty())
                value = defaultValue;
            return value;
        }

        /// <summary>
        /// 获取数据库配置文件路径 
        /// </summary>
        public static string GetTableConfig
        {
            get
            {
                //return GetValue("TableConfig", AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                return GetValue("TableConfig", AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        /// <summary>
        /// 获取项目文路径
        /// </summary>
        //public static string GetConfigPath
        //{
        //    get
        //    {
        //        //AppDomain.CurrentDomain.SetupInformation.ApplicationBase
        //        return GetValue("ConfigPath", AppDomain.CurrentDomain.BaseDirectory);
        //    }
        //}

        #endregion Microsoft.Extensions.Configuration 读取配置文件

        public static string GetConfigPath
        {
            get
            {
                ConfigurationBuilder bulider = new ConfigurationBuilder();
                bulider.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                IConfigurationRoot config = bulider.Build();
                return config.GetSection("ConfigPath").Value;

               
            }

        }


        #region 

        #endregion


        #region 读取Config目录下的文件

        public static XmlDocument GetXmlDoc(string configName)
        {
            try
            {
                string configPath = GetConfigPath + configName + ".xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);
                return xmlDoc;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("初始化配置文件{0}出错：" + ex.Message + ",StackTrace=" + ex.StackTrace, configName + ".xml"));
            }

        }

        public static string SelectSingleNode(string configName, string singleNode)
        {
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc == null)
            {
                return "";
            }
            string value = "";
            try
            {
                value = xmlDoc.SelectSingleNode("//" + configName + "ConfigInfo/" + singleNode).InnerText;
            }
            catch
            {
            }
            return value;
        }
        public static XmlNodeList SelectNodeList(string configName, string singleNode)
        {
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc != null)
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//" + configName + "ConfigInfo/" + singleNode);
                return xmlNodeList;
            }
            return null;
        }
        public static XmlNode SelectSingleAttrNode(string configName, string singleNode, string attrWhere, string attrWhereValue)
        {
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc != null)
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//" + configName + "ConfigInfo/" + singleNode + "[@" + attrWhere + "='" + attrWhereValue + "']");
                if (xmlNodeList.Count < 1)
                {
                    throw new Exception("文件处理配置文件不存在" + attrWhere);
                }
                return xmlNodeList[0];
            }
            return null;
        }
        public static string SelectSingleAttrNode(string configName, string singleNode, string attrWhere, string attrWhereValue, string attrResult)
        {
            string directoryName = string.Empty;
            XmlDocument xmlDoc = GetXmlDoc(configName);
            if (xmlDoc != null)
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//" + configName + "ConfigInfo/" + singleNode + "[@" + attrWhere + "='" + attrWhereValue + "']");
                if (xmlNodeList.Count < 1)
                {
                    throw new Exception("文件处理配置文件不存在" + attrWhere);
                }

                directoryName = xmlNodeList[0].Attributes[attrResult].Value;
            }
            return directoryName;
        }
        #endregion
    }

}
