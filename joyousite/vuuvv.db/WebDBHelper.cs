using System;
using System.Configuration;
using System.Web;
using System.Data.Common;
using System.Data.Linq;

namespace vuuvv.db
{
    public class WebDBHelper
    {
        private static string cstr;
        private static DbProviderFactory factory;

        static WebDBHelper()
        {
            var cs = ConfigurationManager.ConnectionStrings["dbconnection"];
            string path = HttpContext.Current.Server.MapPath(cs.ConnectionString);
            cstr = string.Format("Data Source={0}", path);
            factory = DbProviderFactories.GetFactory(cs.ProviderName);
        }

        public static DataContext db
        {
            get
            {
                DataContext ctx = (DataContext)HttpContext.Current.Items["datacontext"];
                if (ctx == null)
                {
                    ctx = new DataContext(factory.CreateConnection());
                    HttpContext.Current.Items["dbconnection"] = ctx;
                }
                return ctx;
            }
        }
    }
}
