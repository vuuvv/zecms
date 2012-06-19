using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace vuuvv.db
{
    public abstract class Model
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

        private Table _table;
        public Table table
        {
            get
            {
                if (_table != null)
                    return _table;
                _table = ModelHelper.metadata[this.GetType()];
                if (_table == null)
                    throw new MemberAccessException(string.Format("Can't find table of Model {0}", this.GetType().FullName));
                return _table;
            }
        }

        public virtual void insert()
        {
            var columns = from col in table.columns where col.name != "id" select col.name;
            string sql = string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                table.name,
                ModelHelper.join_to_format(columns, "`{0}`"),
                ModelHelper.join_to_format(columns, "@{0}")
            );

            id = db.insert(table.name, sql, convert_to_parameters(table.columns));
        }

        public virtual void update()
        {
            string sql = string.Format(
                "UPDATE {0} SET {1} WHERE id={2}",
                table.name,
                ModelHelper.join_to_format(table.columns, "`{0}`=@{0}"),
                id
            );

            db.execute(sql, convert_to_parameters(table.columns));
        }

        public virtual void delete()
        {
            string sql = string.Format("DELETE FROM {0} WHERE id={1}", table.name, id);
            db.execute(sql);
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
                args.Add(string.Format("@{0}", col.name), t.GetProperty(col.name).GetValue(this, null));
            }
            return args;
        }

    }
}
