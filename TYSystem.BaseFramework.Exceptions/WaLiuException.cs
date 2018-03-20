using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TYSystem.BaseFramework.Exceptions
{
    /// <summary>
    /// 功能：表示在财视能开发框架中触发的异常
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class WaLiuException : Exception
    {
        /// <summary>
        /// 初始化一个新的异常实例
        /// </summary>
        public WaLiuException() : base() { }
        /// <summary>
        /// 通过一个具体的异常消息初始化一个新的异常实例
        /// </summary>
        /// <param name="message">异常消息</param>
        public WaLiuException(string message) : base(message) { }
        /// <summary>
        /// 通过一个内部异常和异常消息初始化一个新的异常实例
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public WaLiuException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>
        /// 通过指定的字符串格式化和参数初始化一个新的异常实例
        /// </summary>
        /// <param name="format">用于格式化异常信息的字符串格式</param>
        /// <param name="args">用于格式化生成错误消息的参数。</param>
        public WaLiuException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
