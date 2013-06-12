namespace Stringly.Metadata
{
    internal class OrderingMetadata
    {
        private readonly string field;
        private readonly bool isAscending;

        public OrderingMetadata(string field, bool isAscending)
        {
            this.field = field;
            this.isAscending = isAscending;
        }

        public string Field
        {
            get { return field; }
        }

        public bool IsAscending
        {
            get { return isAscending; }
        }
    }
}