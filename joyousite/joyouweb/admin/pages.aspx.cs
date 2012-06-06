using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using models;

namespace joyouweb.admin
{
    public partial class pages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            models.Page p = models.Page.get(96);
            p.move_to(models.Page.get(94), Position.left);
        }
    }
}