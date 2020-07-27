using NLog;
using NLog.Config;
using NLog.Targets;

namespace Succubus.Logger
{
    public static class LoggerUtils
    {
        public static void InitializeLogger()
        {
            var loggingConfig = new LoggingConfiguration();
            var coloredConsoleTarget = new ColoredConsoleTarget()
            {
                Layout = @"${shortdate} [${logger}] ${message}"
            };

            loggingConfig.AddTarget(coloredConsoleTarget);
            loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, coloredConsoleTarget));

            LogManager.Configuration = loggingConfig;
        }
    }
}
