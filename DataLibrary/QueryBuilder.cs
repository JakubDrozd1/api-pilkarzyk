using System.Text;

namespace DataLibrary
{
    internal class QueryBuilder<T>
    {
        private readonly StringBuilder query;
        private bool hasWhere;

        public QueryBuilder()
        {
            query = new StringBuilder();
            hasWhere = false;
        }

        public QueryBuilder<T> Select(string select)
        {
            query.Append($"SELECT {select}");
            return this;
        }

        public QueryBuilder<T> From(string from)
        {
            query.Append($" FROM {from}");
            return this;
        }

        public QueryBuilder<T> Where(string condition)
        {
            if (!hasWhere)
            {
                query.Append($" WHERE {condition}");
                hasWhere = true;
            }
            else
            {
                query.Append($" AND {condition}");
            }
            return this;
        }

        public QueryBuilder<T> OrWhere(string condition)
        {
            if (!hasWhere)
            {
                query.Append($" WHERE {condition}");
                hasWhere = true;
            }
            else
            {
                query.Append($" OR {condition}");
            }
            return this;
        }

        public QueryBuilder<T> OrderBy(string orderBy)
        {
            query.Append($" ORDER BY {orderBy}");
            return this;
        }

        public QueryBuilder<T> Limit(int limit)
        {
            query.Append($" LIMIT {limit}");
            return this;
        }

        public QueryBuilder<T> Insert(string tableName, object values)
        {
            var properties = values.GetType().GetProperties();
            var columns = string.Join(", ", properties.Select(p => p.Name).Skip(1));
            var valueParams = string.Join(", ", properties.Select(p => $"@{p.Name}").Skip(1));

            query.Append($"INSERT INTO {tableName} ({columns}) VALUES ({valueParams})");

            return this;
        }

        public QueryBuilder<T> Update(string tableName, object values)
        {
            var properties = values.GetType().GetProperties();
            var setPairs = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}").Skip(1));

            query.Append($"UPDATE {tableName} SET {setPairs}");

            return this;
        }

        public QueryBuilder<T> UpdateColumns(string tableName, params string[] columns)
        {
            var setPairs = string.Join(", ", columns.Select(column => $"{column} = @{column}"));

            query.Append($"UPDATE {tableName} SET {setPairs}");

            return this;
        }

        public QueryBuilder<T> Delete(string tableName)
        {
            query.Append($"DELETE FROM {tableName}");
            return this;
        }

        public string Build()
        {
            return query.ToString();
        }
    }
}
