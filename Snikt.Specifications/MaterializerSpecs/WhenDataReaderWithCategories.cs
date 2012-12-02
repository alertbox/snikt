using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using Snikt.Specifications.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace Snikt.Specifications.MaterializerSpecs
{
    [TestClass]
    public class WhenDataReaderWithCategories
    {
        [TestMethod]
        public void ThenCategoryShaperIsCreated()
        {
            // Build
            SqlConnection connection = SqlConnectionFactory.Get().CreateIfNotExists("name=DefaultConnection");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "dbo.GetAllCategories";

            // Operator
            connection.OpenIfNot();
            IDataReader queryResult = command.ExecuteReader();
            Materializer<Category> categoryMaterializer = new Materializer<Category>(queryResult);

            // Check
            Assert.IsInstanceOfType(categoryMaterializer, typeof(Materializer<Category>));
        }

        [TestMethod]
        public void ThenCollectionOfCategoriesReturned()
        {
            // Build
            SqlConnection connection = SqlConnectionFactory.Get().CreateIfNotExists("name=DefaultConnection");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "dbo.GetAllCategories";

            // Operator
            connection.OpenIfNot();
            IDataReader queryResult = command.ExecuteReader();
            Materializer<Category> categoryMaterializer = new Materializer<Category>(queryResult);
            List<Category> categories = new List<Category>();
            while (queryResult.Read())
            {
                categories.Add(categoryMaterializer.Materialize(queryResult));
            }

            // Check
            Assert.IsTrue(categories.Any());
        }

        [TestMethod]
        public void ThenEachCategoryContainsData()
        {
            // Build
            SqlConnection connection = SqlConnectionFactory.Get().CreateIfNotExists("name=DefaultConnection");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "dbo.GetAllCategories";

            // Operator
            connection.OpenIfNot();
            IDataReader queryResult = command.ExecuteReader();
            Materializer<Category> categoryMaterializer = new Materializer<Category>(queryResult);
            List<Category> categories = new List<Category>();
            while (queryResult.Read())
            {
                categories.Add(categoryMaterializer.Materialize(queryResult));
            }
            foreach (Category cat in categories)
            {
                AssertPropertiesAreNotNull(cat.GetType(), cat);
            }

        }

        #region Helper Methods

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
