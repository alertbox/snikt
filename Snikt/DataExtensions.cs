using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Snikt
{
    public static class DataExtensions
    {
        public static void CloseIfNot(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public static void OpenIfNot(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public static T Field<T>(this IDataRecord record, int ordinal)
        {
            object val = record.IsDBNull(ordinal) ? null : record.GetValue(ordinal);
            return (T)val;
        }

        public static IEnumerable<string> GetFieldNames(this IDataReader reader)
        {
            return Enumerable.Range(0, reader.FieldCount)
                .Select(ordinal => reader.GetName(ordinal));
        }
    }
}
