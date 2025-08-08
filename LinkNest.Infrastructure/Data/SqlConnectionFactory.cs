using Dapper;
using LinkNest.Application.Abstraction.Data;
using Npgsql;
using System.Data;

namespace LinkNest.Infrastructure.Data
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionFactory;

        public SqlConnectionFactory(string connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionFactory);
            connection.Open();

            return connection;
        }
    }
}
