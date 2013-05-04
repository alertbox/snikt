using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Snikt
{
    public interface IDatabase : IDisposable
    {
        IDbConnectionFactory ConnectionFactory { get; }
        IDbConnection Connection { get; }

        IEnumerable<T> SqlQuery<T>(string sql) where T : class, new();
        IEnumerable<T> SqlQuery<T>(string sql, object parameters) where T : class, new();
    }
}
