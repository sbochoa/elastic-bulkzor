using System;
using System.Data.SqlClient;
using Bulkzor.SqlServer;
using NUnit.Framework;

namespace Bulkzor.Tests
{
    [TestFixture]
    public class BulkzorTests
    {
        [Test]
        public void Bulk_Correctly()
        {
            var sql = "select TOP 100000 * from planets";
            var bulkzor = Bulkzor<Planet>
                .ConfigureWith(cfg =>
                {
                    cfg.Nodes(new Uri("http://localhost:9200"));
                    cfg.Source(new SqlServerQuery<Planet>("server=.;integrated security=true;initial catalog=ElasticSearch", sql));
                    cfg.IndexName("planets");
                    cfg.TypeName("planet");
                });

            bulkzor.RunBulk();
        }
    }

    public class Planet
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
