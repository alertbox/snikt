using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Snikt.Specifications.ConnectionFactorySpecs.SadPath
{
    [TestClass]
    public class WhenConnectionStringIsNullOrEmpty
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenThrowArgumentNullException()
        {
            // Build
            IDbConnectionFactory factory = DbConnectionFactory.Get();
            string nameOrConnectionString = string.Empty;

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);
        }
    }
}
