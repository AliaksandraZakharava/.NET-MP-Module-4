﻿namespace NETMP.FileSystemListener.Common
{
    public interface ILogger
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogError(string message);

        void LogFatal(string message);
    }
}
