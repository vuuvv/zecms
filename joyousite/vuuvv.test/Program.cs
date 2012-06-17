using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Data.OleDb;

namespace vuuvv.test
{
    class Program
    {
        static void Main(string[] args)
        {
            string cstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\\code\\joyousite\\joyousite\\joyouweb\\App_Data\\joyou.mdb";
            DataContext db = new DataContext(new OleDbConnection(cstr));
            Table<test> tbl = db.GetTable<test>();
            test t = new test();
            t.name = "zheshishenme";
            t.age = 1001;
            tbl.InsertOnSubmit(t);
            db.SubmitChanges();
        }
    }

    [Table(Name = "test")]
    public class test
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id;
        [Column]
        public string name;
        [Column]
        public int age;
    }
}
