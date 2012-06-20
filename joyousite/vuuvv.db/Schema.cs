using System;
using System.Data;


namespace vuuvv.db
{
    public abstract class Schema : Attribute
    {
        public string name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MyTable : Schema 
    {
        public MyColumn[] columns;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MyColumn : Schema
    {
        public Field field;
    }
}
