//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.Data;

namespace Maynek.Notesvel.Library
{
    public enum LogLevel : int
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical,
    }

    public delegate void LoggingHandler(LogLevel level, string value);

    public class Logger
    {
        public static Logger Instance { get; private set; } = null;
        public static void SetInstance(Logger logger)
        {
            Logger.Instance = logger;
        }
        
        //================================
        // Fields
        //================================
        public event LoggingHandler Handler;


        //================================
        // Methods
        //================================
        public void Write(LogLevel level, string value)
        {
            this.Handler?.Invoke(level, value);
        }

        public void T(string value)
        {
            this.Write(LogLevel.Trace, value);
        }

        public void D(string value)
        {
            this.Write(LogLevel.Debug, value);
        }

        public void I(string value)
        {
            this.Write(LogLevel.Information, value);
        }

        public void W(string value)
        {
            this.Write(LogLevel.Warning, value);
        }

        public void E(string value)
        {
            this.Write(LogLevel.Error, value);
        }

        public void C(string value)
        {
            this.Write(LogLevel.Critical, value);
        }
    }
}
