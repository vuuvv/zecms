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
        public static DBHelper db
        {
            get
            {
                return DBHelper.get();
            }
        }

        public static T get<T>(int id)
            where T : Model
        {
            string table = (string)typeof(T).GetField("table").GetValue(null);
            string sql = string.Format("SELECT * FROM {0} where id=@id", table);
            var reader = db.query(sql, new Dictionary<string, object> {
                {"@id", Convert.ToInt32(id)},
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
            where T : Model
        {
            Type t = typeof(T);
            Assembly assembly = Assembly.GetAssembly(t);
            T model = (T)assembly.CreateInstance(t.FullName);

            // set id
            t.GetProperty("id").SetValue(model, reader["id"], null);

            Dictionary<string, Field> fields = (Dictionary<string, Field>)t.GetField("fields").GetValue(null);
            foreach (var field in fields)
            {
                t.GetProperty(field.Key).SetValue(model, field.Value.to_object(reader[field.Key]), null);
            }
            return model;
        }

        public static T single<T>(DbDataReader reader)
            where T : Model
        {
            reader.Read();
            return fetch_object<T>(reader);
        }

        public static List<T> list<T>(DbDataReader reader)
            where T: Model
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
            List<string> rets = new List<string>();
            foreach (string col in cols)
            {
                rets.Add(string.Format(format, col));
            }
            return string.Join(",", rets.ToArray());
        }
    }
}
