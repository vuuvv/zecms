using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace vuuvv.db
{
    public class Model
    {
        public static DBHelper db
        {
            get
            {
                return DBHelper.get();
            }
        }

        [Column]
        public int id { get; set; }

        protected void insert()
        {
            Type t = this.GetType();
            Table table = ModelHelper.metadata[t];
            string sql = string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                table.name,
                ModelHelper.join_to_format(table.columns, "`{0}`"),
                ModelHelper.join_to_format(table.columns, "@{0}")
            );

            id = db.insert(table.name, sql, convert_to_parameters(table.columns));
        }

        protected void update()
        {
            Type t = this.GetType();
            Table table = ModelHelper.metadata[t];
            string sql = string.Format(
                "UPDATE {0} SET {1} WHERE id={2}",
                table,
                ModelHelper.join_to_format(table.columns, "`{0}`=@{0}"),
                id
            );

            db.execute(sql, convert_to_parameters(table.columns));
        }

        protected Dictionary<string, object> convert_to_parameters(string[] columns)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            Type t = this.GetType();
            foreach (var col in columns)
            {
                args.Add(string.Format("@{0}", col), t.GetProperty(col).GetValue(this, null));
            }
            return args;
        }

        protected Dictionary<string, object> convert_to_parameters(Column[] columns)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            Type t = this.GetType();
            foreach (var col in columns)
            {
                args.Add(string.Format("@{0}", col), t.GetProperty(col.name).GetValue(this, null));
            }
            return args;
        }

    }
}
