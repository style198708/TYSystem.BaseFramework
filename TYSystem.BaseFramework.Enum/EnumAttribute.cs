using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TYSystem.BaseFramework.Enum
{
    /// <summary>
    /// 自定义属性
    /// </summary>    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EnumAttribute : Attribute
    {
        /// <summary>
        /// 英文说明信息
        /// </summary>
        public string DescriptionEng { set; get; }

        /// <summary>
        /// 说明信息
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 初始化说明信息
        /// </summary>
        /// <param name="description">说明信息</param>
        public EnumAttribute(string description)
        {
            this.Description = description;
            this.DescriptionEng = string.Empty;
        }

        /// <summary>
        /// 初始化说明信息及适用区域性
        /// </summary>
        /// <param name="description">说明信息</param>
        /// <param name="culture">适用区域性</param>
        public EnumAttribute(string description, string eng)
        {
            this.Description = description;
            this.DescriptionEng = eng;
        }
    }
}
