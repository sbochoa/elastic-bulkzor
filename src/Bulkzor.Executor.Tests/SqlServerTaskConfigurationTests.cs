using System;
using Bulkzor.Executor.Configurations;
using Common.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Should;

namespace Bulkzor.Executor.Tests
{
    [TestFixture]
    public class SqlServerTaskConfigurationTests
    {
        [Test]
        public void CreateTask_WhenConfiguredCorrectly_ShouldCreatedTaskCorrectly()
        {
            var taskName = "TestTask";
            var configurationJsonObject = new JObject
            {
                ["connectionString"] = "connectionString",
                ["query"] = "SELECT * FROM Foo",
                ["chunkSize"] = "250",
                ["index"] = "Index_Name",
                ["type"] = "type",
                ["host"] = "http://localhost",
                ["port"] = "22"
            };

            var sqlConfiguration = new SqlServerBulkTaskConfiguration(configurationJsonObject, LogManager.GetLogger("test"));
            var bulkTask = sqlConfiguration.CreateTask(taskName);
            var bulkTaskConfiguration = bulkTask.BulkTaskConfiguration;

            bulkTaskConfiguration.TaskName.ShouldEqual(taskName);
        }
    }
}
