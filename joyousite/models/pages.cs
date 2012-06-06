using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace models
{
    public class Page : Model
    {
        public static new string[] columns = { "parent_id", "tree_id", "level", "ordering", "lft", "rgt", "title", "content", "is_published", "in_navigation"};
        public static new string table = "pages";

        private Page _parent;
        private List<Page> _children;
        private List<Page> _ancestors;

        public int id { get; set; }
        public int parent_id { get; set; }
        public int tree_id { get; set; }
        public int level { get; set; }
        public int ordering { get; set; }
        public int lft { get; set; }
        public int rgt { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public bool is_published { get; set; }
        public int in_navigation { get; set; }

        public Page()
        {
            parent_id = -1;
        }

        public Page parent
        {
            get
            {
                if (is_root)
                    return null;
                if (_parent != null)
                    return _parent;
                _parent = Page.get(parent_id);
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

        public List<Page> children
        {
            get
            {
                if (_children != null)
                    return _children;
                string sql = string.Format("SELECT * FROM pages WHERE lft > {0} AND rgt < {1} AND tree_id = {2} ORDER BY lft ASC, ordering ASC", lft, rgt, tree_id);
                OleDbDataReader reader = db.ExecuteReader(sql);
                _children = pages_from_reader(reader);
                return _children;
            }
        }

        public List<Page> ancestors
        {
            get
            {
                if (_ancestors != null)
                    return _ancestors;
                string sql = string.Format("SELECT * FROM pages WHERE lft < {0} AND rgt > {1} AND tree_id ={2} ORDER BY lft ASC", lft, rgt, tree_id);
                OleDbDataReader reader = db.ExecuteReader(sql);
                _ancestors = pages_from_reader(reader);
                return _ancestors;
            }
        }

        public static Page get(int id) 
        {
            string sql = "SELECT * FROM pages where id=@id";
            OleDbDataReader reader = db.ExecuteReader(sql, new Dictionary<string, object> {
                {"@id", id},
            });
            if (reader.HasRows)
            {
                return (Page)single_from_reader(reader, typeof(Page));
            }
            else
            {
                return null;
            }
        }

        public static List<Page> find(Dictionary<string, object> args)
        {
            string sql = "SELECT * FROM {0} WHERE {1}";
            List<Page> pages = new List<Page>();
            return pages;
        }

        public void save()
        {
            pre_save();
            insert();
        }

        protected void pre_save()
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
            }
            /*
            string sql = string.Format("UPDATE pages SET `rgt`=`rgt`+2 WHERE `rgt` >= {0} AND `tree_id` = {1}", lft, tree_id);
            db.ExecuteSql(sql);
            sql = string.Format("UPDATE pages SET `lft`=`lft`+2 WHERE `lft` >= {0} AND `tree_id` = {1}", lft, tree_id);
            db.ExecuteSql(sql);
            */
            add_space(parent.rgt, 2, tree_id);
        }

        public void update(Page page)
        {
        }

        public void delete()
        {
            int tree_width = rgt - lft + 1;
            string sql = string.Format("DELETE FROM pages WHERE `lft` >= {0} AND `rgt` <= {1} AND `tree_id` = {2}", lft, rgt, tree_id);
            db.ExecuteSql(sql);
            rm_space(rgt, tree_width, tree_id);
        }

        public void move_to(Page parent)
        {
            int tree_width = rgt - lft + 1;
            int p_rgt, delta, delta_level;
            if (parent == null)
            {
                // root node
                int p_tree_id = next_tree_id();
                delta = 1 - lft;
                delta_level = -level;
                string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1},`rgt`=`rgt`+{1},`level`=`level`+{2}, `tree_id`={3} WHERE `lft` >= {4} AND `rgt` <={5} AND `tree_id`={6}",
                    table, delta, delta_level, p_tree_id, lft, rgt, tree_id);
                db.ExecuteSql(sql);
                sql = string.Format("UPDATE {0} SET `parent_id`=-1 WHERE `id`={1}", table, id);
                db.ExecuteSql(sql);
                rm_space(rgt, tree_width, tree_id);
            }
            else if (parent.tree_id == tree_id)
            {
                p_rgt = parent.rgt;
                delta = p_rgt - lft - tree_width;
                delta_level = parent.level - 1;

                add_space(p_rgt, tree_width, parent.tree_id);
                string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1},`rgt`=`rgt`+{1},`level`=`level`+{2}, `tree_id`={3} WHERE `lft` >= {4} AND `rgt` <={5} AND `tree_id`={6}",
                    table, delta, delta_level, parent.tree_id, lft + tree_width, rgt + tree_width, tree_id);
                db.ExecuteSql(sql);
                sql = string.Format("UPDATE {0} SET `parent_id`={1} WHERE `id`={2}", table, parent.id, id);
                db.ExecuteSql(sql);
                rm_space(rgt + tree_width, tree_width, tree_id);
            }
            else
            {
                p_rgt = parent.rgt;
                delta = p_rgt - lft;
                delta_level = parent.level - 1;

                add_space(p_rgt, tree_width, parent.tree_id);
                string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1},`rgt`=`rgt`+{1},`level`=`level`+{2}, `tree_id`={3} WHERE `lft` >= {4} AND `rgt` <={5} AND `tree_id`={6}",
                    table, delta, delta_level, parent.tree_id, lft, rgt, tree_id);
                db.ExecuteSql(sql);
                sql = string.Format("UPDATE {0} SET `parent_id`={1} WHERE `id`={2}", table, parent.id, id);
                db.ExecuteSql(sql);
                rm_space(rgt, tree_width, tree_id);
            }
        }

        private void add_space(int rgt, int size, int tree_id)
        {
            string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1} WHERE `lft` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id);
            db.ExecuteSql(sql);
            sql = string.Format("UPDATE {0} SET `rgt`=`rgt`+{1} WHERE `rgt` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id);
            db.ExecuteSql(sql);
        }

        private void rm_space(int rgt, int size, int tree_id)
        {
            string sql = string.Format("UPDATE {0} SET `lft`=`lft`-{1} WHERE `lft` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id);
            db.ExecuteSql(sql);
            sql = string.Format("UPDATE {0} SET `rgt`=`rgt`-{1} WHERE `rgt` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id);
            db.ExecuteSql(sql);
        }

        public static List<Page> pages_from_reader(OleDbDataReader reader)
        {
            List<Page> pages = new List<Page>();
            while (reader.Read())
            {
                Page page = (Page)single_from_reader(reader, typeof(Page));
                pages.Add(page);
            }
            return pages;
        }

        private static int next_tree_id() 
        {
            string sql = "SELECT MAX(`tree_id`) FROM pages";
            var id = db.GetSingle(sql);
            if (id == null)
                return 0;
            return (int)id + 1;
        }
    }
}
