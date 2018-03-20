using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using TYSystem.BaseFramework.Extension;
using TYSystem.BaseFramework.Exceptions;

namespace TYSystem.BaseFramework.Enum
{
    /// <summary>
    /// 枚举属性扩展信息
    /// </summary>
    public static class EnumBaseExtentions
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this EnumBase enumValue, int value)
        {
            return EnumBase.GetDescription(enumValue.GetType(), value);
        }
    }

    public class EnumBaseDictionary
    {
        public string DictionaryText { get; set; }
        public int DictionaryValue { get; set; }
    }
    public abstract class EnumBase
    {
        public static List<EnumBaseDictionary> GetEnumBaseDictionary(Type type)
        {
            List<EnumBaseDictionary> result = new List<EnumBaseDictionary>();
            Dictionary<int, string> keyValue = GetDictionary(type);
            foreach (KeyValuePair<int, string> item in keyValue)
            {
                result.Add(new EnumBaseDictionary() { DictionaryText = item.Value, DictionaryValue = item.Key });
            }
            return result;
        }
        /// <summary>
        /// 获取所有字段值和说明信息的列表
        /// </summary>
        public static Dictionary<int, string> GetDictionary(Type type)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                dictionary.Add(field.GetValue(type).ToInt(), GetDescription(field));
            }

            return dictionary;
        }
        /// <summary>
        /// 获取所有字段值和说明信息的列表
        /// </summary>
        public static Dictionary<string, string> GetDictionaryString(Type type)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (dictionary.ContainsKey(field.GetValue(type).ToString()))
                {
                    throw new WaLiuException(string.Format("{0},{1}已存在", type.ToString(), field.GetValue(type).ToString()));
                }
                dictionary.Add(field.GetValue(type).ToString(), GetDescription(field));
            }

            return dictionary;
        }
        /// <summary>
        /// 获取所有名称和字段值
        /// </summary>
        public static Dictionary<string, string> GetDictionaryNameDesc(Type type)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (dictionary.ContainsKey(field.Name.ToString()))
                {
                    throw new WaLiuException(string.Format("{0},{1}已存在", type.ToString(), field.GetValue(type).ToString()));
                }
                dictionary.Add(field.Name, field.GetValue(type).ToString());
            }

            return dictionary;
        }
        public static Dictionary<int, string[,]> GetAllDictionary(Type type)
        {
            Dictionary<int, string[,]> dictionary = new Dictionary<int, string[,]>();

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                dictionary.Add(field.GetValue(type).ToInt(), new string[,] { { GetDescription(field), field.Name } });
            }

            return dictionary;
        }

        public static string GetName(Type type, BindingFlags flags, int? value)
        {
            FieldInfo[] fields = type.GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == value) return field.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字段名称信息
        /// </summary>
        public static string GetName(Type type, int? value)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == value) return field.Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取值信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetVaule(Type type, string name)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.Name == name)
                    return field.GetValue(type).ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取字段说明信息(跟进语言特征)
        /// </summary>
        public static string GetDescription(Type type, int? value)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == value) return GetDescription(field);
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取字段说明信息(跟进语言特征)
        /// </summary>
        public static string GetDescriptionForm(Type type, object value)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == (value).ToInt(-9999)) return GetDescription(field);
            }

            return string.Empty;
        }
        public static string GetDescription(Type type, int value, bool isEng)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == value) return GetDescription(field, isEng);
            }

            return string.Empty;
        }

        public static string GetZHCNDescription(Type type, int value)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == value) return GetDescription(field, false);
            }

            return string.Empty;
        }

        public static string GetEngDescription(Type type, int value)
        {
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(type).ToInt() == value) return GetDescription(field, true);
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取字段说明信息(跟进语言特征)
        /// </summary>
        protected static string GetDescription(FieldInfo field)
        {
            object[] attributes = field.GetCustomAttributes(false);

            string defaultAttribute = field.Name;

            if (attributes.Length > 0)
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is EnumAttribute)
                    {
                        EnumAttribute attr = attributes[i] as EnumAttribute;
                        if (string.IsNullOrEmpty(attr.DescriptionEng)) attr.DescriptionEng = defaultAttribute;

                        if (string.IsNullOrEmpty(attr.Description)) attr.Description = defaultAttribute;

                        if (string.Compare("zh-CN", Thread.CurrentThread.CurrentUICulture.Name, true) == 0) return attr.Description;

                        if (string.Compare("en-US", Thread.CurrentThread.CurrentUICulture.Name, true) == 0) return attr.DescriptionEng;
                    }

                }
            }

            return defaultAttribute;
        }
        private static string GetDescription(FieldInfo field, bool isEng)
        {
            object[] attributes = field.GetCustomAttributes(false);

            string defaultAttribute = field.Name;

            if (attributes.Length > 0)
            {
                for (int i = 0; i < attributes.Length;)
                {
                    EnumAttribute attr = attributes[i] as EnumAttribute;

                    if (string.IsNullOrEmpty(attr.DescriptionEng)) attr.DescriptionEng = defaultAttribute;

                    if (string.IsNullOrEmpty(attr.Description)) attr.Description = defaultAttribute;
                    i++;
                    if (isEng)
                        return attr.DescriptionEng;
                    else
                        return attr.Description;
                }
            }

            return defaultAttribute;
        }

        #region ListItemCollection初始化
        /// <summary>
        /// ListItemCollection初始化
        /// </summary>
        /// <typeparam name="T">初始化的数据源</typeparam>
        /// <param name="items">初始化的对象</param>
        //public static void ListItemBind<T>(System.Web.UI.WebControls.ListItemCollection items)
        //    where T : EnumBase
        //{
        //    Dictionary<int, string> dict = EnumBase.GetDictionary(typeof(T));
        //    foreach (KeyValuePair<int, string> item in dict)
        //    {
        //        items.Add(new System.Web.UI.WebControls.ListItem(item.Value, item.Key.ToString()));
        //    }
        //    items.Insert(0, new System.Web.UI.WebControls.ListItem("--请选择--", "", true));
        //}
        #endregion

        #region ListItemCollection数据绑定
        /// <summary>
        /// System.Web.UI.WebControls.ListItemCollection根据value来初始化
        /// </summary>
        /// <param name="items">需要初始化的对象</param>
        /// <param name="value">初始化的值</param>
        //public static void ListItemBindFromSource(System.Web.UI.WebControls.ListItemCollection items, int value)
        //{
        //    foreach (System.Web.UI.WebControls.ListItem item in items)
        //    {
        //        if (!string.IsNullOrEmpty(item.Value) && int.Parse(item.Value) == value)
        //        {
        //            item.Selected = true;
        //        }
        //    }
        //}
        #endregion

    }
}
