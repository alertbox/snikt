using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snikt.Specifications.Mocks.LinqToSql;
using System.Collections.Generic;
using System.Linq;

namespace Snikt.Specifications.DatabaseSpecs
{
    [TestClass]
    public class WhenPassingLinqDataContext
    {
        [TestMethod]
        public void ThenCreateDatabaseInstance()
        {
            // Build
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString, LinqDataContextConnectionFactory.Get());

            // Operator

            // Check
            Assert.IsNotNull(db);
            Assert.IsNotNull(db.ConnectionFactory);
            Assert.IsNotNull(db.Connection);
        }


        [TestMethod]
        public void ThenMatchedResultsAreReturned()
        {
            // Biuld
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString, LinqDataContextConnectionFactory.Get());
            var criteria = new { Id = 1 };

            // Operator
            List<Category> categories = db.SqlQuery<Category>("dbo.GetCategory", criteria).ToList();

            // Check
            foreach (Category cat in categories)
            {
                Assert.AreEqual(criteria.Id, cat.Id);
            }
        }

    }
}
