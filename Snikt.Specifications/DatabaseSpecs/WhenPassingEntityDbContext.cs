using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snikt.Specifications.Mocks.EntityFramework;
using System.Collections.Generic;
using Snikt.Specifications.Mocks.Poco;
using System.Linq;

namespace Snikt.Specifications.DatabaseSpecs
{
    [TestClass]
    public class WhenPassingEntityDbContext
    {
        [TestMethod]
        public void ThenCreateDatabaseInstance()
        {
            // Build
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString, DbContextConnectionFactory.Get());

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
            IDatabase db = new Database(nameOrConnectionString, DbContextConnectionFactory.Get());
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
