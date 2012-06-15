using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    public abstract class TreeModel : Model
    {
        public new static Dictionary<string, Field> fields = new Dictionary<string, Field>()
        {   
            {"parent_id",  new IntegerField()},
            {"tree_id", new IntegerField()},
            {"level", new IntegerField()},
            {"lft", new IntegerField()},
            {"rgt", new IntegerField()}
        };

        static TreeModel()
        {
            foreach (var item in Model.fields)
            {
                fields[item.Key] = item.Value;
            }
        }
    }
}
