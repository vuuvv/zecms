using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace models
{
    public class Test : Model
    {
        public static new string[] columns = { "content", "name", "age" };
        public static new string table = "test";
        public string name { get; set; }
        public int age { get; set; }
        public string content { get; set; }

        public void save()
        {
            insert();
        }
    }
}
