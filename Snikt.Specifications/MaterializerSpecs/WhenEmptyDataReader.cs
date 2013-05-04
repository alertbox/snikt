using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using Snikt.Specifications.Mocks.Poco;
using System.Collections.Generic;
using System.Linq;

namespace Snikt.Specifications.MaterializerSpecs
{
    [TestClass]
    public class WhenEmptyDataReader
    {
        private const int SomeCategoryId = 5;

        [TestMethod]
        public void ThenEmptyCategoryCollectionIsReturned()
        {
            // Build
            SqlConnection connection = (SqlConnection)DbConnectionFactory.Get().CreateIfNotExists("name=DefaultConnection");
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetCategory";
            command.Parameters.AddWithValue("Id", SomeCategoryId);

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
            Assert.IsTrue(categories.Count() == 0);
        }
    }
}
