using System;
using Bulkzor.Configuration;
using Bulkzor.SqlServer;
using Bulkzor.Utils;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Executor.Configurations
{
    public class SqlServerTaskConfiguration : BaseTaskConfiguration, ITaskConfiguration
    {
        private readonly JObject _taskConfigurationObject;
        private readonly Uri[] _nodes;

        public SqlServerTaskConfiguration(JObject taskConfigurationObject, params Uri[] nodes)
            : base(taskConfigurationObject)
        {
            _taskConfigurationObject = taskConfigurationObject;
            _nodes = nodes;
        }

        public BulkTask CreateTask()
        {
            var connectionString = _taskConfigurationObject.GetConfigurationValue<string>("connectionString", required: true);
            var query = _taskConfigurationObject.GetConfigurationValue<string>("query", required: true);
            
            return BulkTask.ConfigureWith(configuration =>
            {
                configuration.Nodes(_nodes);
                configuration.ChunkConfiguration(new ChunkConfiguration().ChunkSize(ChunkSize));
                configuration.Source(new SqlServerQuery(connectionString, query));
                configuration.IndexName(IndexNameFunc);
            });
        }
    }
}
