namespace Stringly.Metadata
{
    internal class SelectMetadata
    {
        private readonly string dataFieldName;
        private readonly string displayName;

        public SelectMetadata(string dataFieldName, string displayName)
        {
            this.dataFieldName = dataFieldName;
            this.displayName = displayName;
        }

        public string DataFieldName
        {
            get { return dataFieldName; }
        }

        public string DisplayName
        {
            get { return displayName; }
        }
    }
}