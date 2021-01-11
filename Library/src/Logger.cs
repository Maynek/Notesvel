//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
namespace Maynek.Notesvel.Library
{
    public delegate void LoggingHandler(LogLevel level, string value);

    public enum LogLevel : int
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical,
    }
       
    public class Logger
    {
        //================================
        // Fields
        //================================
        private static Logger Instance = null;
        private event LoggingHandler Handler;


        //================================
        // Properties
        //================================
        public static LoggingHandler LoggingHandler
        {
            get
            {
                return Logger.GetInstance().Handler;
            }

            set
            {
                Logger.GetInstance().Handler = value;
            }
        }

        //================================
        // Constructor
        //================================
        private Logger(){ }


        //================================
        // Static Methods
        //================================
        public static Logger GetInstance()
        {
            if (Logger.Instance == null)
            {
                Logger.Instance = new Logger();
            }

            return Logger.Instance;
        }

        public static void Write(LogLevel level, string value)
        {
            Logger.GetInstance().Handler?.Invoke(level, value);
        }

        public static void T(string value)
        {
            Logger.Write(LogLevel.Trace, value);
        }

        public static void D(string value)
        {
            Logger.Write(LogLevel.Debug, value);
        }

        public static void I(string value)
        {
            Logger.Write(LogLevel.Information, value);
        }

        public static void W(string value)
        {
            Logger.Write(LogLevel.Warning, value);
        }

        public static void E(string value)
        {
            Logger.Write(LogLevel.Error, value);
        }

        public static void C(string value)
        {
            Logger.Write(LogLevel.Critical, value);
        }
    }
}
