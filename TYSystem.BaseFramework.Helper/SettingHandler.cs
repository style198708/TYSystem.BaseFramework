using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using TYSystem.BaseFramework.Helper;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingHandler
    {
        static private XmlDocument xmlDoc = null;
        /// <summary>
        /// 初始化xml配置
        /// </summary>
        /// <remarks>
        /// 相应的配置已加入缓存
        /// 如果配置文件有更改,需要先清除缓存
        /// </remarks>
        static SettingHandler()
        {
            if (xmlDoc == null)
            {
                xmlDoc = InitSettingHandler();
            }
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <returns></returns>
        private static XmlDocument InitSettingHandler()
        {
            try
            {
                xmlDoc = new XmlDocument();
                string fileHandlerPath = Path.Combine(ConfigHelper.GetConfigPath, "SettingHandler.xml");
                xmlDoc.Load(fileHandlerPath);
            }
            catch (Exception ex)
            {
                xmlDoc = null;
                throw new Exception("初始化配置文件SettingHandler出错：" + ex.Message + ",StackTrace=" + ex.StackTrace);
            }
            return xmlDoc;
        }


        /// <summary>
        /// 获取节点的InnerText
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static string GetSettingNodeStr(string str1, string str2)
        {
            if (xmlDoc == null)
            {
                xmlDoc = InitSettingHandler();
            }

            XmlNode node = GetSettingNode(str1, str2);
            if (node == null)
            {
                //Log.Logger.Error("GetSettingNodeStr：node=null-" + str1 + "--" + str2);
                return ""; 
            }
            return node.InnerText;
        }


        /// <summary>
        /// 获取当前网站域
        /// </summary>
        public static string CookieDomain
        {
            get
            {
                if (GetNode("CookieDomain") != null)
                {
                    return GetNode("CookieDomain").InnerText;
                }
                return string.Empty;
            }
        }



        /// <summary>
        /// 获取节点列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodes(string model)
        {
            if (xmlDoc == null)
            {
                xmlDoc = InitSettingHandler();
            }
            return xmlDoc.SelectSingleNode(string.Format("/SettingHandler/{0}", model)).ChildNodes;
        }

        #region 获取XmlNode
        /// <summary>
        /// 获取单个节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static XmlNode GetNode(string model)
        {
            if (xmlDoc == null)
            {
                xmlDoc = InitSettingHandler();
            }
            return xmlDoc.SelectSingleNode(string.Format("/SettingHandler/{0}", model));
        }

        /// <summary>
        /// 获取节点的InnerText
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static XmlNode GetSettingNode(string str1, string str2)
        {
            if (xmlDoc == null)
            {
                xmlDoc = InitSettingHandler();
            }
            return xmlDoc.SelectSingleNode(string.Format("//SettingHandler/{0}/{1}", str1, str2));
        }
        #endregion


    }
}
