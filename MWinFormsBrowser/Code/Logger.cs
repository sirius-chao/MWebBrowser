using log4net;
using System;

public static class MLogger
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    //static MLogger()
    //{
    //  var s =  log4net.Config.XmlConfigurator.Configure(); // 初始化log4net配置
    //}

    private static void Log(LogLevel level, string message, Exception ex = null)
    {
        switch (level)
        {
            case LogLevel.Debug:
                if (log.IsDebugEnabled) log.Debug(message, ex);
                break;
            case LogLevel.Info:
                if (log.IsInfoEnabled) log.Info(message, ex);
                break;
            case LogLevel.Warn:
                log.Warn(message, ex);
                break;
            case LogLevel.Error:
                log.Error(message, ex);
                break;
            case LogLevel.Fatal:
                log.Fatal(message, ex);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }

    public static void Debug(string message, Exception ex = null) => Log(LogLevel.Debug, message, ex);
    public static void Info(string message, Exception ex = null) => Log(LogLevel.Info, message, ex);
    public static void Warn(string message, Exception ex = null) => Log(LogLevel.Warn, message, ex);
    public static void Error(string message, Exception ex = null) => Log(LogLevel.Error, message, ex);
    public static void Fatal(string message, Exception ex = null) => Log(LogLevel.Fatal, message, ex);

    private enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
