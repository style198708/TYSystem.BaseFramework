using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TYSystem.BaseFramework.BaseResult
{
    /// <summary>
    /// 返回结果，object 类型
    /// </summary>
    public class JsonDataResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int? errno;
        /// <summary>
        /// 数据
        /// </summary>
        public object data;
        /// <summary>
        /// 数据2
        /// </summary>
        public object data2;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string errmsg;
        /// <summary>
        /// 
        /// </summary>
        public JsonDataResult()
        {
            errno = 0;//成功
            errmsg = "";
            data = new object();
            data2 = new object();
            
        }
    }

    /// <summary>
    ///  返回结果 T 类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonDataResult<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int? errno;
        /// <summary>
        /// 数据
        /// </summary>
        public T data;
        /// <summary>
        /// 数据2
        /// </summary>
        public object data2;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string errmsg;
        /// <summary>
        /// 
        /// </summary>
        public JsonDataResult()
        {
            errno = 0;//成功
            errmsg = "";
            data2 = new object();
        }
    }

}
