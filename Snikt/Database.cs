using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Snikt
{
    public class Database : IDisposable
    {
        public ISqlConnectionFactory ConnectionFactory { get; private set; }
        public SqlConnection Connection { get; private set; }

        public Database(string nameOrConnectionString) 
            : this(nameOrConnectionString, CreateDefaultConnectionFactory())
        {
            // HINT: Nothing to do here.
        }

        public Database(string nameOrConnectionString, ISqlConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
            Connection = ConnectionFactory.CreateIfNotExists(nameOrConnectionString);
        }

        public static ISqlConnectionFactory CreateDefaultConnectionFactory()
        {
            return SqlConnectionFactory.Get();
        }

        public IEnumerable<T> SqlQuery<T>(string sql) where T : class, new()
        {
            return QueryInternal<T>(sql, null);
        }

        public IEnumerable<T> SqlQuery<T>(string sql, object parameters) where T : class, new()
        {
            //using (SqlCommand command = Connection.CreateCommand())
            //{
            //    var parameterArray = parameters.GetType().GetProperties().Select(property => new { Name = property.Name, Value = property.GetValue(parameters, null) }).ToArray();
            //    foreach (var parameter in parameterArray)
            //    {
            //        command.Parameters.AddWithValue(parameter.Name, parameter.Value);
            //    }
            //    command.CommandText = sql;
            //    command.CommandType = CommandType.StoredProcedure;
            //    Connection.OpenIfNot();
            //    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            //    {
            //        while (reader.Read())
            //        {
            //            yield return new T();
            //        }
            //    }
            //}
            return QueryInternal<T>(sql, parameters);
        }

        internal IEnumerable<T> QueryInternal<T>(string sql, object parameters) where T : class
        {
            //Action<SqlCommand, object> paramReader = (command, obj) => 
            //{
            //    DynamicParameters mappee = new DynamicParameters(obj);
            //    mappee.AddParameters(command, obj);
            //};

            using (SqlCommand command = SetupStoreCommand(null, sql, null, parameters, null, null))
            {
                Connection.OpenIfNot();
                using (IDataReader reader = command.ExecuteReader())
                {
                    Materializer<T> mapper = new Materializer<T>(reader);
                    while (reader.Read())
                    {
                        yield return (T)mapper.Materialize(reader);
                    }
                }
            }
        }

        private SqlCommand SetupStoreCommand(SqlTransaction transaction, string sql, Action<SqlCommand, object> paramReader, object obj, int? commandTimeout, CommandType? commandType)
        {
            SqlCommand command = Connection.CreateCommand();
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            if (commandTimeout.HasValue)
            {
                command.CommandTimeout = commandTimeout.Value;
            }
            if (commandType.HasValue)
            {
                command.CommandType = commandType.Value;
            }
            command.CommandText = sql;
            if (paramReader != null)
            {
                paramReader(command, obj);
            }

            return command;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (Connection)
                {
                    Connection.CloseIfNot();
                }
            }
        }
    }
}
