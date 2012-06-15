using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.page
{
    public class Template : System.Web.UI.Page
    {
        public models.Page page { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            page = (models.Page)Context.Items["page"];
        }
    }
}
