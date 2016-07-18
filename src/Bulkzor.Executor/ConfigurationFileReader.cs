using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Executor.Configurations;
using Bulkzor.Executor.Helpers;
using Bulkzor.Utilities;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Executor
{
    public class ConfigurationFileReader
    {
        private readonly string _configurationFilePath;
        private readonly IFileManager _fileManager;

        public ConfigurationFileReader(string configurationFilePath, IFileManager fileManager)
        {
            _configurationFilePath = configurationFilePath;
            _fileManager = fileManager;
        }

        public IReadOnlyList<BulkTask> CreateTasks()
        {
            var configurationJson = _fileManager.ReadTextFromFile(_configurationFilePath);
            var configurationJObject = JObject.Parse(configurationJson);

            var tasks = configurationJObject.GetConfigurationValue<JArray>("tasks");

            return tasks.Children<JObject>().Select(CreateBulkTask).ToList();
        }

        private static BulkTask CreateBulkTask(JObject task)
        {
            var type = task.GetConfigurationValue<string>("type");
            IBulkTaskConfiguration bulkTaskConfiguration;

            if (type == "sqlserver")
            {
                bulkTaskConfiguration = new SqlServerBulkTaskConfiguration(task);
            }
            else
            {
                throw new InvalidOperationException("Invalid Task Type");
            }

            var taskName = task.GetConfigurationValue<string>("name");

            return bulkTaskConfiguration.CreateTask(taskName);
        }
    }
}
