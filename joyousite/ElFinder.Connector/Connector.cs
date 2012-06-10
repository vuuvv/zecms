using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ElFinder.Connector.Responses;
namespace ElFinder.Connector
{
    public class Connector : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Configuration.Configuration.Init(context);

            var dispatcher = new CommandDispatcher(context.Request);

            var response = dispatcher.DispatchCommand();

            switch (response.ContentType)
            {
                case ContentType.Json:
                    var js = new JavaScriptSerializer();
                    //context.Response.AppendHeader("Content-Type", "application/json");
                    context.Response.Write(js.Serialize(response));                    
                    break;
                case ContentType.Ping:
                    context.Response.End();
                    break;
            }
        }

        #endregion
    }
}
