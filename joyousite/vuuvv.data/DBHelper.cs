using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Remoting.Messaging;

using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;

namespace vuuvv.data
{
    public class DBHelper
    {
        public static string dbpath = "App_Data/db/cms.db";
        public static HashSet<string> libs = new HashSet<string>()
        {
            "vuuvv.page"
        };

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";

        private static ISessionFactory _factory;
        private static ISessionFactory factory
        {
            get
            {
                if (_factory == null)
                {
                    string path = get_db_path();
                    if (!File.Exists(path)) 
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        create_schema();
                    }
                    _factory = build_configuration().BuildSessionFactory();
                }
                return _factory;
            }
        }

        public static ISession session
        {
            get
            {
                ISession s = context_session;
                if (s == null)
                {
                    s = factory.OpenSession();
                    context_session = s;
                }
                return s;
            }
        }

        public static void close_session()
        {
            ISession s = session;
            if (s != null && s.IsOpen)
            {
                s.Flush();
                s.Close();
            }
            context_session = null;
        }

        public static void begin_transaction()
        {
            ITransaction trans = context_transaction;
            if (trans == null)
            {
                trans = session.BeginTransaction();
                context_transaction = trans;
            }
        }

        public static void commit()
        {
            ITransaction trans = context_transaction;
            try
            {
                if (transaction_has_open)
                {
                    trans.Commit();
                    context_transaction = null;
                }
            }
            catch (HibernateException)
            {
                rollback();
                throw;
            }
        }

        public static void rollback()
        {
            ITransaction trans = context_transaction;
            try
            {
                if (transaction_has_open)
                {
                    trans.Rollback();
                }
                context_transaction = null;
            }
            finally
            {
                close_session();
            }
        }

        public static bool transaction_has_open
        {
            get
            {
                ITransaction trans = context_transaction;
                return trans != null && !trans.WasCommitted && !trans.WasRolledBack;
            }
        }

        private static ISession context_session
        {
            get
            {
                return get_context<ISession>(SESSION_KEY);
            }
            set
            {
                set_context(SESSION_KEY, value);
            }
        }

        private static ITransaction context_transaction
        {
            get
            {
                return get_context<ITransaction>(TRANSACTION_KEY);
            }
            set
            {
                set_context(TRANSACTION_KEY, value);
            }
        }

        private static FluentConfiguration build_configuration()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(get_db_path()).ShowSql())
                .Mappings(make_mappings);
        }

        public static void create_schema()
        {
            build_configuration().ExposeConfiguration(build_schema).BuildConfiguration();
        }

        private static void build_schema(Configuration cfg)
        {
            var schema = new SchemaExport(cfg);
            schema.Drop(false, true);
            schema.Create(false, true);
        }

        private static bool is_mapping_of<T>(Type type)
        {
            return !type.IsGenericType && typeof(T).IsAssignableFrom(type);
        }

        private static void make_mappings(MappingConfiguration cfg)
        {
            foreach (var lib in libs)
            {
                var a = System.Reflection.Assembly.Load(lib);
                cfg.FluentMappings.AddFromAssembly(a);
            }
        }

        private static T get_context<T>(string key)
        {
            if (in_web_context)
                return (T)HttpContext.Current.Items[key];
            return (T)CallContext.GetData(key);
        }

        private static void set_context(string key, object value) 
        {
            if (in_web_context)
                HttpContext.Current.Items[key] = value;
            else
                CallContext.SetData(key, value);
        }

        private static bool in_web_context
        {
            get
            {
                return HttpContext.Current != null;
            }
        }

        private static string get_db_path()
        {
            string file = dbpath;
            if (in_web_context)
            {
                return HttpContext.Current.Server.MapPath(file);
            }
            return file;
        }
    }
}
