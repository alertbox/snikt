using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace Snikt.Specifications
{
    [TestClass]
    public class ConnectionFactorySpecs
    {
        private string nameOrConnectionString;
        private ISqlConnectionFactory factory;
        private IDbConnection connection1;
        private IDbConnection connection2;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowIfNullOrEmptyConnectionNameSpec()
        {
            // Build
            CreateNewFactory();
            CreateEmptyConnectionString();

            // Operator
            CreateNewConnection1();

            // Check
            // HINT: This specification must throw an exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowIfGibrishConnectionStringSpec()
        {
            // Build
            CreateNewFactory();
            CreateGibrishConnectionString();

            // Operator
            CreateNewConnection1();

            // Check
            // HINT: This specification must throw an exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowIfWrongConnectionNameSpec()
        {
            // Build
            CreateNewFactory();
            CreateWrongConnectionName();

            // Operator
            CreateNewConnection1();

            // Check
            // HINT: This specification must throw an exception.
        }

        [TestMethod]
        public void CreateSqlConnectionUsingDefaultNameSpec()
        {
            // Build
            CreateNewFactory();
            CreateDefaultConnectionName();

            // Operator
            CreateNewConnection1();

            // Check
            AssertConnection1IsSql();
        }

        [TestMethod]
        public void CreateSqlConnectionWithConnectionStringSpec()
        {
            // Build
            CreateNewFactory();
            CreateValidSqlConnectionString();

            // Operator
            CreateNewConnection1();

            // Check
            AssertConnection1IsSql();
        }

        [TestMethod]
        public void CreateSqlConnection2UsingConnectionPoolSpec()
        {
            // Build
            CreateNewFactory();
            CreateDefaultConnectionName();

            // Operator
            CreateNewConnection1And2();

            // Check
            AssertConnection2WasPooled();
        }

        #region Specification Helpers - Build methods

        private void CreateNewFactory()
        {
            factory = SqlConnectionFactory.Get();
        }

        private void CreateEmptyConnectionString()
        {
            ChangeConnectionStringTo(string.Empty);
        }

        private void CreateGibrishConnectionString()
        {
            ChangeConnectionStringTo("Gibrish connection string");
        }

        private void CreateWrongConnectionName()
        {
            ChangeConnectionStringTo("name=WrongConnection");
        }

        private void CreateDefaultConnectionName()
        {
            ChangeConnectionStringTo("name=DefaultConnection");
        }

        private void CreateValidSqlConnectionString()
        {
            ChangeConnectionStringTo("Data Source=(LocalDb)\v11.0;Initial Catalog=aspnet-CodeCamper.Web-20120601131139;");
        }

        private void ChangeConnectionStringTo(string nameOrString)
        {
            nameOrConnectionString = nameOrString;
        }

        #endregion // Specification Helpers - Build methods

        #region Specification Helpers - Operator methods

        private void CreateNewConnection1And2()
        {
            CreateNewConnection1();
            CreateNewConnection2();
        }

        private void CreateNewConnection1()
        {
            CreateConnection1Using(nameOrConnectionString);
        }

        private void CreateNewConnection2()
        {
            CreateConnection2Using(nameOrConnectionString);
        }

        private void CreateConnection1Using(string nameOrString)
        {
            connection1 = factory.CreateIfNotExists(nameOrString);
        }

        private void CreateConnection2Using(string nameOrString)
        {
            connection2 = factory.CreateIfNotExists(nameOrString);
        }

        #endregion // Specification Helpers - Operator methods

        #region Specification Helpers - Check methods

        private void AssertConnection1IsSql()
        {
            Assert.IsInstanceOfType(connection1, typeof(SqlConnection));
        }

        private void AssertConnection2WasPooled()
        {
            Assert.AreEqual<IDbConnection>(connection1, connection2);
        }

        #endregion // Specification Helpers - Check methods
    }
}
