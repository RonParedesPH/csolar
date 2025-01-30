using log4net;

namespace logger
{
    /*  common class for stockroomDap
   public class stockroomDap
   {
   }
   ------------------------------------ */

    public static class log
    {
        private static ILog _log = null;
        private static string _logFile = null;

        public enum TracingLevel
        {
            ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF
        }


        public static void Initialize()
        {

            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _log.Info("VerityClouds stockroomDap service initialization..");
        }

        public static string LogFile
        {
            get { return _logFile; }
        }

        public static void LogMessage(TracingLevel Level, string Message)
        {
            switch (Level)
            {
                case TracingLevel.DEBUG:
                    _log.Debug(Message);
                    break;

                case TracingLevel.INFO:
                    _log.Info(Message);
                    break;

                case TracingLevel.WARN:
                    _log.Warn(Message);
                    break;

                case TracingLevel.ERROR:
                    _log.Error(Message);
                    break;

                case TracingLevel.FATAL:
                    _log.Fatal(Message);
                    break;
            }
        }
    }

}