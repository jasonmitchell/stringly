using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Stringly.Metadata;

namespace Stringly.Queries
{
    public class SqlQuery : IDynamicQuery
    {
        private readonly string connectionString;
        private readonly QueryMetadata metadata;
        private readonly string generatedSql;

        internal SqlQuery(string connectionString, QueryMetadata metadata)
        {
            this.connectionString = connectionString;
            this.metadata = metadata;

            generatedSql = GenerateSql();
        }

        public DataTable Execute()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(generatedSql, connection))
            {
                connection.Open();
                command.CommandType = CommandType.Text;

                DataTable results = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(results);
                }

                return results;
            }
        }

        private string GenerateSql()
        {
            string innerSql = GenerateInnerSql();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendFormat("WITH QueryPage AS ({0})", innerSql);
            sqlBuilder.AppendLine();
            sqlBuilder.AppendLine("SELECT *");
            sqlBuilder.AppendLine("FROM QueryPage");

            int startRowNumber = (metadata.CurrentPage - 1) * metadata.RecordsPerPage;
            int endRowNumber = startRowNumber + metadata.RecordsPerPage;
            sqlBuilder.AppendLine(string.Format("WHERE RowNumber > {0} AND RowNumber <= {1}", startRowNumber, endRowNumber));

            GenerateOrderings(sqlBuilder, true);

            return sqlBuilder.ToString();
        }

        private string GenerateInnerSql()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            
            GenerateSelect(sqlBuilder);
            GenerateFrom(sqlBuilder);
            GenerateJoins(sqlBuilder);
            GenerateConditions(sqlBuilder);

            return sqlBuilder.ToString();
        }

        private void GenerateSelect(StringBuilder sqlBuilder)
        {
            sqlBuilder.Append("SELECT");

            StringBuilder orderBuilder = new StringBuilder();
            GenerateOrderings(orderBuilder, false);

            sqlBuilder.AppendFormat(" ROW_NUMBER() OVER ({0}) AS RowNumber", orderBuilder);

            foreach (SelectMetadata select in metadata.Selects)
            {
                sqlBuilder.AppendFormat(", {0} AS [{1}]", select.DataFieldName, select.DisplayName);
            }
        }

        private void GenerateFrom(StringBuilder sqlBuilder)
        {
            sqlBuilder.AppendLine();
            sqlBuilder.AppendLine(string.Format("FROM [{0}]", metadata.TableName));
        }

        private void GenerateJoins(StringBuilder sqlBuilder)
        {
            foreach (JoinMetadata join in metadata.Joins)
            {
                sqlBuilder.AppendLine(string.Format("JOIN [{0}] ON {1} = {2}", join.TableName, join.PrimaryKeyField, join.ForeignKeyField));
            }
        }

        private void GenerateConditions(StringBuilder sqlBuilder)
        {
            Dictionary<ComparisonOperation, string> operatorMappings = new Dictionary<ComparisonOperation, string>
            {
                { ComparisonOperation.Equals, "=" },
                { ComparisonOperation.LessThan, "<" },
                { ComparisonOperation.LessThanOrEqualTo, "<=" },
                { ComparisonOperation.GreaterThan, ">" },
                { ComparisonOperation.GreaterThanOrEqualTo, ">=" }
            };

            bool hasPreviousCondition = false;
            foreach (ConditionMetadata condition in metadata.Conditions)
            {
                string prefix = !hasPreviousCondition ? "WHERE" : "AND";
                string comparisonOperator = operatorMappings[condition.ComparisonOperation];

                sqlBuilder.AppendLine(string.Format("{0} {1} {2} '{3}'", prefix, condition.FieldName, comparisonOperator, condition.Value));

                hasPreviousCondition = true;
            }
        }

        private void GenerateOrderings(StringBuilder sqlBuilder, bool useFieldDisplayNames)
        {
            bool hasPreviousOrdering = false;
            foreach (OrderingMetadata ordering in metadata.Orderings)
            {
                string prefix = !hasPreviousOrdering ? "ORDER BY" : ",";
                string fieldName = !useFieldDisplayNames ? ordering.Field : metadata.Selects.Single(x => x.DataFieldName == ordering.Field).DisplayName;
                string direction = ordering.IsAscending ? "ASC" : "DESC";

                sqlBuilder.AppendFormat("{0} {1} {2}", prefix, fieldName, direction);

                hasPreviousOrdering = true;
            }
        }

        public string GeneratedSql
        {
            get { return generatedSql; }
        }
    }
}