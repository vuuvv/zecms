using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using vuuvv.page.configuration;

namespace vuuvv.page
{
    public class RewriteModule : IHttpModule
    {
        public PageSection config
        {
            get
            {
                System.Web.Caching.Cache cache = HttpContext.Current.Cache;
                if (cache["PageRouteConfig"] == null)
                {
                    cache.Insert("PageRouteConfig", ConfigurationManager.GetSection("Page"));
                }
                return (PageSection)cache["PageRouteConfig"];
            }
        }

        public static string resolve_url(string app_path, string url)
        {
            if (url.Length == 0 || url[0] != '~')
                return url;
            if (url.Length == 1)
                return app_path;
            if (url[1] == '/' || url[1] == '\\')
            {
                if (app_path.Length > 1)
                    return app_path + "/" + url.Substring(2);
                else
                    return "/" + url.Substring(2);
            }
            if (app_path.Length > 1)
                return app_path + "/" + url.Substring(1);
            else
                return app_path + url.Substring(1);
        }

        public static string resolve_pattern(string app_path, string pattern)
        {
            if (pattern[0] == '/')
                return "^" + pattern;
            return "^" + app_path + pattern;
        }

        public static string pop_string(string s, string prefix)
        {
            if (s.StartsWith(prefix))
                return s.Substring(prefix.Length);
            return s;
        }

        public virtual void Init(HttpApplication app)
        {
            app.AuthorizeRequest += new EventHandler(ModuleRewrite_AuthorizeRequest);
            app.EndRequest += new EventHandler(db_close);
        }

        public virtual void Dispose() { }

        protected virtual void ModuleRewrite_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            dispatch(app.Context);
        }

        protected virtual void db_close(object sender, EventArgs e)
        {
            //vuuvv.db.WebDBHelper.db.Dispose();
        }

        protected void dispatch(HttpContext context)
        {
            HttpRequest req = context.Request;
            string path = pop_string(req.Path.ToLower(), req.ApplicationPath);
            if (!path.EndsWith(".aspx"))
                return;

            path = to_slug(path, new string[] { "/index.aspx", "/default.aspx"}, ".aspx");
            RouteElementsCollection routes = config.routes;
            string template = config.template;

            for (int i = 0; i < routes.Count; i++)
            {
                RouteElement route = routes[i];
                Regex re = new Regex(route.pattern, RegexOptions.IgnoreCase);
                if (re.IsMatch(path))
                {
                    if (route.handler == "")
                        return;
                    string handler = route.handler;
                    if (route.ignore)
                    {
                        context.RewritePath(handler);
                        return;
                    }
                    else
                    {
                        template = handler;
                        break;
                    }
                }
            }
            // remained page need fetch data from database
            /*
            models.Page page = models.Page.from_slug(path);
            if (page == null)
            {
                throw new Exception("Page not find!");
            }
            context.Items["page"] = page;
            context.RewritePath(resolve_url(req.ApplicationPath, template));
            */
        }

        public static string to_slug(string path, string[] defaults, string ext)
        {
            if (Array.IndexOf(defaults, "/" + path) != -1)
                return "";
            List<string> tails = new List<string>(defaults);
            tails.Add(ext);
            return cut_tail(path, tails.ToArray());
        }

        public static string cut_tail(string origin, string[] tails)
        {
            foreach (string tail in tails)
            {
                if (origin.EndsWith(tail))
                    return origin.Substring(0, origin.Length - tail.Length);
            }
            return origin;
        }
    }
}
