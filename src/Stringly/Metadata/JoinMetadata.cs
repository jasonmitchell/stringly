namespace Stringly.Metadata
{
    internal class JoinMetadata
    {
        private readonly string tableName;
        private readonly string primaryKeyField;
        private readonly string foreignKeyField;

        public JoinMetadata(string tableName, string primaryKeyField, string foreignKeyField)
        {
            this.tableName = tableName;
            this.primaryKeyField = primaryKeyField;
            this.foreignKeyField = foreignKeyField;
        }

        public string TableName
        {
            get { return tableName; }
        }

        public string PrimaryKeyField
        {
            get { return primaryKeyField; }
        }

        public string ForeignKeyField
        {
            get { return foreignKeyField; }
        }
    }
}