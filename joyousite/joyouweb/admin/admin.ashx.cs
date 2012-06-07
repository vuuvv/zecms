using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace joyouweb.admin
{
    /// <summary>
    /// Summary description for admin
    /// </summary>
    public class admin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string action = context.Request.Params["action"];
            Type t = this.GetType();
            t.GetMethod(action).Invoke(this, new object[] { context });
        }

        public void page_delete(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            models.Page.get(context.Request.Params["id"]).delete();
            context.Response.Redirect("pages.aspx");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}