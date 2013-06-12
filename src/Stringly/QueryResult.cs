using System.Collections.Generic;

namespace Stringly
{
    public class QueryResult
    {
        private readonly IReadOnlyCollection<string> columns;
        private readonly List<QueryResultRow> rows = new List<QueryResultRow>(); 

        public QueryResult(List<string> columns)
        {
            this.columns = columns.AsReadOnly();
        }

        public IReadOnlyCollection<string> Columns
        {
            get { return columns; }
        }

        public List<QueryResultRow> Rows
        {
            get { return rows; }
        }
    }
}
