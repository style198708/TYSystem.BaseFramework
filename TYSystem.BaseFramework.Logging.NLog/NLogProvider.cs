using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using System.Collections.Concurrent;
using System.Xml;
using System.Reflection;
using System.IO;
using NLog.Config;

namespace TYSystem.BaseFramework.Logging.NLog
{
    public class NLogProvider : Microsoft.Extensions.Logging.ILoggerProvider
    {
        /// <summary>
        /// The loggers collection.
        /// </summary>
        private readonly ConcurrentDictionary<string, NLogLogger> loggers = new ConcurrentDictionary<string, NLogLogger>();

        public NLogProvider(string configFile)
        {
            LogManager.LoadConfiguration(configFile);
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
          => this.loggers.GetOrAdd(categoryName, this.CreateLoggerImplementation);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            this.loggers.Clear();
        }
        /// <summary>
        /// Parses log4net config file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The <see cref="XmlElement"/> with the log4net XML element.</returns>
        private static XmlElement ParsenlogConfigFile(string filename)
        {
            using (FileStream fp = File.OpenRead(filename))
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit
                };

                var Config = new XmlDocument();
                using (var reader = XmlReader.Create(fp, settings))
                {
                    Config.Load(reader);
                }
                return Config["nlog"];
            }
        }

        /// <summary>
        /// Creates the logger implementation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="Log4NetLogger"/> instance.</returns>
        private NLogLogger CreateLoggerImplementation(string name)
            => new NLogLogger(name);

        /// <summary>
        /// Tries to retrieve the assembly from a "Startup" type found in the stacktrace.
        /// </summary>
        /// <returns>Null for NetCoreApp 1.1 otherwise Assembly of Startup type if found in stacktrace.</returns>
        private static Assembly GetCallingAssemblyFromStartup()
        {
            var stackTrace = new System.Diagnostics.StackTrace(2);

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var type = frame.GetMethod()?.DeclaringType;

                if (string.Equals(type?.Name, "Startup", StringComparison.OrdinalIgnoreCase))
                {
                    return type.Assembly;
                }
            }
           return null;
        }
    }
}
