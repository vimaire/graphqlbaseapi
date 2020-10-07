using System;
using System.Text.RegularExpressions;
using NHibernate.Cfg;

namespace Api.Infrastructure.NHibernate
{
    // Source: https://manfredlange.blogspot.com/2011/04/fluent-nhibernate-postgresql-and.html
    public class PostgresNamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className)
        {
            return DoubleQuote(className);
        }
        public string PropertyToColumnName(string propertyName)
        {
            return DoubleQuote(propertyName);
        }
        public string TableName(string tableName)
        {
            return DoubleQuote(tableName);
        }
        public string ColumnName(string columnName)
        {
            return DoubleQuote(columnName);
        }
        public string PropertyToTableName(string className,
            string propertyName)
        {
            return DoubleQuote(propertyName);
        }
        public string LogicalColumnName(string columnName,
            string propertyName)
        {
            return String.IsNullOrWhiteSpace(columnName) ?
                DoubleQuote(propertyName) :
                DoubleQuote(columnName);
        }
        private static string DoubleQuote(string raw)
        {
            var alreadyQuoted = new Regex("\\\"\\w*\\\"");
            if (alreadyQuoted.IsMatch(raw))
            {
                return raw;
            }

            // In some cases the identifier is single-quoted.
            // We simply remove the single quotes:
            raw = raw.Replace("`", "");
            return String.Format("\"{0}\"", raw);
        }
    }
}
