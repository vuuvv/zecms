using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    class Model
    {
        public static string table = "";

        public static DBHelper db
        {
            get
            {
                return DBHelper.get();
            }
        }

        public static Dictionary<string, Field> fields = new Dictionary<string, Field>()
        {
            { "id", new IntegerField() }
        };

        public int id { get; set; }

        protected void insert()
        {
            Type t = this.GetType();
            string table = (string)t.GetField("table").GetValue(null);
            var fields = (Dictionary<string, Field>)t.GetField("fields").GetValue(null);
            string sql = string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                table,
                DBHelper.join_to_format(fields.Keys.ToArray(), "`{0}`"),
                DBHelper.join_to_format(fields.Keys.ToArray(), "@{0}")
            );

            id = db.insert(table, sql, convert_to_parameters(fields.Keys.ToArray()));
        }

        protected void update()
        {
            Type t = this.GetType();
            string table = (string)t.GetField("table").GetValue(null);
            var fields = (Dictionary<string, Field>)t.GetField("fields").GetValue(null);
            string sql = string.Format(
                "UPDATE {0} SET {1} WHERE id={2}",
                table,
                DBHelper.join_to_format(fields.Keys.ToArray(), "`{0}`=@{0}"),
                id
            );

            db.execute(sql, convert_to_parameters(fields.Keys.ToArray()));
        }

        protected Dictionary<string, object> convert_to_parameters(string[] columns)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            Type t = this.GetType();
            foreach (string col in columns)
            {
                args.Add(string.Format("@{0}", col), t.GetProperty(col).GetValue(this, null));
            }
            return args;
        }
    }
}
