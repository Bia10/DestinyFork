using System.Collections.Generic;

using Destiny.IO;

namespace Destiny.Data
{
    public static class Meta
    {
        public static Dictionary<string, Dictionary<string, Column>> Tables { get; private set; }

        public static void Initialize(bool mcdb)
        {
            Tables = new Dictionary<string, Dictionary<string, Column>>();

            foreach (Datum datum in new Datums("COLUMNS").Populate("TABLE_SCHEMA = {0} OR TABLE_SCHEMA = {1}", Database.DefaultSchema, mcdb ? "mcdb" : string.Empty))
            {
                Add(datum);
            }

            Log.Inform("Meta analyzed database.");
        }

        private static void Add(Datum datum)
        {
            Dictionary<string, Column> table;

            string tableName = (string)datum["TABLE_NAME"];

            if (Tables.ContainsKey(tableName))
            {
                table = Tables[tableName];
            }

            else
            {
                table = new Dictionary<string, Column>();

                Tables.Add(tableName, table);
            }

            table.Add((string)datum["COLUMN_NAME"], new Column(datum));
        }

        public static bool IsBool(string tableName, string fieldName)
        {
            return Tables[tableName][fieldName].ColumnType == "tinyint(1) unsigned";
        }

        public static bool IsDate(string tableName, string fieldName)
        {
            return Tables[tableName][fieldName].ColumnType == "date";
        }

        public static bool IsDateTime(string tableName, string fieldName)
        {
            return Tables[tableName][fieldName].ColumnType == "datetime";
        }
    }

    public sealed class Column
    {
        public string Name { get; }
        public bool IsPrimaryKey { get; }
        public bool IsUniqueKey { get; }
        public string ColumnType { get; }

        public Column(Datum datum)
        {
            Name = (string)datum["COLUMN_NAME"];
            IsPrimaryKey = (string)datum["COLUMN_KEY"] == "PRI";
            IsUniqueKey = (string)datum["COLUMN_KEY"] == "UNI";
            ColumnType = (string)datum["COLUMN_TYPE"];
        }
    }
}