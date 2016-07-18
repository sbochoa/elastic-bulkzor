using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Executor.Configurations;
using Bulkzor.Executor.Helpers;
using Bulkzor.Utilities;
using Common.Logging;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Executor
{
    public class ConfigurationFileReader
    {
        private readonly string _configurationFilePath;
        private readonly IFileManager _fileManager;
        private readonly ILog _logger;

        public ConfigurationFileReader(string configurationFilePath, IFileManager fileManager, ILog logger)
        {
            _configurationFilePath = configurationFilePath;
            _fileManager = fileManager;
            _logger = logger;
        }

        public IReadOnlyList<BulkTask> CreateTasks()
        {
            var configurationJson = _fileManager.ReadTextFromFile(_configurationFilePath);
            var configurationJObject = JObject.Parse(configurationJson);

            var tasks = configurationJObject.GetConfigurationValue<JArray>("tasks");

            return tasks.Children<JObject>().Select(CreateBulkTask).ToList();
        }

        private BulkTask CreateBulkTask(JObject task)
        {
            var type = task.GetConfigurationValue<string>("taskType");
            IBulkTaskConfiguration bulkTaskConfiguration;

            if (type == "sqlserver")
            {
                bulkTaskConfiguration = new SqlServerBulkTaskConfiguration(task, _logger);
            }
            else
            {
                throw new InvalidOperationException("Invalid Task Type");
            }

            var taskName = task.GetConfigurationValue<string>("taskName");

            return bulkTaskConfiguration.CreateTask(taskName);
        }
    }
}
