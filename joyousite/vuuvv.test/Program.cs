using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

using vuuvv.db;

namespace vuuvv.test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string cstr = @"Data Source=F:\\code\\joyousite\\joyousite\\joyouweb\\App_Data\\joyou.db";
            DataContext db = new DataContext(new SQLiteConnection(cstr));
            Table<test> tbl = db.GetTable<test>();
            test t = new test();
            t.name = "zheshishenme";
            t.age = 1001;
            tbl.InsertOnSubmit(t);
            db.SubmitChanges();
            */
        }
    }

    [Table(name = "test")]
    public class test
    {
        [Column]
        public int id;
        [Column]
        public string name;
        [Column]
        public int age;
    }
}
