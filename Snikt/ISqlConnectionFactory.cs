using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Snikt
{
    public interface ISqlConnectionFactory
    {
        Dictionary<string, SqlConnection> Pool { get; }
        SqlConnection CreateIfNotExists(string nameOrConnectionString);
    }
}
