using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Snikt
{
    public interface IDbConnectionFactory
    {
        Dictionary<string, IDbConnection> Pool { get; }
        IDbConnection CreateIfNotExists(string nameOrConnectionString);
    }
}
