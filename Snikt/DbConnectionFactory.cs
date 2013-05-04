using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Snikt
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public Dictionary<string, IDbConnection> Pool { get; private set; }

        protected DbConnectionFactory()
        {
            Pool = new Dictionary<string, IDbConnection>();
        }

        public static IDbConnectionFactory Get()
        {
            return new DbConnectionFactory();
        }

        public IDbConnection CreateIfNotExists(string nameOrConnectionString)
        {
            Assert.ThrowIfNull(nameOrConnectionString, "string nameOrConnectionString", Messages.NameOrConnectionStringNullOrEmpty);

            string connectionString = ParseToConnectionString(nameOrConnectionString);
            IDbConnection connection = GetConnection(connectionString);
            return connection;
        }

        private string ParseToConnectionString(string nameOrConnectionString)
        {
            string connectionString = null;
            if (NeedConfiguration(nameOrConnectionString))
            {
                string connectionName = GetConnectionName(nameOrConnectionString);
                connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }

            return connectionString ?? nameOrConnectionString;
        }

        private bool NeedConfiguration(string name)
        {
            return name.StartsWith("name=");
        }

        private string GetConnectionName(string stringName)
        {
            return stringName.Replace("name=", string.Empty);
        }

        private IDbConnection GetConnection(string connectionString)
        {
            if (NotPooled(connectionString))
            {
                CreatePooledConnection(connectionString);
            }
            return GetPooledConnection(connectionString);
        }

        private bool NotPooled(string connectionString)
        {
            return !Pooled(connectionString);
        }

        private bool Pooled(string connectionString)
        {
            return Pool.ContainsKey(connectionString);
        }

        private IDbConnection CreatePooledConnection(string connectionString)
        {
            IDbConnection connection = new SqlConnection(connectionString);
            PoolConnection(connection);
            return connection;
        }

        private void PoolConnection(IDbConnection connection)
        {
            Pool.Add(connection.ConnectionString, connection);
        }

        private IDbConnection GetPooledConnection(string connectionString)
        {
            IDbConnection connection = null;
            if (Pool.ContainsKey(connectionString))
            {
                connection = Pool[connectionString];
            }
            return connection;
        }
    }
}
