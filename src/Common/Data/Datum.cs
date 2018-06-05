using MySql.Data.MySqlClient;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Destiny.Data
{
    public sealed class Datums : IEnumerable<Datum>
    {
        private string Table { get; }
        private List<Datum> Values { get; set; }
        private string ConnectionString { get; }

        public Datums(string table)
        {
            Table = table;
            ConnectionString = Database.ConnectionString;
        }

        public Datums(string table, string schema)
        {
            Table = table;
            ConnectionString = string.Format("server={0}; database={1}; uid={2}; password={3};" +
                                                  "convertzerodatetime=yes; SslMode=none;",
                Database.Host,
                schema,
                Database.Username,
                Database.Password);
        }

        private void PopulateInternal(string fields, string constraints, params object[] args)
        {
            Values = new List<Datum>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = Database.GetCommand(connection, constraints, args))
                {
                    string whereClause = constraints != null ? " WHERE " + command.CommandText : string.Empty;
                    command.CommandText = string.Format("SELECT {0} FROM `{1}`{2}", fields == null ? "*" : Database.CorrectFields(fields), Table, whereClause);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dictionary.Add(reader.GetName(i), reader.GetValue(i));
                            }

                            Values.Add(new Datum(Table, dictionary));
                        }
                    }
                }
            }
        }

        public Datums Populate()
        {
            PopulateInternal(null, null, null);

            return this;
        }

        public Datums Populate(string constraints, params object[] args)
        {
            PopulateInternal(null, constraints, args);

            return this;
        }

        public Datums PopulateWith(string fields)
        {
            PopulateInternal(fields, null, null);

            return this;
        }

        public Datums PopulateWith(string fields, string constraints, params object[] args)
        {
            PopulateInternal(fields, constraints, args);

            return this;
        }

        public IEnumerator<Datum> GetEnumerator()
        {
            foreach (Datum loopDatum in Values)
            {
                yield return loopDatum;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class Datum
    {
        public string Table { get; }
        public Dictionary<string, Object> Dictionary { get; }
        private string ConnectionString { get; }

        public object this[string name]
        {
            get
            {
                if (Dictionary[name] is DBNull)
                {
                    return null;
                }

                if (Dictionary[name] is byte && Meta.IsBool(Table, name))
                {
                    return (byte) Dictionary[name] > 0;
                }

                return Dictionary[name];
            }
            set
            {
                if (value is DateTime)
                {
                    if (Meta.IsDate(Table, name))
                    {
                        value = ((DateTime)value).ToString("yyyy-MM-dd");
                    }

                    else if (Meta.IsDateTime(Table, name))
                    {
                        value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                Dictionary[name] = value;
            }
        }

        public Datum(string table)
        {
            Table = table;
            Dictionary = new Dictionary<string, object>();
            ConnectionString = Database.ConnectionString;
        }

        public Datum(string table, string schema)
        {
            Table = table;
            Dictionary = new Dictionary<string, object>();
            ConnectionString = string.Format("server={0}; database={1}; uid={2}; password={3};" +
                                                  "convertzerodatetime=yes; SslMode=none;",
                Database.Host,
                schema,
                Database.Username,
                Database.Password);
        }

        public Datum(string table, Dictionary<string, object> dictionary)
        {
            Table = table;
            Dictionary = dictionary;
            ConnectionString = Database.ConnectionString;
        }

        public Datum(string table, string schema, Dictionary<string, object> dictionary)
        {
            Table = table;
            Dictionary = dictionary;
            ConnectionString = string.Format("server={0}; database={1}; uid={2}; password={3};" +
                                                  "convertzerodatetime=yes; SslMode=none;",
                Database.Host,
                schema,
                Database.Username,
                Database.Password);
        }

        public Datum Populate(string constraints, params object[] args)
        {
            PopulateWith("*", constraints, args);

            return this;
        }

        public Datum PopulateWith(string fields, string constraints, params object[] args)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = Database.GetCommand(connection, constraints, args))
                {
                    command.CommandText = string.Format("SELECT {0} FROM `{1}` WHERE ", Database.CorrectFields(fields), Table) + command.CommandText;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 1)
                        {
                            throw new RowNotUniqueException();
                        }

                        if (!reader.HasRows)
                        {
                            throw new RowNotInTableException();
                        }

                        reader.Read();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string name = reader.GetName(i);
                            object value = reader.GetValue(i);

                            Dictionary[name] = value;
                        }
                    }
                }
            }

            return this;
        }

        public void Insert()
        {
            string fields = "( ";

            int processed = 0;

            foreach (KeyValuePair<string, object> loopPair in Dictionary)
            {
                fields += "`" + loopPair.Key + "`";
                processed++;

                if (processed < Dictionary.Count)
                {
                    fields += ", ";
                }
            }

            fields += " ) VALUES ( ";

            object[] valueArr = new object[Dictionary.Count];
            Dictionary.Values.CopyTo(valueArr, 0);
            for (int i = 0; i < valueArr.Length; i++)
            {
                fields += "{" + i + "}";

                if (i < valueArr.Length - 1)
                {
                    fields += ", ";
                }
            }

            fields += " )";

            Database.Execute(string.Format("INSERT INTO `{0}` {1}", Table, fields), valueArr);
        }

        public int InsertAndReturnID()
        {
            Insert();

            return (int)(ulong) Database.Scalar("SELECT LAST_INSERT_ID()");
        }

        public void Update(string constraints, params object[] args)
        {
            int processed = 0;

            string fields = string.Empty;

            object[] valueArr = new object[Dictionary.Count];
            Dictionary.Values.CopyTo(valueArr, 0);
            foreach (KeyValuePair<string, object> loopPair in Dictionary)
            {
                fields += string.Format("`{0}`={1}", loopPair.Key, "{" + processed + "}");
                processed++;

                if (processed < Dictionary.Count)
                {
                    fields += ", ";
                }
            }

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = Database.GetCommand(connection, constraints, args))
                {
                    command.CommandText = Database.ParameterizeCommandText("set", string.Format("UPDATE `{0}` SET {1} WHERE ", Table, fields), valueArr) + command.CommandText;
                    command.Parameters.AddRange(Database.ConstraintsToParameters("set", fields, valueArr));

                    command.ExecuteNonQuery();
                }
            }
        }

        public override string ToString()
        {
            string result = Table + " [ ";

            int processed = 0;

            foreach (KeyValuePair<string, object> value in Dictionary)
            {
                result += value.Key;
                processed++;

                if (processed < Dictionary.Count)
                {
                    result += ", ";
                }
            }

            result += " ]";

            return result;
        }
    }
}
