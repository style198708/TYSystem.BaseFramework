using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TYSystem.BaseFramework.Configuration
{
    public class Config
    {
        public static AppSettings Settings { get; set; }

        static Config()
        {
            if (Settings == null)
            {
                ConfigurationBuilder bulider = new ConfigurationBuilder();
                bulider.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                IConfigurationRoot config = bulider.Build();
                Settings = new AppSettings();
                config.Bind(Settings);
            }
        }



        /// <summary>
        /// 配置映射对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        public static void Bind<T>(string file, T entity, string key = null)
        {
            ConfigurationBuilder bulider = new ConfigurationBuilder();
            bulider.SetBasePath(Settings.ConfigPath).AddJsonFile(file);
            IConfigurationRoot config = bulider.Build();
            if (key == null)
            {
                config.Bind(entity);
            }
            else
            {
                config.Bind(key, entity);
            }
        }
        public static T Bind<T>(string file, string key = null)
        {
            T entity = Activator.CreateInstance<T>();
            Bind<T>(file, entity, key);
            return entity;
        }
    }


    public class AppSettings
    {
        public Logging Logging { get; set; }

        public string ConfigPath { get; set; }

        public string TableConfig { get; set; }
    }

    public class Logging
    {
        public string IncludeScopes { get; set; }
        public Debug Debug { get; set; }

        public Console Console { get; set; }
    }

    public class Debug
    {
        public LogLevel Level { get; set; }
    }
    public class Console
    {
        public LogLevel Level { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }


}
