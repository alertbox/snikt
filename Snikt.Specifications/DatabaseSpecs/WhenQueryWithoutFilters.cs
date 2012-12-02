using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snikt.Specifications.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace Snikt.Specifications.DatabaseSpecs
{
    [TestClass]
    public class WhenQueryWithoutFilters
    {
        [TestMethod]
        public void ThenCategoriesAreListed()
        {
            // Biuld
            string nameOrConnectionString = "name=DefaultConnection";
            Database db = new Database(nameOrConnectionString);

            // Operator
            List<Category> categories = db.SqlQuery<Category>("dbo.GetAllCategories").ToList();

            // Check
            Assert.IsTrue(categories.Count() > 0);
        }
    }
}
