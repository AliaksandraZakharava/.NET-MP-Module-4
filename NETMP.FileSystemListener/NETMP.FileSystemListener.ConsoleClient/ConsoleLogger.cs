using System;
using NETMP.FileSystemListener.Common;
using Dates = NETMP.FileSystemListener.Common.Resources.Dates;

namespace NETMP.FileSystemListener.ConsoleClient
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            DisplayLog("INFO", message);
        }

        public void LogWarn(string message)
        {
            DisplayLog("WARN", message);
        }

        public void LogError(string message)
        {
            DisplayLog("ERROR", message);
        }

        public void LogFatal(string message)
        {
            DisplayLog("FATAL", message);
        }

        private void DisplayLog(string level, string message)
        {
            Console.WriteLine($"[{level}] {DateTime.UtcNow.ToString(Dates.DateAndTime)} | {message}");
        }
    }
}
