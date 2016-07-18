using System.Linq;
using Bulkzor.Executor.Helpers;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.NLog;
using NLog.Config;
using NLog.Targets;

namespace Bulkzor.Executor
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationFilePath = "test.json";
            ConfigureNLogger();

            var tasks = new ConfigurationFileReader(configurationFilePath, new FileManager(), LogManager.GetLogger("bulkzor")).CreateTasks();

            Bulkzor.RunBulks(tasks.ToArray());
        }

        private static void ConfigureNLogger()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/logs/${logger}.txt";
            fileTarget.Layout = "${message}";

            var rule1 = new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            NLog.LogManager.Configuration = config;

            var properties = new NameValueCollection { };

            LogManager.Adapter = new NLogLoggerFactoryAdapter(properties);
        }
    }
}
