using System.Collections.Generic;
using System.Data.SqlClient;
using Bulkzor.Utilities;
using Dapper;

namespace Bulkzor.SqlServer
{
    public class SqlServerQuery
        : IManagedSource
    {
        public string SqlQueryQuery => _sqlQuery;
        private readonly string _connectionString;
        private readonly string _sqlQuery;
        private readonly bool _buffered;
        private SqlConnection _connection;

        public SqlServerQuery(string connectionString, string sqlQuery, bool buffered = false)
        {
            Check.NotEmpty(connectionString, "Connection String");
            Check.NotEmpty(sqlQuery, "Sql Query");

            _connectionString = connectionString;
            _sqlQuery = sqlQuery;
            _buffered = buffered;
        }

        public void OpenConnection()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public IEnumerable<object> GetData()
        {
            return _connection.Query(_sqlQuery, buffered: _buffered);
        }

        public void CloseConnection()
        {
            _connection.Close();
        }
    }
}
