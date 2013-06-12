namespace Stringly.Metadata
{
    internal class PagingMetadata
    {
        private readonly int currentPage;
        private readonly int recordsPerPage;

        public PagingMetadata(int currentPage, int recordsPerPage)
        {
            this.currentPage = currentPage;
            this.recordsPerPage = recordsPerPage;
        }

        public int CurrentPage
        {
            get { return currentPage; }
        }

        public int RecordsPerPage
        {
            get { return recordsPerPage; }
        }
    }
}
