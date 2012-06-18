using System;
using System.Collections.Generic;

namespace vuuvv.db
{
    public class TreeModel : Model
    {
        [Column]
        public int parent_id { get; set; }
        [Column]
        public int tree_id { get; set; }
        [Column]
        public int level { get; set; }
        [Column]
        public int lft { get; set; }
        [Column]
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
