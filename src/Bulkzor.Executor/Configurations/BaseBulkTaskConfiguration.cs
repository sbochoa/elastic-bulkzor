using Bulkzor.Utilities;
using Newtonsoft.Json.Linq;

namespace Bulkzor.Executor.Configurations
{
    public abstract class BaseBulkTaskConfiguration
    {
        private readonly JObject _taskConfigurationObject;

        protected BaseBulkTaskConfiguration(JObject taskConfigurationObject)
        {
            _taskConfigurationObject = taskConfigurationObject;
        }

        protected int ChunkSize => _taskConfigurationObject.GetConfigurationValue("chunkSize", 1000);
        protected string IndexTemplate => _taskConfigurationObject.GetConfigurationValue<string>("index");
        protected string Type => _taskConfigurationObject.GetConfigurationValue<string>("type");
    }
}
