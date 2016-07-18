using System.Collections.Generic;
using System.Linq;
using Bulkzor.Executor.Tests.Fakes;
using Bulkzor.SqlServer;
using Newtonsoft.Json;
using NUnit.Framework;
using Should;
using SpecsFor;

namespace Bulkzor.Executor.Tests.ConfigurationFileReader
{
    public class WhenCreateSqlTasks : SpecsFor<Executor.ConfigurationFileReader>
    {
        const string ConfigurationFilePath = "config.json";
        private IReadOnlyList<BulkTask> _bulkTasks; 
        private readonly FakeFileManager _fileManager = new FakeFileManager();

        protected override void Given()
        {
            ConfigurationJsonFile();
        }

        private void ConfigurationJsonFile()
        {
            var configurationObject = new
            {
                tasks = new []
                {
                    new
                    {
                        type = "sqlserver",
                        name = "simplequery2",
                        connectionString = "connectionString",
                        query = "query2",
                        index = "index_name",
                        host = "http://localhost",
                        port = 22
                    },
                    new
                    {
                        type = "sqlserver",
                        name = "simplequery3",
                        connectionString = "connectionString",
                        query = "query3",
                        index = "index_name",
                        host = "http://localhost",
                        port = 22
                    }
                }
            };

            _fileManager.Files.Add(ConfigurationFilePath, JsonConvert.SerializeObject(configurationObject));
        }

        protected override void InitializeClassUnderTest()
        {
            SUT = new Executor.ConfigurationFileReader(ConfigurationFilePath, _fileManager);
        }

        protected override void When()
        {
            _bulkTasks = SUT.CreateTasks();
        }

        [Test]
        public void ShouldCreateMoreThanOneSqlBulkTask()
        {
            _bulkTasks.ShouldNotBeEmpty();
        }

        [Test]
        public void ShouldCreateTwoSqlBulkTask()
        {
            _bulkTasks.Count().ShouldEqual(2);
        }

        [Test]
        public void ShouldHaveTheCorrectName()
        {
            _bulkTasks[0].BulkTaskConfiguration.TaskName.ShouldEqual("simplequery2");
            _bulkTasks[1].BulkTaskConfiguration.TaskName.ShouldEqual("simplequery3");
        }

        [Test]
        public void HostShouldBeCorrect()
        {
            var expectedFullHost = "http://localhost:22";
            _bulkTasks[0].BulkTaskConfiguration.GetFullHost().ShouldEqual(expectedFullHost);
            _bulkTasks[1].BulkTaskConfiguration.GetFullHost().ShouldEqual(expectedFullHost);
        }
    }
}
