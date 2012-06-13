using System;
using System.Web;

namespace UrlRewrite
{
    class RewriteModule : IHttpModule
    {
        public virtual void Init(HttpApplication app)
        {
            app.AuthorizeRequest += new EventHandler(ModuleRewrite_AuthorizeRequest);
        }

        public virtual void Dispose() { }

        protected virtual void ModuleRewrite_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            dispatch(app.Context);
        }

        protected string cut_tail(string origin, string[] tails)
        {
            foreach (string tail in tails)
            {
                if (origin.EndsWith(tail))
                    return origin.Substring(0, origin.Length - tail.Length);
            }
            return origin;
        }

        protected void dispatch(HttpContext context)
        {
            string ext = ".aspx";
            string path = context.Request.Path.ToLower();
            if (!path.StartsWith("/admin/") && path.EndsWith(ext))
            {
                path = cut_tail(path, new string[] { "/index.aspx", "/default.aspx", ".aspx" });
                if (path == "")
                    path = "/";
                models.Page page = models.Page.from_slug(path);
                if (page != null)
                {
                    context.Items["page"] = page;
                    context.RewritePath("/allin/template.aspx");
                }
                else
                {
                    // TODO: raise the 404 error
                }
            }
        }
    }
}
