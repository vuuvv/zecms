using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace joyouweb.allin
{
    public partial class template : System.Web.UI.Page
    {
        public models.Page page;
        protected void Page_Load(object sender, EventArgs e)
        {
            page = (models.Page)Context.Items["page"];
        }
    }
}