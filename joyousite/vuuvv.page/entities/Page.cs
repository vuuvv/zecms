using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

using vuuvv.data;

namespace vuuvv.page.entities
{
    public class Page : Model
    {
        public virtual Page parent { get; set; }
        public virtual int tree_id { get; set; }
        public virtual int lft { get; set; }
        public virtual int rgt { get; set; }
        public virtual int level { get; set; }
        public virtual string title { get; set; }
        public virtual string slug { get; set; }
        public virtual string content { get; set; }
        public virtual bool is_published { get; set; }
        public virtual int in_navigation { get; set; }

        public virtual bool is_root
        {
            get
            {
                return parent == null;
            }
        }

        public virtual bool is_leaf
        {
            get
            {
                return rgt - lft == 1;
            }
        }

        public virtual IQueryable<Page> table
        {
            get
            {
                return session.Query<Page>();
            }
        }

        public override void save() 
        {
            if (is_root)
            {
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
                add_space("Page", parent.rgt, 2, tree_id);
            }
            base.save();
        }

        private void add_space(string table, int rgt, int size, int tree_id)
        {
            session.CreateSQLQuery(string.Format("UPDATE {0} SET `lft`=`lft`+{1} WHERE `lft` >= {2} AND `tree_id`={3}",
                table,size, rgt, tree_id)).ExecuteUpdate();
            session.CreateSQLQuery(string.Format("UPDATE {0} SET `rgt`=`rgt`+{1} WHERE `rgt` >= {2} AND `tree_id`={3}",
                table,size, rgt, tree_id)).ExecuteUpdate();
        }

        private void rm_space(string table, int size, int tree_id)
        {
            session.CreateSQLQuery(string.Format("UPDATE {0} SET `lft`=`lft`-{1} WHERE `lft` >= {2} AND `tree_id`={3}",
                table, size, rgt, tree_id)).ExecuteUpdate();
            session.CreateSQLQuery(string.Format("UPDATE {0} SET `rgt`=`rgt`-{1} WHERE `rgt` >= {2} AND `tree_id`={3}",
                table, size, rgt, tree_id)).ExecuteUpdate();
        }

        private int next_tree_id()
        {
            int id = 0;
            try
            {
                id = table.Max(x => x.tree_id);
            }
            catch (Exception)
            {
            }
            return id + 1;
        }
    }
}
