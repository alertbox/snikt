using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace Snikt.Specifications.ConnectionFactorySpecs
{
    [TestClass]
    public class WhenPassingDefaultName
    {
        [TestMethod]
        public void ThenCreateNewDatabaseConnection()
        {
            // Build
            ISqlConnectionFactory factory = SqlConnectionFactory.Get();
            string nameOrConnectionString = "name=DefaultConnection";

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);

            // Check
            Assert.IsInstanceOfType(connection1, typeof(SqlConnection));
        }
    }
}
