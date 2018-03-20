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
    /// �ļ�������
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
                throw new Exception("��ʼ�������ļ�BaseConfig.xml����:" + ex.ToString());
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
        /// ȡ�����ļ��Ĵ���λ����Ϣ
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

        #region ��Ŀ����·��
        /// <summary>
        /// ȡ��̨��Ŀ�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿBASE�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿIMAGE�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿFiles�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿMEMBER�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿCART�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿWWW�Ĵ���λ����Ϣ
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
        /// ȡǰ̨��ĿPASSPORT�Ĵ���λ����Ϣ
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
        /// ר���¼�·��
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
        /// ��ȡESIP
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
        /// ��ȡES�˿�
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
        /// ��ȡES��ʱʱ��
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
        /// ��ȡES��־
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
        /// ȡ��ǰ��Ŀ�İ汾��Ϣ
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
        /// ȡ��ǰ��Ŀ��JS�汾��Ϣ
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
        /// ��ȡ��¼��Կ
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
