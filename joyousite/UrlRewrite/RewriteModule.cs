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
            //app.Context.Response.Write(app.Context.Request.Path);
        }
    }
}
