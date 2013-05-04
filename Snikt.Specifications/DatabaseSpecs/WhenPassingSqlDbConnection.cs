using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Snikt.Specifications.DatabaseSpecs
{
    [TestClass]
    public class WhenPassingSqlDbConnection
    {
        [TestMethod]
        public void ThenCreateDatabaseInstance()
        {
            // Build
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString, DbConnectionFactory.Get());

            // Operator

            // Check
            Assert.IsNotNull(db);
            Assert.IsNotNull(db.ConnectionFactory);
            Assert.IsNotNull(db.Connection);
        }
    }
}
