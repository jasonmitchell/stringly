namespace Stringly.Metadata
{
    internal class ConditionMetadata
    {
        private readonly string fieldName;
        private readonly ComparisonOperation comparisonOperation;
        private readonly string value;

        public ConditionMetadata(string fieldName, ComparisonOperation comparisonOperation, string value)
        {
            this.fieldName = fieldName;
            this.comparisonOperation = comparisonOperation;
            this.value = value;
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public ComparisonOperation ComparisonOperation
        {
            get { return comparisonOperation; }
        }

        public string Value
        {
            get { return value; }
        }
    }
}