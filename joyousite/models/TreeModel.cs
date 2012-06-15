using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace models
{
    public abstract class TreeModel : Model
    {
        public new static string table;
        public new static string[] columns = { "parent_id", "tree_id", "level", "lft", "rgt" };
        protected new static Dictionary<string, Field> _fields = new Dictionary<string, Field>()
        {   
            {"parent_id",  new IntegerField()},
            {"tree_id", new IntegerField()},
            {"level", new IntegerField()},
            {"lft", new IntegerField()},
            {"rgt", new IntegerField()}
        };

        static TreeModel()
        {
            foreach (var item in Model._fields)
            {
                _fields[item.Key] = item.Value;
            }
        }
    }
}
