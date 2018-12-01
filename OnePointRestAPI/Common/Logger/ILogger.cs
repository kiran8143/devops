using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.Common.Logger
{
    public interface ILogger
    {
        void Log(string message, LogType logType);
        void Log(Exception exception, LogType logType);
        void Log(string message, Exception exception, LogType logType);
        void Log(Type caller, string message, LogType logType);
        void Log(Type caller, string message, Exception exception, LogType logType);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public enum LogType
    {
        Warn,
        Info,
        Debug,
        Error,
        Fatal
    }
}
