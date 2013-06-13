using System;
using System.Linq;
using Stringly.Metadata;
using Stringly.Queries;

namespace Stringly
{
    public abstract class AbstractFluentQueryBuilder : IFluentQueryBuilder
    {
        private readonly string connectionString;
        private readonly QueryMetadata metadata;

        protected AbstractFluentQueryBuilder(string connectionString, string tableName)
        {
            this.connectionString = connectionString;
            metadata = new QueryMetadata(tableName);
        }

        public IFluentQueryBuilder Join(string tableName, string primaryKey, string foreignKey)
        {
            JoinMetadata join = new JoinMetadata(tableName, primaryKey, foreignKey);
            metadata.AddJoin(join);

            return this;
        }

        public IFluentQueryBuilder Where(string fieldName, string comparisonOperation, string value)
        {
            ComparisonOperation strongComparisonOperation = (ComparisonOperation) Enum.Parse(typeof (ComparisonOperation), comparisonOperation);
            return Where(fieldName, strongComparisonOperation, value);
        }

        public IFluentQueryBuilder Where(string fieldName, ComparisonOperation comparisonOperation, string value)
        {
            ConditionMetadata condition = new ConditionMetadata(fieldName, comparisonOperation, value);
            metadata.AddCondition(condition);

            return this;
        }

        public IFluentQueryBuilder Select(string fieldName)
        {
            return Select(fieldName, fieldName.Replace('.', '_'));
        }

        public IFluentQueryBuilder Select(string fieldName, string displayName)
        {
            SelectMetadata select = new SelectMetadata(fieldName, displayName);
            metadata.AddSelect(select);

            return this;
        }

        public IFluentQueryBuilder OrderBy(string fieldName, string isAscending)
        {
            bool strongIsAscending = bool.Parse(isAscending);
            return OrderBy(fieldName, strongIsAscending);
        }

        public IFluentQueryBuilder OrderBy(string fieldName, bool isAscending)
        {
            OrderingMetadata ordering = new OrderingMetadata(fieldName, isAscending);
            metadata.AddOrdering(ordering);

            return this;
        }

        public IFluentQueryBuilder Page(int currentPage, int recordsPerPage)
        {
            metadata.Paging = new PagingMetadata(currentPage, recordsPerPage);
            return this;
        }

        public abstract IDynamicQuery Compile();

        protected void AssertMetadataIsValid()
        {
            if (!metadata.Selects.Any())
                throw new InvalidOperationException("No select fields have been specified.");

            if(metadata.Paging != null && !metadata.Orderings.Any())
                throw new InvalidOperationException("A default ordering must be provided if paging is required.");
        }

        protected string ConnectionString
        {
            get { return connectionString; }
        }

        internal QueryMetadata Metadata
        {
            get { return metadata; }
        }
    }
}