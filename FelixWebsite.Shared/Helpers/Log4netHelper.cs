using log4net;
using System;

namespace FelixWebsite.Core.Helpers
{
    public sealed class Log4NetHelper
    {
        private static ILog logger;
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Log4NetHelper()
        {
        }

        private Log4NetHelper()
        {
            logger = LogManager.GetLogger(GetType());
        }

        public static Log4NetHelper Instance { get; } = new Log4NetHelper();

        public void DebugLogMessage(string message)
        {
            logger.Debug(message);
        }

        public void DebugLogErrorMessage(string message, Exception e)
        {
            logger.Debug($"{message} {e}");
        }

        public void InfoLogMessage(string message)
        {
            logger.Info(message);
        }
    }
}
