using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using Snikt.Specifications.Mocks;

namespace Snikt.Specifications
{
    [TestClass]
    public class DatabaseSpecs
    {
        private string nameOrConnectionString;
        private ISqlConnectionFactory factory;
        private Database db;
        private IEnumerable<Category> categories;

        [TestMethod]
        public void CreateDatabaseSpec()
        {
            // Build
            CreateDefaultConnectionName();
            CreateNewDbUsingConnectionString();

            // Operator
            // HINT: Nothing to operate here.

            // Check
            AssertDbCanBeUsed();
        }

        [TestMethod]
        public void CreateDatabaseSpec2()
        {
            // Build
            CreateDefaultConnectionName();
            CreateNewFactory();
            CreateNewDb();

            // Operator
            // HINT: Nothing to operate here.

            // Check
            AssertDbUsingConnectionFactory();
        }

        [TestMethod]
        public void QueryWithoutFiltersSpec()
        {
            // Build
            CreateDefaultConnectionName();
            CreateNewFactory();
            CreateNewDb();

            // Operator
            QueryAllCategories();

            // Check
            AssertCategoryListNotEmpty();
        }

        #region Specification Helpers - Build methods

        private void CreateDefaultConnectionName()
        {
            ChangeConnectionStringTo("name=DefaultConnection");
        }

        private void ChangeConnectionStringTo(string nameOrString)
        {
            nameOrConnectionString = nameOrString;
        }

        private void CreateNewFactory()
        {
            factory = SqlConnectionFactory.Get();
        }

        private void CreateNewDbUsingConnectionString()
        {
            db = new Database(nameOrConnectionString);
        }

        private void CreateNewDb()
        {
            db = new Database(nameOrConnectionString, factory);
        }

        #endregion // Specification Helpers - Build methods

        #region Specification Helpers - Operator methods

        private void QueryAllCategories()
        {
            categories = db.SqlQuery<Category>("dbo.GetAllCategories").ToList();
        }

        #endregion // Specification Helpers - Operator methods

        #region Specification Helpers - Check methods

        private void AssertDbCanBeUsed()
        {
            Assert.IsInstanceOfType(db.Connection, typeof(SqlConnection));
            Assert.IsInstanceOfType(db.ConnectionFactory, typeof(ISqlConnectionFactory));
        }

        private void AssertDbUsingConnectionFactory()
        {
            Assert.AreEqual<ISqlConnectionFactory>(db.ConnectionFactory, factory);
        }

        private void AssertCategoryListNotEmpty()
        {
            Assert.IsTrue(categories.Any());
        }

        #endregion // Specification Helpers - Check methods
    }
}
