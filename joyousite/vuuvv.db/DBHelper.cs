using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Web;

namespace vuuvv.db
{
    public class DBHelper
    {
        private string connection_string;
        private DbProviderFactory factory;

        public static DBHelper create()
        {
            var cstr = ConfigurationManager.ConnectionStrings["dbconnection"];
            string[] parts = cstr.ConnectionString.Split(';');
            string path = HttpContext.Current.Server.MapPath(parts[1]);
            return new DBHelper(string.Format("Provider={0};Data Source={1};", parts[0], path), cstr.ProviderName);
        }

        public static DBHelper get()
        {
            
            if (HttpContext.Current.Cache["dbhelper"] == null)
            {
                HttpContext.Current.Cache.Insert("dbhelper", DBHelper.create());
            }
            return (DBHelper)HttpContext.Current.Cache["dbhelper"];
        }

        public static object fetch_object(DbDataReader reader, Type t)
        {
            Assembly assembly = Assembly.GetAssembly(t);
            object model = assembly.CreateInstance(t.FullName);

            // set id
            t.GetProperty("id").SetValue(model, reader["id"], null);

            Dictionary<string, Field> fields = (Dictionary<string, Field>)t.GetField("fields").GetValue(null);
            foreach (var field in fields)
            {
                t.GetProperty(field.Key).SetValue(model, field.Value.to_object(reader[field.Key]), null);
            }
            return model;
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

        private DBHelper(string connection_string, string provider)
        {
            this.connection_string = connection_string;
            factory = DbProviderFactories.GetFactory(provider);
        }

        public int execute(string sql)
        {
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connection_string;
                using (DbCommand cmd = factory.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        public int execute(string sql, Dictionary<string, object> args)
        {
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connection_string;
                using (DbCommand cmd = factory.CreateCommand())
                {
                    prepare(cmd, conn, null, sql, args);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
            }
        }

        public int insert(string table, string sql, Dictionary<string, object> args)
        {
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connection_string;
                using (DbCommand cmd = factory.CreateCommand())
                {
                    prepare(cmd, conn, null, sql, args);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT @@IDENTITY FROM " + table;
                    DbDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    return (int)reader.GetValue(0);
                }
            }
        }

        public DbDataReader query(string sql)
        {
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connection_string;
                using (DbCommand cmd = factory.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    conn.Open();
                    return cmd.ExecuteReader();
                }
            }
        }

        public DbDataReader query(string sql, Dictionary<string, object> args)
        {
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connection_string;
                using (DbCommand cmd = factory.CreateCommand())
                {
                    prepare(cmd, conn, null, sql, args);
                    DbDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.Clear();
                    return reader;
                }
            }
        }

        private void prepare(DbCommand cmd, DbConnection conn, DbTransaction trans, string sql, Dictionary<string, object> parameters)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    DbParameter param = factory.CreateParameter();
                    param.ParameterName = entry.Key;
                    param.Value = entry.Value;
                    cmd.Parameters.Add(param);
                }
            }
        }
    }
}
