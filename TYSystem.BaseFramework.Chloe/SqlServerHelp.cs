using System;
using System.Collections.Generic;
using System.Text;
using Chloe;
using Chloe.SqlServer;
using TYSystem.BaseFramework.Serializer;
using System.IO;
using Chloe.Entity;
using System.Linq;

namespace TYSystem.BaseFramework.Chloe
{
    public static class SqlServerHelp
    {
        public static List<TableConfiguration> TableConfig { get; set; }

        static SqlServerHelp()
        {
            if (TableConfig == null)
                TableConfig = (List<TableConfiguration>)SerializerXml.LoadSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TableConfig.xml"), typeof(List<TableConfiguration>));
        }

        /// <summary>
        /// 取数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuery<T> ConfigInit<T>() where T : class
        {
            string ConfigName = GetConfigName<T>();
            TableConfiguration configuration = TableConfig.Where(p => p.Name == ConfigName).FirstOrDefault();
            if (configuration != null)
            {
                return new MsSqlContext(configuration.ConnectString).Query<T>();
            }
            return null;
        }

        /// <summary>
        /// 取数据库节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetConfigName<T>()
        {
            Type type = typeof(T);
            TableAttribute table = (TableAttribute)type.GetCustomAttributes(false)[0];
            return table.ConfigName;
        }

    }

    public static class ChloeExtentions
    {
        /// <summary>
        /// 实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this MsSqlContext client) where T : class, new()
        {
            return client.Query<T>().FirstOrDefault();
        }
    }
}
