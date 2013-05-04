using Snikt.Specifications.Mocks.Poco;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Snikt.Specifications.Mocks.EntityFramework
{
    public class DbContextConnectionFactory : IDbConnectionFactory
    {
        public Dictionary<string, IDbConnection> Pool { get; private set; }

        public DbContextConnectionFactory()
        {
            Pool = new Dictionary<string, IDbConnection>();
        }

        public static IDbConnectionFactory Get()
        {
            return new DbContextConnectionFactory();
        }

        public IDbConnection CreateIfNotExists(string nameOrConnectionString)
        {
            if (NotPooled(nameOrConnectionString))
            {
                CreatePooledConnection(nameOrConnectionString);
            }
            return GetPooledConnection(nameOrConnectionString);
        }

        private bool NotPooled(string nameOrConnectionString)
        {
            return !Pooled(nameOrConnectionString);
        }

        private bool Pooled(string nameOrConnectionString)
        {
            return Pool.ContainsKey(nameOrConnectionString);
        }

        private void CreatePooledConnection(string nameOrConnectionString)
        {
            MiniNWDbContext dbContext = new MiniNWDbContext(nameOrConnectionString);
            Pool.Add(nameOrConnectionString, dbContext.Database.Connection);
        }

        private IDbConnection GetPooledConnection(string nameOrConnectionString)
        {
            IDbConnection connection = null;
            if (Pool.ContainsKey(nameOrConnectionString))
            {
                connection = Pool[nameOrConnectionString];
            }
            return connection;
        }
    }
}
