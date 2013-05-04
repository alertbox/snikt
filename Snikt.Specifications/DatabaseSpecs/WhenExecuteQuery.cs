using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snikt.Specifications.Mocks.Poco;
using System.Collections.Generic;
using System.Linq;

namespace Snikt.Specifications.DatabaseSpecs
{
    [TestClass]
    public class WhenExecuteQuery
    {
        [TestMethod]
        public void ThenStrongTypedListIsReturned()
        {
            // Biuld
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString);

            // Operator
            List<Category> categories = db.SqlQuery<Category>("dbo.GetAllCategories").ToList();

            // Check
            AssertListNotEmpty(categories);
        }

        [TestMethod]
        public void ThenMapResultToStrongTypedList()
        {
            // Biuld
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString);

            // Operator
            List<Category> categories = db.SqlQuery<Category>("dbo.GetAllCategories").ToList();

            // Check
            foreach (Category cat in categories)
            {
                AssertPropertiesAreNotNull(cat.GetType(), cat);
            }
        }

        [TestMethod]
        public void ThenMatchedResultsAreReturned()
        {
            // Biuld
            string nameOrConnectionString = "name=DefaultConnection";
            IDatabase db = new Database(nameOrConnectionString);
            var criteria = new { Id = 1 };

            // Operator
            List<Category> categories = db.SqlQuery<Category>("dbo.GetCategory", criteria).ToList();

            // Check
            foreach (Category cat in categories)
            {
                Assert.AreEqual(criteria.Id, cat.Id);
            }
        }

        #region Helper Methods

        private void AssertListNotEmpty<T>(IEnumerable<T> list)
        {
            Assert.IsTrue(list.Any());
        }

        private void AssertPropertiesAreNotNull(Type t, object obj)
        {
            t.GetProperties()
                .Select(property => property.GetValue(obj, null))
                .ToList()
                .ForEach(value => Assert.IsNotNull(value));
        }

        #endregion // Helper Methods

    }
}
