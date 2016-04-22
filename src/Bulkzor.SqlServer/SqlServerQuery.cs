using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Bulkzor.SqlServer
{
    public class SqlServerQuery
        : IManagedSource
    {
        private readonly string _connectionString;
        private readonly string _sql;
        private readonly bool _buffered;
        private SqlConnection _connection;

        public SqlServerQuery(string connectionString, string sql, bool buffered = false)
        {
            _connectionString = connectionString;
            _sql = sql;
            _buffered = buffered;
        }

        public void OpenConnection()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public IEnumerable<T> GetData<T>()
            where T : class
        {
            return _connection.Query<T>(_sql, buffered: _buffered);
        }

        public void CloseConnection()
        {
            _connection.Close();
        }
    }
}
