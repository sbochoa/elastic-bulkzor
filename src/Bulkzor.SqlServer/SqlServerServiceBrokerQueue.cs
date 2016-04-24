using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Dapper;
using Nest;

namespace Bulkzor.SqlServer
{
    public class SqlServerServiceBrokerQueue
        : ISource
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly string _rootElement;
        private readonly int? _resultsQuantity;
        private const int DefaultResultsQuantity = 10;

        public SqlServerServiceBrokerQueue(string connectionString, string queueName, string rootElement)
        {
            _connectionString = connectionString;
            _queueName = queueName;
            _rootElement = rootElement;
        }

        public SqlServerServiceBrokerQueue(string connectionString, string queueName, string rootElement, int resultsQuantity)
            :this(connectionString, queueName, rootElement)
        {
            _resultsQuantity = resultsQuantity;
        }

        public IEnumerable<T> GetData<T>() 
            where T : class, IIndexableObject
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            var data = connection.Query<string>($@"WAITFOR( RECEIVE TOP({ _resultsQuantity ?? DefaultResultsQuantity }) 
                                                CONVERT(XML, message_body)
                                                AS Message
                                                FROM { _queueName})").ToList();

            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(_rootElement));

            var messageList = data.Select(xml => (T)serializer.Deserialize(new StringReader(xml))).ToList();

            connection.Close();

            return messageList;
        }
    }
}
