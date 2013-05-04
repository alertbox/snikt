using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace Snikt.Specifications.ConnectionFactorySpecs
{
    [TestClass]
    public class WhenTwoConnections
    {
        [TestMethod]
        public void ThenConnection1And2AreSame()
        {
            // Build
            IDbConnectionFactory factory = DbConnectionFactory.Get();
            string nameOrConnectionString = "Data Source=(LocalDb)\v11.0;Initial Catalog=aspnet-CodeCamper.Web-20120601131139;";

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);
            IDbConnection connection2 = factory.CreateIfNotExists(nameOrConnectionString);

            // Check
            Assert.AreEqual<IDbConnection>(connection1, connection2);
        }

        [TestMethod]
        public void ThenConnection2IsPooled()
        {
            // Build
            IDbConnectionFactory factory = DbConnectionFactory.Get();
            string nameOrConnectionString = "Data Source=(LocalDb)\v11.0;Initial Catalog=aspnet-CodeCamper.Web-20120601131139;";

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);
            IDbConnection connection2 = factory.CreateIfNotExists(nameOrConnectionString);

            // Check
            Assert.IsTrue(factory.Pool.Count == 1);
            Assert.AreEqual<IDbConnection>(factory.Pool[nameOrConnectionString], connection2);
        }
    }
}
