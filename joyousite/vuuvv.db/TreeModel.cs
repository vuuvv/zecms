using System;
using System.Collections.Generic;

namespace vuuvv.db
{
    public enum Position { first_child, last_child, left, right };
    public abstract class TreeModel : MyModel
    {
        [MyColumn]
        public int parent_id { get; set; }
        [MyColumn]
        public int tree_id { get; set; }
        [MyColumn]
        public int level { get; set; }
        [MyColumn]
        public int lft { get; set; }
        [MyColumn]
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

        private void pre_insert()
        {
            // A new node should be a leaf node
            if (is_root)
            {
                // root node
                lft = 1;
                rgt = 2;
                level = 0;
                tree_id = next_tree_id();
            }
            else
            {
                lft = parent.rgt;
                rgt = lft + 1;
                tree_id = parent.tree_id;
                level = parent.level + 1;
                add_space(parent.rgt, 2, tree_id);
            }
        }

        public override void insert() 
        {
            pre_insert();
            base.insert();
        }
        public void move_to(TreeModel target, Position position)
        {
            TreeModel parent = null;
            int t_lft = 1;
            int tree_width = rgt - lft + 1;
            int delta = t_lft - lft;
            int delta_level = -level;
            int t_parent_id = -1;
            int c_lft = lft;
            int c_rgt = rgt;
            int t_tree_id;
            if (target == null || (target.parent_id == -1 && (position == Position.left || position == Position.right)))
            {
                t_tree_id = next_tree_id();
            }
            else
            {
                // here make sure the parent not be null
                switch (position)
                {
                    case Position.first_child:
                        parent = target;
                        t_lft = parent.lft + 1;
                        break;
                    case Position.last_child:
                        parent = target;
                        t_lft = parent.rgt;
                        break;
                    case Position.left:
                        parent = target.parent;
                        t_lft = target.lft;
                        break;
                    case Position.right:
                        parent = target.parent;
                        t_lft = target.rgt + 1;
                        break;
                }
                delta = t_lft - lft;
                if (tree_id == target.tree_id && t_lft < lft)
                {
                    delta = t_lft - lft - tree_width;
                    c_lft = lft + tree_width;
                    c_rgt = rgt + tree_width;
                }
                t_tree_id = target.tree_id;
                delta_level = parent.level + 1 - level;
                t_parent_id = parent.id;
                add_space(t_lft, tree_width, t_tree_id);
            }
            tune(t_tree_id, delta, delta_level, c_lft, c_rgt, t_parent_id);
            rm_space(c_rgt, tree_width, tree_id);
        }

        public void move_to(TreeModel tree)
        {
            move_to(tree, Position.last_child);
        }

        private void tune(int t_tree_id, int delta, int delta_level, int c_lft, int c_rgt, int parent_id)
        {
            string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1},`rgt`=`rgt`+{1},`level`=`level`+{2}, `tree_id`={3} WHERE `lft` >= {4} AND `rgt` <={5} AND `tree_id`={6}",
                table.name, delta, delta_level, t_tree_id, c_lft, c_rgt, tree_id);
            db.execute(sql);
            sql = string.Format("UPDATE {0} SET `parent_id`={1} WHERE `id`={2}", table.name, parent_id, id);
            db.execute(sql);
        }

        private void add_space(int rgt, int size, int tree_id)
        {
            string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1} WHERE `lft` >= {2} AND `tree_id`={3}", table.name, size, rgt, tree_id);
            db.execute(sql);
            sql = string.Format("UPDATE {0} SET `rgt`=`rgt`+{1} WHERE `rgt` >= {2} AND `tree_id`={3}", table.name, size, rgt, tree_id);
            db.execute(sql);
        }

        private void rm_space(int rgt, int size, int tree_id)
        {
            string sql = string.Format("UPDATE {0} SET `lft`=`lft`-{1} WHERE `lft` >= {2} AND `tree_id`={3}", table.name, size, rgt, tree_id);
            db.execute(sql);
            sql = string.Format("UPDATE {0} SET `rgt`=`rgt`-{1} WHERE `rgt` >= {2} AND `tree_id`={3}", table.name, size, rgt, tree_id);
            db.execute(sql);
        }

        private static int next_tree_id()
        {
            string sql = "SELECT MAX(`tree_id`) FROM pages";
            var id = db.one<int>(sql);
            if (id == null)
                return 0;
            return id + 1;
        }
    }
}
