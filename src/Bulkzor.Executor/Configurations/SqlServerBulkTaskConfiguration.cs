using Bulkzor.Configuration;
using Bulkzor.SqlServer;
using Bulkzor.Utilities;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Executor.Configurations
{
    public class SqlServerBulkTaskConfiguration : BaseBulkTaskConfiguration, IBulkTaskConfiguration
    {
        private readonly JObject _taskConfigurationObject;

        public SqlServerBulkTaskConfiguration(JObject taskConfigurationObject)
            : base(taskConfigurationObject)
        {
            _taskConfigurationObject = taskConfigurationObject;
        }

        public BulkTask CreateTask(string taskName)
        {
            Check.NotEmpty(taskName, nameof(taskName));

            var connectionString = _taskConfigurationObject.GetConfigurationValue<string>("connectionString");
            var query = _taskConfigurationObject.GetConfigurationValue<string>("query");
            var host = _taskConfigurationObject.GetConfigurationValue<string>("host");
            var port = _taskConfigurationObject.GetConfigurationValue<int>("port");
            var source = new SqlServerQuery(connectionString, query);
            var bulkTaskConfiguration = new BulkTaskConfiguration(taskName, host, port) { IndexName = IndexTemplate };
            var chunkConfiguration = new ChunkConfiguration() { ChunkSize =  ChunkSize };

            return new BulkTask(bulkTaskConfiguration, chunkConfiguration, source, null);
        }
    }
}
