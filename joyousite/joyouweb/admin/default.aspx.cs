using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dbutils;

namespace joyouweb.admin
{
    public partial class _default : System.Web.UI.Page
    {
        private DbHelperOleDb db;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = (DbHelperOleDb)Application["dbhelper"];
            db.ExecuteSql("SELECT * FROM pages");
        }
    }
}