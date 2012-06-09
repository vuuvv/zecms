using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Reflection;
using dbutils;

namespace models
{
    public class Model
    {
        public static string[] columns = {};

        public static string table = "";

        public int id { get; set; }

        protected static DbHelperOleDb db
        {
            get
            {
                return DbHelperOleDb.get();
            }
        }

        protected static object fetch_data_from_reader(OleDbDataReader reader, Type t)
        {
            Assembly assembly = Assembly.GetAssembly(t);
            object model = assembly.CreateInstance(t.FullName);

            // set id
            t.GetProperty("id").SetValue(model, reader.GetValue(reader.GetOrdinal("id")), null);

            string[] cols = (string[])t.GetField("columns").GetValue(null);
            foreach (string col in cols)
            {
                t.GetProperty(col).SetValue(model, reader.GetValue(reader.GetOrdinal(col)), null);
            }
            return model;
        }

        protected static object single_from_reader(OleDbDataReader reader, Type t)
        {
            reader.Read();
            return fetch_data_from_reader(reader, t);
        }

        protected static List<object> list_from_reader(OleDbDataReader reader, Type t)
        {
            List<object> objs = new List<object>();
            while (reader.Read())
            {
                objs.Add(fetch_data_from_reader(reader, t));
            }
            return objs;
        }

        protected void insert()
        {
            Type t = this.GetType();
            Dictionary<string, object> args = new Dictionary<string, object>();
            string[] cols = (string[])t.GetField("columns").GetValue(null);
            string table = (string)t.GetField("table").GetValue(null);
            string sql = string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})", table, 
                join_to_format(cols, "`{0}`"),
                join_to_format(cols, "@{0}")
            );
            foreach (string col in cols)
            {
                args.Add(string.Format("@{0}",col), t.GetProperty(col).GetValue(this, null));
            }
            id = db.ExecuteInsert(sql, args);
        }

        protected void update()
        {
            Type t = this.GetType();
            Dictionary<string, object> args = new Dictionary<string, object>();
            string[] cols = (string[])t.GetField("columns").GetValue(null);
            string table = (string)t.GetField("table").GetValue(null);
            string sql = string.Format(
                "UPDATE {0} SET {1} WHERE id={2}", 
                table, 
                join_to_format(cols, "`{0}`=@{0}"),
                id
            );
            foreach (string col in cols)
            {
                args.Add(string.Format("@{0}",col), t.GetProperty(col).GetValue(this, null));
            }
            db.ExecuteSql(sql, args);
        }

        protected static List<object> find(Dictionary<string, object> args, Type t)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE {1}");
            OleDbDataReader reader = db.ExecuteReader(sql, args);
            return list_from_reader(reader, t);
        }

        protected static string join_to_format(string[] cols, string format)
        {
            List<string> rets = new List<string>();
            foreach (string col in cols)
            {
                rets.Add(string.Format(format, col));
            }
            return string.Join(",", rets.ToArray());
        }
    }
}
