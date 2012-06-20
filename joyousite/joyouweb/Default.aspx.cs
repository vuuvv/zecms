using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Data.SqlServerCe;

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
            /*
            Test t = new Test();
            t.name = "Jack";
            t.age = 18;
            t.insert();
            Test t1 = ModelHelper.get<Test>(t.id);
            */
            AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
            var cstr = string.Format("Data Source={0};Persist Security Info=False;", Server.MapPath("~/App_Data/joyou.sdf"));
            try
            {
                DataContext db = new DataContext(new SqlCeConnection(cstr));
                var page = new Page();
                var table = db.GetTable<Page>();
                table.InsertOnSubmit(page);
                db.SubmitChanges();
                var tests = from c in table select c;
                foreach (var tt in tests)
                {
                    Response.Write(tt.title);
                }
            }
            catch (Exception err)
            {
                Response.Write(cstr);
                Response.Write(err.Message);
            }
            //DataContext db = new DataContext(new SqlCeConnection(string.Format("Data Source={0}", Server.MapPath("~/App_Data/joyou.sdf"))));
        }
    }

    class Page : vuuvv.db.Model, vuuvv.db.TreeModel<Page>
    {
        [Column(CanBeNull=false)]
        public string title { get; set; }

        [Column(CanBeNull=false)]
        public string slug { get; set; }

        [Column(CanBeNull=false)]
        public string content { get; set; }
    }
}
