using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;


namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// 类全部以XDocument的扩展
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// XDocument转成XmlDocument
        /// </summary>
        /// <param name="xDocument"></param>
        /// <returns></returns>
        public XmlDocument ToXmlDocument(XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// XmlDocument转成XDocument
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public XDocument ToXDocument(XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Xml字符串转成XmlDocument
        /// </summary>
        /// <param name="XmlStr">Xml字符串</param>
        /// <returns></returns>
        public XmlDocument GetXmlDocument(string XmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlStr);
            return doc;
        }

        /// <summary>
        /// Xml字符串转成XDocument
        /// </summary>
        /// <param name="XmlStr">Xml</param>
        /// <param name="IsHead">是否显示xmls</param>
        /// <returns></returns>
        public XDocument GetXDocument(string XmlStr, bool IsHead = false)
        {
            XDocument doc = ToXDocument(GetXmlDocument(XmlStr));
            if (!IsHead)
            {
                if (doc.Elements().Count() > 0)
                {
                    doc.Elements().First().Attributes().Remove();
                }
            }
            return doc;
        }

        /// <summary>
        /// 实体转化成XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string EnXMLSerialize<T>(T entity) where T : class
        {
            return XDocSerialize<T>(entity).ToString().Replace("\r\n", "").Replace(" ", "");
        }

        /// <summary>
        /// 实体转化为XDocument
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public XDocument XDocSerialize<T>(T entity) where T : class
        {
            StringBuilder buffer = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StringWriter(buffer))
            {
                serializer.Serialize(writer, entity);
            }
            return GetXDocument(buffer.ToString());
        }

        /// <summary>
        /// XML字符串转化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public T DeXMLSerialize<T>(string xmlString) where T : class
        {
            T cloneObject = default(T);
            StringBuilder buffer = new StringBuilder();
            buffer.Append(xmlString);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(buffer.ToString()))
            {
                Object obj = serializer.Deserialize(reader);
                cloneObject = (T)obj;
            }
            return cloneObject;
        }

        /// <summary>
        /// XDocument 转成xml字符串
        /// </summary>
        /// <param name="xdoc"></param>
        /// <returns></returns>
        public StringBuilder XDocumentToString(XDocument xdoc)
        {
            StringBuilder bulider = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(bulider))
            {
                xdoc.WriteTo(writer);
            }
            return bulider;
        }

        /// <summary>     
        /// XElement转换为XmlElement     
        /// </summary>     
        public XmlElement ToXmlElement(XElement xElement)
        {
            if (xElement == null) return null;
            XmlElement xmlElement = null;
            XmlReader xmlReader = null;
            try
            {
                xmlReader = xElement.CreateReader();
                var doc = new XmlDocument();
                xmlElement = doc.ReadNode(xElement.CreateReader()) as XmlElement;
            }
            catch
            {
            }
            finally
            {
                if (xmlReader != null) xmlReader.Close();
            }

            return xmlElement;
        }

        /// <summary>     
        /// XmlElement转换为XElement     
        /// </summary>     
        public XElement ToXElement(XmlElement xmlElement)
        {
            if (xmlElement == null) return null;

            XElement xElement = null;
            try
            {
                var doc = new XmlDocument();
                doc.AppendChild(doc.ImportNode(xmlElement, true));
                xElement = XElement.Parse(doc.InnerXml);
            }
            catch { }

            return xElement;
        }

        public XElement GetXElement(XDocument doc, List<string> Path)
        {
            FommatPath = Path;
            XElement Root = doc.Element(Path.First());
            Path.RemoveAt(0);
            Peek(Root);
            return Element;
        }

        private List<string> FommatPath { get; set; }

        private XElement Element { get; set; }

        public void Peek(XElement doc, int index = 0)
        {
            XElement element = doc.Element(FommatPath[index]);
            index++;
            if (element.Elements().Count() > 0 && index < FommatPath.Count())
            {
                Peek(element, index);
            }
            else
            {
                Element = element;
            }
        }
    }
}
