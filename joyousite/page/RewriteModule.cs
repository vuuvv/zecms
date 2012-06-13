using System;
using System.Web;

namespace vuuvv.page
{
    class RewriteModule : IHttpModule
    {
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
    }
}
