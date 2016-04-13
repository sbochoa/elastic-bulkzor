using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Bulkzor.SqlServer
{
    public class SqlServerQuery<T>
        : ISource<T>
        where T:class 
    {
        private readonly string _connectionString;
        private readonly string _sql;

        public SqlServerQuery(string connectionString, string sql)
        {
            _connectionString = connectionString;
            _sql = sql;
        }

        public IEnumerable<T> GetData()
        {
            var connection = new SqlConnection(_connectionString);
            
            connection.Open();

            var result = connection.Query<T>(_sql, buffered:false);

            //var cmd = new SqlCommand(_sql);

            //using (var cmdDataReader = cmd.ExecuteReader())
            //{
            //    while (cmdDataReader.Read())
            //    {
                    
            //        //var jsonObject = new JObject();

            //        //for (var i = 0; i < cmdDataReader.FieldCount; i++)
            //        //{
            //        //    jsonObject[cmdDataReader.GetName(i)] = new JValue(cmdDataReader.GetValue(i));
            //        //}

            //        //yield return jsonObject;
            //    }
            //}

            connection.Close();

            return result;
        }
    }
}
