using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Snikt.Specifications.ConnectionFactorySpecs.SadPath
{
    [TestClass]
    public class WhenConnectionNameIsWrong
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenThrowAnException()
        {
            // Build
            IDbConnectionFactory factory = DbConnectionFactory.Get();
            string nameOrConnectionString = "name=WrongConnection";

            // Operator
            IDbConnection connection1 = factory.CreateIfNotExists(nameOrConnectionString);
        }
    }
}
