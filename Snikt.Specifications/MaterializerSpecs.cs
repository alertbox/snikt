using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using Snikt.Specifications.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace Snikt.Specifications
{
    [TestClass]
    public class MaterializerSpecs
    {
        private SqlConnection connection;
        private SqlCommand command;
        private Materializer<Category> categoryMaterializer;
        private IDataReader queryResult;
        private ICollection<Category> categories;

        [TestMethod]
        public void CreateMaterializerSpec()
        {
            // Build
            CreateNewDbConnection();

            // Operator
            QueryAllCategories();
            CreateMaterializer();

            // Check
            AssertCategoryMaterializerInitialized();
        }

        [TestMethod]
        public void MaterializeCategoriesSpec()
        {
            // Build
            CreateNewDbConnection();

            // Operator
            QueryAllCategories();
            CreateMaterializer();
            MaterializeCategories();

            // Check
            AssertCategoriesAreNotEmpty();
            AssertCategoriesAreMaterialized();
        }

        #region Specification Helpers - Build methods

        private void CreateNewDbConnection()
        {
            connection = SqlConnectionFactory.Get().CreateIfNotExists("name=DefaultConnection");
        }

        #endregion // Specification Helpers - Build methods

        #region Specification Helpers - Operator methods

        private void QueryAllCategories()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "dbo.GetAllCategories";
            connection.OpenIfNot();
            queryResult = command.ExecuteReader();
        }

        private void CreateMaterializer()
        {
            categoryMaterializer = new Materializer<Category>(queryResult);
        }

        private void MaterializeCategories()
        {
            categories = new List<Category>();
            while (queryResult.Read())
            {
                categories.Add(categoryMaterializer.Materialize(queryResult));
            }
        }

        #endregion // Specification Helpers - Operator methods

        #region Specification Helpers - Check methods

        private void AssertCategoryMaterializerInitialized()
        {
            Assert.IsInstanceOfType(categoryMaterializer, typeof(Materializer<Category>));
        }

        private void AssertCategoriesAreNotEmpty()
        {
            Assert.IsTrue(categories.Any());
        }

        private void AssertCategoriesAreMaterialized()
        {
            foreach (Category cat in categories)
            {
                AssertPropertiesAreNotNull(cat.GetType(), cat);
            }
        }

        private void AssertPropertiesAreNotNull(Type t, object obj)
        {
            t.GetProperties()
                .Select(property => property.GetValue(obj, null))
                .ToList()
                .ForEach(value => Assert.IsNotNull(value));
        }

        #endregion // Specification Helpers - Check methods
    }
}
