using NLog;

namespace LCPC.Share;

public static class LoggerManager
{
    private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();
    public static void LoggerInfo(string message)
    {
         _logger.Info(message);
    }

    public static void LoggerWarn(string message)
    {
        _logger.Warn(message);
    }

    public static void LoggerError(string message, Exception exception)
    {
        _logger.Error(message,exception);
    }

    public static void LoggerDebugger(string message)
    {
        _logger.Debug(message);
    }
}