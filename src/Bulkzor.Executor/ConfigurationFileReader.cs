using System;
using System.Collections.Generic;
using System.Linq;
using Bulkzor.Executor.Configurations;
using Bulkzor.Utils;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Executor
{
    public class ConfigurationFileReader
    {
        private readonly string _configurationFilePath;

        public ConfigurationFileReader(string configurationFilePath)
        {
            _configurationFilePath = configurationFilePath;
        }

        public IReadOnlyList<BulkTask> CreateTasks()
        {
            var configurationJson = System.IO.File.ReadAllText(_configurationFilePath);
            var configurationJObject = JObject.Parse(configurationJson);

            var uri = GetHostUri(configurationJObject);

            var tasks = configurationJObject.GetConfigurationValue<JArray>("tasks", required:true);

            return tasks.Children<JObject>().Select(task => CreateBulkTask(task, uri)).ToList();
        }

        private static BulkTask CreateBulkTask(JObject task, Uri uri)
        {
            var type = task.GetConfigurationValue<string>("type", required: true);
            ITaskConfiguration taskConfiguration;

            if (type == "sqlserver")
            {
                taskConfiguration = new SqlServerTaskConfiguration(task, uri);
            }
            else
            {
                throw new InvalidOperationException("Invalid Task Type");
            }

            return taskConfiguration.CreateTask();
        }

        private static Uri GetHostUri(JObject configurationJObject)
        {
            var host = configurationJObject.GetConfigurationValue<string>("host", required: true);
            var port = configurationJObject.GetConfigurationValue<int>("port", required: true);
            var uri = new Uri($"{host.TrimEnd('/')}:{port}");
            return uri;
        }
    }
}
