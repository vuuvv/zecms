using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SQLite;
using vuuvv.db;

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
            Test t = new Test();
            t.name = "Jack";
            t.age = 18;
            t.save();
            Test t1 = ModelHelper.get<Test>(t.id);
        }
    }

    [Table(name = "test")]
    class Test : Model
    {
        [Column]
        public string name { get; set; }
        [Column]
        public int age { get; set; }
    }
}
