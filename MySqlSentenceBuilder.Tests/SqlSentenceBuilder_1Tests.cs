using System;
using System.Windows.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlSentenceBuilder.Tests.Extensions;
using MySqlSentenceBuilder.Tests.Tables;

namespace MySqlSentenceBuilder.Tests
{
    [TestClass]
    public class SqlSentenceBuilder_1Tests
    {
        [TestMethod]
        public void SelectSpecificColumnsTest()
        {
            var builder = new SqlSentenceBuilder<Customer>();
            builder.WithColumns(c => c.Id, c => c.LastName);
            var sql = builder.ToSql();
            var expected = "select Customer.Id, Customer.LastName from Customer";
            Assert.AreEqual(expected.TrimEveryWhere(), sql.TrimEveryWhere());
        }

        [TestMethod]
        public void SelectWillSelectAllColumnsIfNoColumnsAreSpecified()
        {
            var builder = new SqlSentenceBuilder<Customer>();
            var sql = builder.ToSql();
            var expected = "select Customer.Id, Customer.Name, Customer.LastName from Customer";
            Assert.AreEqual(expected.TrimEveryWhere(), sql.TrimEveryWhere());
        }

        [TestMethod]
        public void WhereGeneratesWhereClausule()
        {
            var builder = new SqlSentenceBuilder<Customer>();
            builder.WithColumns(c => c.Id, c => c.LastName).
                WhereParameters(c => c.Id);
            var sql = builder.ToSql();
            var expected = "select Customer.Id, Customer.LastName from Customer where Customer.Id = @Id";
            Assert.AreEqual(expected.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }
    }
}
