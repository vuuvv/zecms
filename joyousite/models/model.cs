using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Reflection;
using vuuvv.db;

namespace models
{
    public abstract class Model
    {
        public static string[] columns = {};

        public static string table = "";

        protected static Dictionary<string, Field> _fields = new Dictionary<string, Field>()
        {
            { "id", new IntegerField() }
        };

        public int id { get; set; }

        protected static MyDBHelper db
        {
            get
            {
                return MyDBHelper.get();
            }
        }

        protected static Model fetch_data_from_reader(DbDataReader reader, Type t)
        {
            Assembly assembly = Assembly.GetAssembly(t);
            Model model = (Model)assembly.CreateInstance(t.FullName);

            // set id
            t.GetProperty("id").SetValue(model, reader.GetValue(reader.GetOrdinal("id")), null);

            string[] cols = (string[])t.GetField("columns").GetValue(null);
            foreach (string col in cols)
            {
                t.GetProperty(col).SetValue(model, reader[col], null);
            }
            return model;
        }

        protected static Model single_from_reader(DbDataReader reader, Type t)
        {
            reader.Read();
            return null;
            //return (Model)ModelHelper.fetch_object<vuuvv.db.Model>(reader);
        }

        protected static List<Model> list_from_reader(DbDataReader reader, Type t)
        {
            List<Model> objs = new List<Model>();
            while (reader.Read())
            {
            }
            return objs;
        }

        public Model get(int id, Type t)
        {
            string sql = "SELECT * FROM pages where id=@id";
            DbDataReader reader = db.query(sql, new Dictionary<string, object> {
                {"@id", Convert.ToInt32(id)},
            });
            if (reader.HasRows)
            {
                return (Page)single_from_reader(reader, typeof(Page));
            }
            else
            {
                return null;
            }
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
            //id = db.insert(table, sql, args);
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
            db.execute(sql, args);
        }

        protected static List<object> find(Dictionary<string, object> args, Type t)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE {1}");
            DbDataReader reader = db.query(sql, args);
            return null;
            //return list_from_reader(reader, t);
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
