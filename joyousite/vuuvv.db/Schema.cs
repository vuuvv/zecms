using System;
using System.Data;


namespace vuuvv.db
{
    public abstract class Schema : Attribute
    {
        public string name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Table : Schema 
    {
        public Column[] columns;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class Column : Schema
    {
        public Field field;
    }
}
