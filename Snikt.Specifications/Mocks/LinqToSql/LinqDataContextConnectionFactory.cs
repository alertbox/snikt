using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Snikt.Specifications.Mocks.LinqToSql
{
    public class LinqDataContextConnectionFactory : IDbConnectionFactory
    {
        public Dictionary<string, IDbConnection> Pool { get; private set; }

        public LinqDataContextConnectionFactory()
        {
            Pool = new Dictionary<string, IDbConnection>();
        }

        public static IDbConnectionFactory Get()
        {
            return new LinqDataContextConnectionFactory();
        }

        public IDbConnection CreateIfNotExists(string nameOrConnectionString)
        {
            return new MiniNWDataContext().Connection;
        }
    }
}
