using NLog;
using NLog.Config;
using NLog.Targets;

namespace Succubus.Common
{
    public static class Log
    {
        public static void InitializeLogger()
        {
            var loggingConfig = new LoggingConfiguration();
            var coloredConsoleTarget = new ColoredConsoleTarget()
            {
                Layout = "[${logger:shortName=true}] - ${shortdate}\n${message}\n"
            };

            loggingConfig.AddTarget("Console", coloredConsoleTarget);
            loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, coloredConsoleTarget));

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "\\[[^\\]]*\\]",
                ForegroundColor = ConsoleOutputColor.Cyan,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild Joined",
                ForegroundColor = ConsoleOutputColor.Green,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild Left",
                ForegroundColor = ConsoleOutputColor.Blue,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild Available",
                ForegroundColor = ConsoleOutputColor.Yellow
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Name",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Owner",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Members",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Created",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "User",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Channel",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Guild",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Date",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            coloredConsoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Text = "Raw Message",
                ForegroundColor = ConsoleOutputColor.Red,
            });

            LogManager.Configuration = loggingConfig;
        }
    }
}