using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlSentenceBuilder.Tests.Extensions;
using MySqlSentenceBuilder.Tests.Tables;

namespace MySqlSentenceBuilder.Tests
{
    [TestClass]
    public class MultiSqlSentenceBuilderTests
    {
        [TestMethod]
        public void TwoTablesWithInnerJoin()
        {
            var builder = new MultiSqlSentenceBuilder();
            builder.WithTable<Customer>().WithColumns(t => t.Id, t => t.LastName, t => t.Name);
            builder.WithTable<Product>().WithColumns(p => p.Id, p => p.Name);
            builder.Join<Customer, Product>(c => c.Id, p => p.CustomerId);
            var sql = builder.ToSql();
            var expectedSql =
                "select Customer.Id, Customer.LastName, Customer.Name, Product.Id, Product.Name from Customer,Product INNER join Product on Customer.Id = Product.CustomerId";
            Assert.AreEqual(expectedSql.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }

        [TestMethod]
        public void TwoTablesWithInnerAndWhere()
        {
            var builder = new MultiSqlSentenceBuilder();
            builder.WithTable<Customer>().WithColumns(t => t.Id, t => t.LastName, t => t.Name).
                WithTable<Product>().WithColumns(p => p.Id, p => p.Name).
                WhereParameters<Product>(WhereSpecOperator.GT, p => p.CustomerId).
                Join<Customer, Product>(c => c.Id, p => p.CustomerId);
            var sql = builder.ToSql();
            var expectedSql =
                "select  Customer.Id, Customer.LastName, Customer.Name, Product.Id,Product.Name from Customer, Product INNER join Product on Customer.Id = Product.CustomerId  WHERE Product.CustomerId > @CustomerId";
            Assert.AreEqual(expectedSql.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }

        [TestMethod]
        public void TwoTablesJoinWithoutRightColumns()
        {
            var builder = new MultiSqlSentenceBuilder().
                WithFullTable<Customer>().
                Join<Customer, Product>(c => c.Id, p => p.CustomerId);
            var sql = builder.ToSql();
            var expected =
                "select Customer.Id, Customer.Name, Customer.LastName from Customer INNER join Product on Customer.Id = Product.CustomerId";
                Assert.AreEqual(expected.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }

        [TestMethod]
        public void TwoTablesWithInnerJoinAndOrderBy()
        {
            var builder = new MultiSqlSentenceBuilder();
            builder.WithTable<Customer>().WithColumns(t => t.Id, t => t.LastName, t => t.Name);
            builder.WithTable<Product>().WithColumns(p => p.Id, p => p.Name);
            builder.Join<Customer, Product>(c => c.Id, p => p.CustomerId);
            var sql = builder.ToSql();
            var expectedSql =
                "select Customer.Id, Customer.LastName, Customer.Name, Product.Id, Product.Name from Customer, Product INNER join Product on Customer.Id = Product.CustomerId";
            Assert.AreEqual(expectedSql.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }

        [TestMethod]
        public void SelectWithColumnsSelectsOnlyTheseColumns()
        {
            var builder = new MultiSqlSentenceBuilder();
            builder.WithTable<Customer>().WithColumns(c => c.Id, c => c.LastName);
            var sql = builder.ToSql();
            var expectedSql = "select Customer.Id, Customer.LastName from Customer";
            Assert.AreEqual(expectedSql.TrimEveryWhere(), sql.TrimEveryWhere(), true);
        }

        [TestMethod]
        public void SelectWithNoColumnsSelectsAllTheColumns()
        {
            var builder = new MultiSqlSentenceBuilder();
            builder.WithTable<Customer>();
            var sql = builder.ToSql();
            var expected = "select Customer.Id, Customer.Name, Customer.LastName from Customer";
            Assert.AreEqual(expected.TrimEveryWhere(), sql.TrimEveryWhere(), true);

        }


    }
}
