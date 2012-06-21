using System;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlServerCe;
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
            /*
            Test t = new Test();
            t.name = "Jack";
            t.age = 18;
            t.insert();
            Test t1 = ModelHelper.get<Test>(t.id);
            */
            AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
            var cstr = string.Format("Data Source={0};Persist Security Info=False;", Server.MapPath("~/App_Data/joyou.sdf"));
            DataContext db = new DataContext(new SqlCeConnection(cstr));
            var table = db.GetTable<Page>();
            var pages = from c in table join o in table on c.parent_id equals o.id where c.id == 18 select c;
            foreach (var p in pages)
            {
                Response.Write(p.parent.id);
            }
            /*
            var page = new Page();
            page.level = 0;
            page.lft = 1;
            page.rgt = 2;
            page.parent_id = null;
            page.title = "home";
            page.slug = "home";
            page.content = "home";

            table.InsertOnSubmit(page);
            db.SubmitChanges();

            var p1 = new Page();
            p1.level = 1;
            p1.lft = 3;
            p1.rgt = 4;
            p1.parent = page;
            p1.title = "about";
            p1.slug = "about";
            p1.content = "about";

            table.InsertOnSubmit(p1);
            db.SubmitChanges();
            var tests = from c in table select c;
            foreach (var tt in tests)
            {
                Response.Write(tt.title);
            }
            */
            //DataContext db = new DataContext(new SqlCeConnection(string.Format("Data Source={0}", Server.MapPath("~/App_Data/joyou.sdf"))));
        }
    }
    [Table(Name="tree")]
    public class Tree
    {
        [Column(IsPrimaryKey = true)]
        public int TreeId;

        [Column]
        public int ParentId;
        public EntityRef<Tree> _Parent;
        [Association(ThisKey = "TreeId")]
        public Tree Parent
        {
            get
            {
                return this._Parent.Entity;
            }
            set
            {
                this._Parent.Entity = value;
            }
        }
    }

    [Table(Name = "Orders")]
    public class Order
    {
        [Column(IsPrimaryKey = true)]
        public int OrderID;
        [Column]
        public string CustomerID;
        private EntityRef<Customer> _Customer;
        [Association(Storage = "_Customer", ThisKey = "CustomerID")]
        public Customer Customer
        {
            get { return this._Customer.Entity; }
            set { this._Customer.Entity = value; }
        }
    }

    [Table(Name = "Customers")]
    public partial class Customer
    {
        [Column(IsPrimaryKey = true)]
        public string CustomerID;
        // ...
        private EntitySet<Order> _Orders;
        [Association(Storage = "_Orders", OtherKey = "CustomerID")]
        public EntitySet<Order> Orders
        {
            get { return this._Orders; }
            set { this._Orders.Assign(value); }
        }
    }
}
