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
            var db = HttpContext.Current.Cache["dbhelper"];

            if (db == null)
            {
                db = DBHelper.create();
                HttpContext.Current.Cache.Insert("dbhelper", db);
            }
            return (DBHelper)db;
        }

        public DbConnection conn
        {
            get
            {
                DbConnection c = (DbConnection)HttpContext.Current.Items["dbconnection"];
                if (c == null || c.State == ConnectionState.Closed)
                {
                    c = connect();
                    HttpContext.Current.Items["dbconnection"] = c;
                }
                return c;
            }
        }

        private DbConnection connect()
        {
            DbConnection c = factory.CreateConnection();
            c.ConnectionString = connection_string;
            c.Open();
            return c;
        }

        public void disconnect()
        {
            DbConnection c = (DbConnection)HttpContext.Current.Items["dbconnection"];
            if (c != null)
                c.Close();
        }

        private DBHelper(string connection_string, string provider)
        {
            this.connection_string = connection_string;
            factory = DbProviderFactories.GetFactory(provider);
        }

        public int execute(string sql)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;

                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
        }

        public int execute(string sql, Dictionary<string, object> args)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                prepare(cmd, conn, null, sql, args);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rows;
            }
        }

        public int insert(string table, string sql, Dictionary<string, object> args)
        {
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

        public DbDataReader query(string sql)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;

                DbDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        public DbDataReader query(string sql, Dictionary<string, object> args)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                prepare(cmd, conn, null, sql, args);
                DbDataReader reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return reader;
            }
        }

        public object one(string sql)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;

                var obj = cmd.ExecuteScalar();
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return obj;
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
