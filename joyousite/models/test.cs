using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using dbutils;

namespace models
{
    public class Test
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string content { get; set; }

        public void save()
        {
            string sql = "INSERT INTO test (`content`,`name`, `age`) VALUES (@content, @name, @age)";
            DbHelperOleDb.get().ExecuteSql(sql, new Dictionary<string, object> {
                {"@content", this.content},
                {"@name", this.name},
                {"@age", this.age},
            });
        }
    }
}
