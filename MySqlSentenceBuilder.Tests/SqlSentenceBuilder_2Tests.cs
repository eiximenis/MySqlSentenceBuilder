using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlSentenceBuilder.Tests.Extensions;
using MySqlSentenceBuilder.Tests.Tables;

namespace MySqlSentenceBuilder.Tests
{
    [TestClass]
    public class SqlSentenceBuilder_2Tests
    {
        [TestMethod]
        public void TwoTablesJoinWithoutRightColumns()
        {
            var builder = new SqlSentenceBuilder<Customer, Product>().
                WithNoRightColumns().
                Join(c => c.Id, p => p.CustomerId);

            var sql = builder.ToSql();
            var expected =
                "select Customer.Id, Customer.Name, Customer.LastName from Customer INNER join Product on Customer.Id = Product.CustomerId";

            Assert.AreEqual(expected.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }
    }
}
