using System;
using System.Collections.Generic;
using System.Linq;

namespace Stringly.Metadata
{
    internal class QueryMetadata
    {
        private readonly string tableName;
        private readonly List<SelectMetadata> selects = new List<SelectMetadata>(); 
        private readonly List<JoinMetadata> joins = new List<JoinMetadata>();
        private readonly List<ConditionMetadata> conditions = new List<ConditionMetadata>();
        private readonly List<OrderingMetadata> orderings = new List<OrderingMetadata>();

        private int currentPage = 1;
        private int recordsPerPage = 50;

        public QueryMetadata(string tableName)
        {
            this.tableName = tableName;
        }

        public void AddSelect(SelectMetadata select)
        {
            if(selects.Any(x => x.DisplayName == select.DisplayName))
                throw new InvalidOperationException(string.Format("Field with duplicate display name: {0}", select.DisplayName));

            selects.Add(select);
        }

        public void AddJoin(JoinMetadata join)
        {
            joins.Add(join);
        }

        public void AddCondition(ConditionMetadata condition)
        {
            conditions.Add(condition);
        }

        public void AddOrdering(OrderingMetadata ordering)
        {
            orderings.Add(ordering);
        }

        public string TableName
        {
            get { return tableName; }
        }

        public IReadOnlyCollection<SelectMetadata> Selects
        {
            get { return selects.AsReadOnly(); }
        }

        public IReadOnlyCollection<JoinMetadata> Joins
        {
            get { return joins.AsReadOnly(); }
        }

        public IReadOnlyCollection<ConditionMetadata> Conditions
        {
            get { return conditions.AsReadOnly(); }
        }

        public IReadOnlyCollection<OrderingMetadata> Orderings
        {
            get { return orderings.AsReadOnly(); }
        }

        public int CurrentPage
        {
            get { return currentPage; }
            internal set { currentPage = value; }
        }

        public int RecordsPerPage
        {
            get { return recordsPerPage; }
            internal set { recordsPerPage = value; }
        }
    }
}