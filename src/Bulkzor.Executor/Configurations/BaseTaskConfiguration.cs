using System;
using Bulkzor.Utils;
using Newtonsoft.Json.Linq;
using SmartFormat;

namespace Bulkzor.Executor.Configurations
{
    public abstract class BaseTaskConfiguration
    {
        private readonly JObject _taskConfigurationObject;

        protected BaseTaskConfiguration(JObject taskConfigurationObject)
        {
            _taskConfigurationObject = taskConfigurationObject;
        }

        protected int ChunkSize => _taskConfigurationObject.GetConfigurationValue("chunkSize", 1000, true);
        protected Func<object, string> IndexNameFunc => GetIndexNameFunc;
        private string GetIndexNameFunc(object objectFromSource)
        {
            var indexTemplate = _taskConfigurationObject.GetConfigurationValue<string>("index", required: true);
            return Smart.Format(indexTemplate, objectFromSource);
        }
    }
}
