using System.Collections.Generic;

namespace Stringly
{
    public class QueryResultRow
    {
        private readonly IReadOnlyCollection<object> data; 

        public QueryResultRow(List<object> data)
        {
            this.data = data.AsReadOnly();
        }

        public IReadOnlyCollection<object> Data
        {
            get { return data; }
        }
    }
}
