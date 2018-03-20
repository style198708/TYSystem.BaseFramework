using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// 复制实体帮助类
    /// </summary>
    public static class CopyHelper
    {
        /// <summary>
        /// 一般拷贝
        /// </summary>
        /// <typeparam name="T1">复制的实体,必须是类</typeparam>
        /// <typeparam name="T2">返回的实体</typeparam>
        /// <param name="entity">复制的实体</param>
        /// <param name="isNeedNullValue">是否复制空值</param>
        /// <returns></returns>
        public static T2 CopyTo<T1, T2>(this T1 entity, T2 targetEntity, bool isNeedNullValue) 
            where T1 : class
            where T2 : class
        {
            if (entity == null)
                return targetEntity;

            PropertyInfo[] entityProperties = entity.GetType().GetProperties();
            PropertyInfo[] targetEntityProperties = targetEntity.GetType().GetProperties();
            IDictionary<string, PropertyInfo> entityPropertiesNamePair = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo pInfo in entityProperties)
            {
                if (!pInfo.CanRead) continue;
                entityPropertiesNamePair[pInfo.Name.ToLower()] = pInfo;
            }
            var targetPInfos = from targetPropertyInfo in targetEntityProperties
                               where entityPropertiesNamePair.Keys.Contains(targetPropertyInfo.Name.ToLower())
                               select targetPropertyInfo;
            foreach (PropertyInfo targetPropertyInfo in targetPInfos)
            {
                if (!targetPropertyInfo.CanWrite)
                    continue;
                object entityPInfoValue = entityPropertiesNamePair[targetPropertyInfo.Name.ToLower()].GetValue(entity, null);
                if ((isNeedNullValue) || (entityPInfoValue != null))
                {
                    targetPropertyInfo.SetValue(targetEntity, entityPInfoValue, null);
                }
            }
            return targetEntity;
        }


        /// <summary>
        /// 一般拷贝
        /// </summary>
        /// <typeparam name="T1">复制的实体,必须是类</typeparam>
        /// <typeparam name="T2">返回的实体</typeparam>
        /// <param name="entity">复制的实体</param>
        /// <param name="isNeedNullValue">是否复制空值</param>
        /// <returns></returns>
        public static T2 CopyTo<T1, T2>(this T1 entity, bool isNeedNullValue)
            where T1 : class
            where T2 : class
        {
            if (entity == null)
                return default(T2);

            T2 targetEntity = Activator.CreateInstance<T2>();
            PropertyInfo[] entityProperties = entity.GetType().GetProperties();
            PropertyInfo[] targetEntityProperties = targetEntity.GetType().GetProperties();
            IDictionary<string, PropertyInfo> entityPropertiesNamePair = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo pInfo in entityProperties)
            {
                if (!pInfo.CanRead) continue;
                entityPropertiesNamePair[pInfo.Name.ToLower()] = pInfo;
            }
            var targetPInfos = from targetPropertyInfo in targetEntityProperties
                               where entityPropertiesNamePair.Keys.Contains(targetPropertyInfo.Name.ToLower())
                               select targetPropertyInfo;
            foreach (PropertyInfo targetPropertyInfo in targetPInfos)
            {
                if (!targetPropertyInfo.CanWrite)
                    continue;
                object entityPInfoValue = entityPropertiesNamePair[targetPropertyInfo.Name.ToLower()].GetValue(entity, null);
                if ((isNeedNullValue) || (entityPInfoValue != null))
                {
                    targetPropertyInfo.SetValue(targetEntity, entityPInfoValue, null);
                }
            }
            return targetEntity;
        }

         


        /// <summary>
        /// 深度拷贝,可复制里面的类属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
            where T : class
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// 复制控件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeObj"></param>
        /// <returns></returns>
        public static T GetObjectClone<T>(this T serializeObj)
        {
            string base64String = SerializeObject<T>(serializeObj);
            return DeserializeObject<T>(base64String);
        }

        /// <summary>
        /// 序列化为Base64String。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeObj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(this T serializeObj)
        {
            string base64String = string.Empty;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(ms, serializeObj);
                base64String = Convert.ToBase64String(ms.GetBuffer());
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                ms.Close();
            }
            return base64String;
        }

        /// <summary>
        /// 从Base64反序列化。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string base64String)
        {
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64String));
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                // Deserialize the hashtable from the file and
                // assign the reference to the local variable.
                return (T)formatter.Deserialize(ms);
            }
            catch
            {
                return (T)(new object());
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
