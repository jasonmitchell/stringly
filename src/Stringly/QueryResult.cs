using System.Collections.Generic;

namespace Stringly
{
    public class QueryResult
    {
        private readonly int currentPage;
        private readonly IReadOnlyCollection<string> columns;
        private readonly List<QueryResultRow> rows = new List<QueryResultRow>(); 

        public QueryResult(List<string> columns, int currentPage)
        {
            this.currentPage = currentPage;
            this.columns = columns.AsReadOnly();
        }

        public int CurrentPage
        {
            get { return currentPage; }
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
