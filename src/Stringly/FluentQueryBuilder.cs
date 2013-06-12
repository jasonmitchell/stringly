using System;
using System.Linq;
using Stringly.Metadata;
using Stringly.Queries;

namespace Stringly
{
    public class FluentQueryBuilder
    {
        private readonly string connectionString;
        private readonly QueryMetadata metadata;

        public static FluentQueryBuilder Query(string connectionString, string tableName)
        {
            QueryMetadata metadata = new QueryMetadata(tableName);
            FluentQueryBuilder queryBuilder = new FluentQueryBuilder(connectionString, metadata);

            return queryBuilder;
        }

        private FluentQueryBuilder(string connectionString, QueryMetadata metadata)
        {
            this.connectionString = connectionString;
            this.metadata = metadata;
        }

        public FluentQueryBuilder Join(string tableName, string primaryKey, string foreignKey)
        {
            JoinMetadata join = new JoinMetadata(tableName, primaryKey, foreignKey);
            metadata.AddJoin(join);

            return this;
        }

        public FluentQueryBuilder Where(string fieldName, string comparisonOperation, string value)
        {
            ComparisonOperation strongComparisonOperation = (ComparisonOperation) Enum.Parse(typeof (ComparisonOperation), comparisonOperation);
            return Where(fieldName, strongComparisonOperation, value);
        }

        public FluentQueryBuilder Where(string fieldName, ComparisonOperation comparisonOperation, string value)
        {
            ConditionMetadata condition = new ConditionMetadata(fieldName, comparisonOperation, value);
            metadata.AddCondition(condition);

            return this;
        }

        public FluentQueryBuilder Select(string fieldName)
        {
            return Select(fieldName, fieldName.Replace('.', '_'));
        }

        public FluentQueryBuilder Select(string fieldName, string displayName)
        {
            SelectMetadata select = new SelectMetadata(fieldName, displayName);
            metadata.AddSelect(select);

            return this;
        }

        public FluentQueryBuilder OrderBy(string fieldName, string isAscending)
        {
            bool strongIsAscending = bool.Parse(isAscending);
            return OrderBy(fieldName, strongIsAscending);
        }

        public FluentQueryBuilder OrderBy(string fieldName, bool isAscending)
        {
            OrderingMetadata ordering = new OrderingMetadata(fieldName, isAscending);
            metadata.AddOrdering(ordering);

            return this;
        }

        public FluentQueryBuilder Page(int currentPage, int recordsPerPage)
        {
            metadata.Paging = new PagingMetadata(currentPage, recordsPerPage);
            return this;
        }

        public IDynamicQuery Compile()
        {
            if(!metadata.Selects.Any())
                throw new InvalidOperationException("No select fields have been specified.");

            return new SqlQuery(connectionString, metadata);
        }
    }
}