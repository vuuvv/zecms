using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;

namespace joyouweb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            string t = vuuvv.db.TreeModel.table;
            var db = vuuvv.db.DBHelper.get();
            int id = db.insert("test", "INSERT INTO test (name, age) VALUES(@name, @age)", new Dictionary<string, object>()
            {
                {"@name", "Mike"},
                {"@age", 38},
            });
            Response.Write(id);
            */
            vuuvv.db.ModelHelper.get<vuuvv.db.Page>(1);
        }
    }
}
