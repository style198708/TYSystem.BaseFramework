using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.Logging.NLog
{
    public static class NLogExtensions
    {
        /// <summary>
        /// The default log4net config file name.
        /// </summary>
        private const string DefaultLog4NetConfigFile = "nlog.config";
        
        /// <summary>
        /// Adds the log4net.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="log4NetConfigFile">The log4net Config File.</param>
        /// <returns>The <see cref="ILoggerFactory"/>.</returns>
        public static ILoggerFactory AddNLog(this ILoggerFactory factory, string log4NetConfigFile)
        {
            factory.AddProvider(new NLogProvider(log4NetConfigFile));
            return factory;
        }
        /// <summary>
        /// Adds the log4net.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>The <see cref="ILoggerFactory"/>.</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory.AddNLog(DefaultLog4NetConfigFile);
            return factory;
        }

        /// <summary>
        /// Adds the log4net logging provider.
        /// </summary>
        /// <param name="builder">The logging builder instance.</param>
        /// <param name="exceptionFormatter">The exception formatter.</param>
        /// <returns></returns>
        public static ILoggingBuilder AddNLog(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new NLogProvider(DefaultLog4NetConfigFile));
            return builder;
        }

        /// <summary>
        /// Adds the log4net logging provider.
        /// </summary>
        /// <param name="builder">The logging builder instance.</param>
        /// <param name="log4NetConfigFile">The log4net Config File.</param>
        /// <returns></returns>
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string nlogConfigFile)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new NLogProvider(nlogConfigFile));
            return builder;
        }
    }
}
