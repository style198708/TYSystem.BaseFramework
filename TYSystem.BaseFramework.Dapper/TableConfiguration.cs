using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYSystem.BaseFramework.Dapper
{
    /// <summary>
    /// 
    /// </summary>
    public class DapperConfig
    {
        public List<TableConfiguration> TableConfigurations { get; set; }
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    public class TableConfiguration
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库类型 
        /// </summary>
        public string SqlType { get; set; }
        /// <summary>
        /// 是否支持锁
        /// </summary>
        public bool WithLock { get; set; }
        /// <summary>
        /// 隶属
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 是否支持缓存
        /// </summary>
        public bool CacheDependency { get; set; }
        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string ConnectString { get; set; }
    }
}
