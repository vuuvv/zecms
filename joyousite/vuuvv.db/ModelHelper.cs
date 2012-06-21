using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;

namespace vuuvv.db
{
    public static class ModelHelper
    {
        public static Dictionary<Type, Field> default_types = new Dictionary<Type, Field>
        {
            { typeof(int), new IntegerField() },
            { typeof(long), new IntegerField() },
            { typeof(string), new StringField() },
            { typeof(bool), new BooleanField() }
        };

        public static MyDBHelper db
        {
            get
            {
                return MyDBHelper.get();
            }
        }

        private static Dictionary<Type, MyTable> _metadata;

        public static Dictionary<Type, MyTable> metadata
        {
            get
            {
                if (_metadata == null)
                {
                    _metadata = new Dictionary<Type, MyTable>();
                    foreach (Type t in get_types_with_attribute<MyTable>(false)) 
                    {
                        MyTable table = (MyTable)Attribute.GetCustomAttribute(t, typeof(MyTable));
                        table.columns = get_columns(t);
                        _metadata.Add(t, table);
                    }
                }
                return _metadata;
            }
        }

        public static IEnumerable<Type> get_types_with_attribute<T>(bool inherit) 
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                {
                    if (t.IsDefined(typeof(T), inherit))
                        yield return t;
                }
            }
        }

        public static MyColumn[] get_columns(Type t)
        {
            PropertyInfo[] properties = t.GetProperties();
            List<MyColumn> columns = new List<MyColumn>();
            foreach (var p in properties) 
            {
                MyColumn col = (MyColumn)Attribute.GetCustomAttribute(p, typeof(MyColumn));
                if (col != null)
                {
                    if (col.field == null)
                    {
                        col.field = default_types[p.PropertyType];
                    }
                    if (col.name == null)
                    {
                        col.name = p.Name;
                    }
                    columns.Add(col);
                }
            }
            return columns.ToArray();
        }

        public static T get<T>(int id)
            where T : MyModel
        {
            Type t = typeof(T);
            MyTable table = metadata[t];
            string sql = string.Format("SELECT * FROM {0} where id=@id", table.name);
            var reader = db.query(sql, new Dictionary<string, object> {
                {"@id", id},
            });
            if (reader.HasRows)
            {
                reader.Read();
                return ModelHelper.fetch_object<T>(reader);
            }
            else
            {
                return default(T);
            }
        }

        public static T fetch_object<T>(DbDataReader reader)
            where T : MyModel
        {
            Type t = typeof(T);
            MyTable table = metadata[t];
            Assembly assembly = Assembly.GetAssembly(t);
            T model = (T)assembly.CreateInstance(t.FullName);

            // set id
            set_property_from_reader(model, "id", reader);

            foreach (var col in table.columns)
            {
                set_property_from_reader(model, col.name, reader);
            }
            return model;
        }

        public static void set_property_from_reader(object obj, string t_name, DbDataReader reader, string r_name) 
        {
            Type t = obj.GetType();
            PropertyInfo prop = t.GetProperty(t_name);
            Type desired = prop.PropertyType;
            prop.SetValue(obj, MyDBHelper.convert_to(reader[r_name], desired), null);
        }

        public static void set_property_from_reader(object obj, string name, DbDataReader reader)
        {
            set_property_from_reader(obj, name, reader, name);
        }

        public static T single<T>(DbDataReader reader)
            where T : MyModel
        {
            reader.Read();
            return fetch_object<T>(reader);
        }

        public static List<T> list<T>(DbDataReader reader)
            where T: MyModel
        {
            List<T> models = new List<T>();
            while (reader.Read())
            {
                models.Add(single<T>(reader));
            }
            return models;
        }

        public static string join_to_format(string[] cols, string format)
        {
            return string.Join(",", cols);
        }

        public static string join_to_format(MyColumn[] cols, string format)
        {
            List<string> rets = new List<string>();
            foreach (var col in cols)
            {
                rets.Add(string.Format(format, col.name));
            }
            return string.Join(",", rets.ToArray());
        }

        public static string join_to_format(IEnumerable<string> cols, string format)
        {
            cols = from col in cols select string.Format(format, col);
            return string.Join(",", cols.ToArray());
        }

    }
}
