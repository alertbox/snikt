using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Snikt.Specifications.ConnectionFactorySpecs.SadPath
{
    [TestClass]
    public class WhenConnectionStringIsGibrish
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenThrowArgumentNullException()
        {
            // Build
            IDbConnectionFactory factory = DbConnectionFactory.Get();
            string nameOrConnectionString = "Gibrish connection string";

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);
        }
    }
}
