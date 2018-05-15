using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Level = Microsoft.Extensions.Logging.LogLevel;

namespace TYSystem.BaseFramework.Logging.NLog
{

    public class NLogLogger : Microsoft.Extensions.Logging.ILogger
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly Logger log;

        public NLogLogger(string  name)
        {
            this.log = LogManager.GetLogger(name);
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return null; 
        }

        public bool IsEnabled(Level logLevel)
        {
            switch (logLevel)
            {
                case Level.Critical:
                    return log.IsFatalEnabled;
                case Level.Debug:
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return log.IsDebugEnabled;
                case Level.Error:
                    return log.IsErrorEnabled;
                case Level.Information:
                    return log.IsInfoEnabled;
                case Level.Warning:
                    return log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }

            throw new NotImplementedException();
        }

        public void Log<TState>(Level logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }
            if (null == formatter)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            string message = formatter(state, exception);
            if (!string.IsNullOrEmpty(message)
                || exception != null)
            {
                switch (logLevel)
                {
                    case Level.Critical:
                        //log.Fatal(message, exception);
                        break;
                    case Level.Debug:
                    case Level.Trace:
                        //log.Debug(message, exception);
                        break;
                    case Level.Error:
                        //log.Error(message, exception);
                        break;
                    case Level.Information:
                        //log.Info(message, exception);
                        break;
                    case Level.Warning:
                        //log.Warn(message, exception);
                        break;
                    default:
                        // log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        //log.Info(message, exception);
                        break;
                }
            }
        }
    }
}
