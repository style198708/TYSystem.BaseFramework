//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TYSystem.BaseFramework.Serializer
{
    /// <summary>
    /// JSON序列化处理方法类
    /// </summary>
    public class SerializerJson
    {
        /// <summary>
        /// 序列化成字符串
        /// </summary>
        /// <param name="obj">除了DataTable,Color等类型不能序列化之外,基本其它都能序列化,未亲自测试</param>
        /// <returns></returns>
        public static string JsonSerializeObject<T>(T obj) where T : class
        {
            DataContractJsonSerializer JsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream msObj = new MemoryStream();
            //将序列化之后的Json格式数据写入流中
            JsonSerializer.WriteObject(msObj, obj);
            msObj.Position = 0;
            //从0这个位置开始读取流中的数据
            StreamReader sr = new StreamReader(msObj, Encoding.UTF8);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();
            return json;
        }
      

        /// <summary>
        /// 反序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            DataContractJsonSerializer JsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(value)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
                T model = (T)deseralizer.ReadObject(ms);// //反序列化ReadObject
                return model;
            }
        }
      
        #region byte序列化
        /// <summary>
        /// 序列化成字节流
        /// </summary>
        /// <typeparam name="T">泛型实体</typeparam>
        /// <param name="entity">具体实体</param>
        /// <returns>byte字节流</returns>
        private byte[] SerializeObject<T>(T entity)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, entity);
                byte[] bytes = ms.ToArray();
                return bytes;
            }
        }

        /// <summary>
        /// 反序列化成实体
        /// </summary>
        /// <typeparam name="T">具体实体</typeparam>
        /// <param name="data">byte字节流</param>
        /// <returns>返回一个实体</returns>
        private T DeserializeObject<T>(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            T entity = DeserializeObject<T>(json);
            return entity;
        }
        #endregion

    }
}
