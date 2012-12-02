using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace Snikt.Specifications.ConnectionFactorySpecs
{
    [TestClass]
    public class WhenPassingConnectionString
    {
        [TestMethod]
        public void ThenCreateNewDatabaseConnection()
        {
            // Build
            ISqlConnectionFactory factory = SqlConnectionFactory.Get();
            string nameOrConnectionString = "Data Source=(LocalDb)\v11.0;Initial Catalog=aspnet-CodeCamper.Web-20120601131139;";

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);

            // Check
            Assert.IsInstanceOfType(connection1, typeof(SqlConnection));
        }
    }
}
