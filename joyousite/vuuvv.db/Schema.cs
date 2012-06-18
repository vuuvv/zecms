using System;
using System.Data;


namespace vuuvv.db
{
    public abstract class Schema : Attribute
    {
        public string name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Table : Schema 
    {
        public Column[] columns;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class Column : Schema
    {
        public DbType dbtype { get; set;}
    }
}
