﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace models
{
    public enum Position { first_child, last_child, left, right };
    public class Page : Model
    {
        public static new string[] columns = { "parent_id", "tree_id", "level", "lft", "rgt", "title", "content", "is_published", "in_navigation"};
        public static new string table = "pages";

        private Page _parent;
        private List<Page> _children;
        private List<Page> _ancestors;

        public int parent_id { get; set; }
        public int tree_id { get; set; }
        public int level { get; set; }
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

        public bool is_leaf
        {
            get
            {
                return rgt - lft == 1;
            }
        }

        public List<Page> children
        {
            get
            {
                if (_children != null)
                    return _children;
                string sql = string.Format("SELECT * FROM pages WHERE lft > {0} AND rgt < {1} AND tree_id = {2} ORDER BY lft ASC", lft, rgt, tree_id);
                OleDbDataReader reader = db.ExecuteReader(sql);
                _children = pages_from_reader(reader);
                return _children;
            }
        }

        public static List<Page> all
        {
            get
            {
                string sql = "SELECT * FROM pages ORDER BY tree_id ASC, lft ASC";
                OleDbDataReader reader = db.ExecuteReader(sql);
                return pages_from_reader(reader);
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

        public static Page get(object id) 
        {
            string sql = "SELECT * FROM pages where id=@id";
            OleDbDataReader reader = db.ExecuteReader(sql, new Dictionary<string, object> {
                {"@id", Convert.ToInt32(id)},
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
            //string sql = "SELECT * FROM {0} WHERE {1}";
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

        public void move_to(Page target, Position position)
        {
            Page parent = null;
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

        public void move_to(Page page)
        {
            move_to(page, Position.last_child);
        }

        private void tune(int t_tree_id, int delta, int delta_level, int c_lft, int c_rgt, int parent_id)
        {
            string sql = string.Format("UPDATE {0} SET `lft`=`lft`+{1},`rgt`=`rgt`+{1},`level`=`level`+{2}, `tree_id`={3} WHERE `lft` >= {4} AND `rgt` <={5} AND `tree_id`={6}",
                table, delta, delta_level, t_tree_id, c_lft, c_rgt, tree_id);
            db.ExecuteSql(sql);
            sql = string.Format("UPDATE {0} SET `parent_id`={1} WHERE `id`={2}", table, parent_id, id);
            db.ExecuteSql(sql);
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

        private List<string> sql_add_space(int rgt, int size, int tree_id)
        {
            List<string> sqls = new List<string>();
            sqls.Add(string.Format("UPDATE {0} SET `lft`=`lft`+{1} WHERE `lft` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id));
            sqls.Add(string.Format("UPDATE {0} SET `rgt`=`rgt`+{1} WHERE `rgt` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id));
            return sqls;
        }

        private List<string> sql_rm_space(int rgt, int size, int tree_id)
        {
            List<string> sqls = new List<string>();
            sqls.Add(string.Format("UPDATE {0} SET `lft`=`lft`-{1} WHERE `lft` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id));
            sqls.Add(string.Format("UPDATE {0} SET `rgt`=`rgt`-{1} WHERE `rgt` >= {2} AND `tree_id`={3}", table, size, rgt, tree_id));
            return sqls;
        }

        private List<string> sql_tune(int t_tree_id, int delta, int delta_level, int c_lft, int c_rgt, int parent_id)
        {
            List<string> sqls = new List<string>();
            sqls.Add(string.Format("UPDATE {0} SET `lft`=`lft`+{1},`rgt`=`rgt`+{1},`level`=`level`+{2}, `tree_id`={3} WHERE `lft` >= {4} AND `rgt` <={5} AND `tree_id`={6}",
                table, delta, delta_level, t_tree_id, c_lft, c_rgt, tree_id));
            sqls.Add(string.Format("UPDATE {0} SET `parent_id`={1} WHERE `id`={2}", table, parent_id, id));
            return sqls;
        }

        public static List<Page> pages_from_reader(OleDbDataReader reader)
        {
            List<Page> pages = new List<Page>();
            while (reader.Read())
            {
                Page page = (Page)fetch_data_from_reader(reader, typeof(Page));
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