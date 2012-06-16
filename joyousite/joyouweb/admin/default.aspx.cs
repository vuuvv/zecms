﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using vuuvv.db;

namespace joyouweb.admin
{
    public partial class _default : System.Web.UI.Page
    {
        private DBHelper db;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = DBHelper.get();
            db.execute("SELECT * FROM pages");
        }
    }
}