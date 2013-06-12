Stringly
========

Stringly is a library for dynamically building queries from metadata supplied by an external source at runtime.  This is useful for scenarios where you don't know the structure of queries at compile-time but you need to provide a method of dynamically creating queries at run-time based on some external input; e.g. an application that allows users to create custom reports for a database through a UI such as a web page.  Stringly works by building a collection of metadata relating to a query which it uses to generate the query itself which can then be executed to return a DataTable.  **Strong typing of results hopefully coming in the future.**

Supported Query Methods
-----------------------
Stringly currently only supports dynamic generation of SQL queries.  I'm intending to support generation of LINQ queries in the future to enable support for dynamic query building for frameworks with LINQ providers (LINQ to SQL, Entity Framework, NHibernate).

Example
-------

Stringly uses a fluent API for building queries which allows developers to chain method calls together when constructing queries:

    DataTable results = FluentSqlQueryBuilder.Query(connectionString, "Users")
                                             .Join("Organisations", "Organisations.Id", "Users.OrganisationId")
                                             .Where("Users.FirstName", ComparisonOperation.Equals, "Jason")
                                             .Select("Users.FirstName")
                                             .Select("Users.LastName")
                                             .Select("Users.Username")
                                             .Select("Organisations.Name", "OrganisationName")
                                             .Select("Organisations.CreatedDate")
                                             .OrderBy("Organisations.Name", true)
                                             .Page(1, 100)
                                             .Compile()
                                             .Execute();
                                          
This query will generate the following (semi-tidy) SQL:

    WITH QueryPage AS
    (
    SELECT ROW_NUMBER() OVER (ORDER BY Organisations.Name ASC) AS RowNumber, Users.FirstName AS [Users_FirstName], Users.LastName AS [Users_LastName], Users.Username AS [Users_Username], Organisations.Name AS [OrganisationName], Organisations.CreatedDate AS [Organisations_CreatedDate]
    FROM [Users]
    JOIN [Organisations] ON Organisations.Id = Users.OrganisationId
    WHERE Users.FirstName = 'Jason'
    )
    SELECT *
    FROM QueryPage
    WHERE RowNumber > 0 AND RowNumber <= 100
    ORDER BY OrganisationName ASC
    
Stringly generates SQL queries as Common Table Expressions in order to enable paging of results using the ROW_NUMBER() function.  This SQL should work or SQL Server 2005 and above.

Previewing generated SQL
------------------------

In order to preview the SQL generated by Stringly you need to cast the object returned by the Compile() method to SqlQuery and inspect the GeneratedSql property.  Example:

    IDynamicQuery query = FluentQueryBuilder.Query(connectionString, "Users")
                                            .Where("Users.Name", ComparisonOperation.Equals, "Jason")
                                            .Select("Users.FirstName")
                                            .Compile();

    string generatedSql = ((SqlQuery) query).GeneratedSql;
    
Contributions are welcome!
-------------------------
                                          
