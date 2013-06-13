using Stringly.Queries;

namespace Stringly
{
    public interface IFluentQueryBuilder
    {
        IFluentQueryBuilder Join(string tableName, string primaryKey, string foreignKey);
        IFluentQueryBuilder Where(string fieldName, string comparisonOperation, string value);
        IFluentQueryBuilder Where(string fieldName, ComparisonOperation comparisonOperation, string value);
        IFluentQueryBuilder Select(string fieldName);
        IFluentQueryBuilder Select(string[] fieldNames);
        IFluentQueryBuilder Select(string fieldName, string displayName);
        IFluentQueryBuilder OrderBy(string fieldName, string isAscending);
        IFluentQueryBuilder OrderBy(string fieldName, bool isAscending);
        IFluentQueryBuilder Page(int currentPage, int recordsPerPage);
        IDynamicQuery Compile();
    }
}
