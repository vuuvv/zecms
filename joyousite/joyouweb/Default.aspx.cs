﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace joyouweb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var db = vuuvv.db.DBHelper.get();
            int id = db.insert("test", "INSERT INTO test (name, age) VALUES(@name, @age)", new Dictionary<string, object>()
            {
                {"@name", "Mike"},
                {"@age", 38},
            });
            Response.Write(id);
        }
    }
}
