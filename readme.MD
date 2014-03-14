MySql Sentence Builders
=======================

This project contains some classes to help generate SQL SELECT sentences for MySql Database.  
Tf you use any micro-ORM like Dapper one of the most tedious things is to write manually the SQL Sentences.  
Classes on this project allows writing simple SELECT sentences using fluent interface and entity classes.  

Provided are three classes for helping you on this:

- SqlSentenceBuilder&lt;T&gt;: To build sentences involving on table
- SqlSentenceBuilder&lt;TL,TR&gt;: To build sentences involving two tables (i. e. joins)
- MultiSqlSentenceBuilder: To build sentences involving any number of tables.

Use is very easy as three classes exposes a similar (but not identical) fluent interface.  
Some basic samples:

Select some columns
--------------------
        var builder = new SqlSentenceBuilder<Customer>();
        builder.WithColumns(c => c.Id, c => c.LastName);
        var sql = builder.ToSql();

Generates: select Customer.Id, Customer.LastName from Customer

Filtering
---------
        var builder = new SqlSentenceBuilder<Customer>();
        builder.WithColumns(c => c.Id, c => c.LastName).WhereParameters(c => c.Id);
        var sql = builder.ToSql();

Generates: select Customer.Id, Customer.LastName from Customer where Customer.Id = @Id

Joining
-------
        var builder = new MultiSqlSentenceBuilder();
        builder.WithTable<Customer>().WithColumns(t => t.Id, t => t.LastName, t => t.Name);
        builder.WithTable<Product>().WithColumns(p => p.Id, p => p.Name);
        builder.Join<Customer, Product>(c => c.Id, p => p.CustomerId);
        var sql = builder.ToSql();

Generates: select Customer.Id, Customer.LastName, Customer.Name, Product.Id, Product.Name from Customer,Product INNER join Product on Customer.Id = Product.CustomerId 

        var builder = new MultiSqlSentenceBuilder();
        builder.WithTable<Customer>().WithColumns(t => t.Id, t => t.LastName, t => t.Name).
            WithTable<Product>().WithColumns(p => p.Id, p => p.Name).
            WhereParameters<Product>(WhereSpecOperator.GT, p => p.CustomerId).
            Join<Customer, Product>(c => c.Id, p => p.CustomerId);
        var sql = builder.ToSql();

Generates: select  Customer.Id, Customer.LastName, Customer.Name, Product.Id,Product.Name from Customer, Product INNER join Product on Customer.Id = Product.CustomerId  WHERE Product.CustomerId &gt; @CustomerId

Joining two tables but only selecting fields of one
----------------------------------------------------
        var builder = new SqlSentenceBuilder<Customer, Product>().
            WithNoRightColumns().
            Join(c => c.Id, p => p.CustomerId);

Generates: select Customer.Id, Customer.Name, Customer.LastName from Customer INNER join Product on Customer.Id = Product.CustomerId

If you prefer, can use MultiSqlSentenceBuilder for generating the same select:

        var builder = new MultiSqlSentenceBuilder().
            WithFullTable<Customer>().
            Join<Customer, Product>(c => c.Id, p => p.CustomerId);

