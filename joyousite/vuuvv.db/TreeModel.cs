using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    public class TreeModel : Model
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

        public int parent_id { get; set; }
        public int tree_id { get; set; }
        public int level { get; set; }
        public int lft { get; set; }
        public int rgt { get; set; }

        private TreeModel _parent;

        public TreeModel()
            : base()
        {
            parent_id = -1;
        }

        public TreeModel parent
        {
            get
            {
                if (is_root)
                    return null;
                if (_parent != null)
                    return _parent;
                _parent = ModelHelper.get<TreeModel>(parent_id);
                return _parent;
            }
        }

        public bool is_root
        {
            get
            {
                return parent_id == -1;
            }
        }

        public bool is_leaf
        {
            get
            {
                return rgt - lft == 1;
            }
        }
    }
}
