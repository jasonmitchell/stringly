using Stringly.Queries;

namespace Stringly
{
    public class FluentSqlQueryBuilder : AbstractFluentQueryBuilder
    {
        public static FluentSqlQueryBuilder Query(string connectionString, string tableName)
        {
            return new FluentSqlQueryBuilder(connectionString, tableName);
        }

        private FluentSqlQueryBuilder(string connectionString, string tableName) : base(connectionString, tableName) {}
        
        public override IDynamicQuery Compile()
        {
            AssertMetadataIsValid();
            return new SqlQuery(ConnectionString, Metadata);
        }
    }
}
