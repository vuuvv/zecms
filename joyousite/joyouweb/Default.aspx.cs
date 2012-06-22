using System;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlServerCe;

using vuuvv.data;
using vuuvv.page;
using vuuvv.page.entities;

namespace joyouweb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new PageManager{ model = new Page() };
            manager.save();
        }
    }
}
