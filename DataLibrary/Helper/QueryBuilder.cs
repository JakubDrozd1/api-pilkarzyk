using System.Text;
using DataLibrary.Model.DTO;
using static Dapper.SqlMapper;

namespace DataLibrary.Helper
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
            query.Append($"FROM {from}");
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
                query.Append($"{condition}");
            }
            return this;
        }

        public QueryBuilder<T> Limit(IPagination Limit)
        {
            if (Limit.OnPage != -1)
            {
                query.Append($"ROWS {Limit.Page * Limit.OnPage + 1} TO {Limit.Page * Limit.OnPage + Limit.OnPage} ");
            }
            return this;
        }

        public QueryBuilder<T> OrderBy(ISortable OrderBy)
        {
            if (OrderBy.SortColumn != null)
            {
                if (OrderBy.SortMode == null || !OrderBy.SortMode.Equals("asc", StringComparison.CurrentCultureIgnoreCase) && !OrderBy.SortMode.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    OrderBy.SortMode = "ASC";
                }
                query.Append($"ORDER BY {OrderBy.SortColumn} {OrderBy.SortMode} ");
            }

            return this;
        }

        public QueryBuilder<T> Insert(string tableName, object values)
        {
            var properties = values.GetType().GetProperties();
            var columns = string.Join(", ", properties.Select(p => p.Name).Skip(1));
            var valueParams = string.Join(", ", properties.Select(p => $"@{p.Name}").Skip(1));

            query.Append($"INSERT INTO {tableName} ({columns}) VALUES ({valueParams}) ");

            return this;
        }

        public QueryBuilder<T> Update(string tableName, object values)
        {
            var properties = values.GetType().GetProperties();
            var setPairs = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}").Skip(1));

            query.Append($"UPDATE {tableName} SET {setPairs}");

            return this;
        }

        public QueryBuilder<T> UpdateColumns(string tableName, string[] columns)
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
