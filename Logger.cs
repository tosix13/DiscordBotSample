using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.NetCore
{
    public enum LogLevel
    {
        TRACE = 0,
        DEBUG = 1,
        INFO = 2,
        WARN = 3,
        ERROR = 4
    }
    public class Logger
    {
        private const string LOGLEVEL_LOWER_TRACE = "trace";
        private const string LOGLEVEL_LOWER_DEBUG = "debug";
        private const string LOGLEVEL_LOWER_INFO = "info";
        private const string LOGLEVEL_LOWER_WARN = "warn";
        private const string LOGLEVEL_LOWER_ERROR = "error";

        private const string LOGTAG_TRACE = "7";
        private const string LOGTAG_DEBUG = "6";
        private const string LOGTAG_INFO = "5";
        private const string LOGTAG_WARN = "4";
        private const string LOGTAG_ERROR = "3";

        private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff zzz";

        public static LogLevel _outputLogLevel { get; private set; } = LogLevel.INFO;
        private string LoggingClassName { get; }

        private Logger(string iClassName)
        {
            LoggingClassName = iClassName;
        }

        public static Logger GetLogger(Type type)
        {
            return new Logger(type.FullName);
        }

        public static void SetOutputLogLevel(string iLogLevel)
        {
            string tmpLevel = iLogLevel.ToLower();

            switch (tmpLevel)
            {
                case LOGLEVEL_LOWER_TRACE:
                    _outputLogLevel = LogLevel.TRACE;
                    break;
                case LOGLEVEL_LOWER_DEBUG:
                    _outputLogLevel = LogLevel.TRACE;
                    break;
                case LOGLEVEL_LOWER_INFO:
                    _outputLogLevel = LogLevel.INFO;
                    break;
                case LOGLEVEL_LOWER_WARN:
                    _outputLogLevel = LogLevel.WARN;
                    break;
                case LOGLEVEL_LOWER_ERROR:
                    _outputLogLevel = LogLevel.ERROR;
                    break;
                default:
                    throw new Exception("Invalid LogLevel string.");
            }
        }

        /// <summary>
        /// ????????????????????????????????????????????????????????????????????????
        /// ??????????????????????????????????????????????????????
        /// </summary>
        /// <param name="iLevel">???????????????</param>
        /// <param name="iMsg">??????????????????</param>
        public void WriteLog(LogLevel iLevel, string iMsg)
        {
            if ((int)iLevel < (int)_outputLogLevel)
            {
                return;
            }

            string tag = string.Empty;
            switch (iLevel)
            {
                case LogLevel.TRACE:
                    tag = LOGTAG_TRACE;
                    break;
                case LogLevel.DEBUG:
                    tag = LOGTAG_DEBUG;
                    break;
                case LogLevel.INFO:
                    tag = LOGTAG_INFO;
                    break;
                case LogLevel.WARN:
                    tag = LOGTAG_WARN;
                    break;
                case LogLevel.ERROR:
                    tag = LOGTAG_ERROR;
                    break;
            }

            string nowTime = DateTime.Now.ToString(DATETIME_FORMAT);
            List<string> msgs = new List<string> { string.Empty };

            // ??????????????????????????????????????????????????????
            if (null != iMsg)
            {
                msgs = iMsg.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n").ToList();
            }

            foreach (var lineMsg in msgs)
            {
                Console.WriteLine($"<{tag}> {nowTime} [{lineMsg}]");
            }
        }
    }
}
