using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace vuuvv.db
{
    public abstract class Model
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true, CanBeNull=false)]
        public int id { get; set; }

        public DataContext db
        {
            get
            {
                return WebDBHelper.db;
            }
        }
    }

    public abstract class TreeModel : Model
    {
        [Column(CanBeNull=false)]
        public int lft { get; set; }

        [Column(CanBeNull=false)]
        public int rgt { get; set; }

        [Column(CanBeNull=false)]
        public int level { get; set; }

        [Association(ThisKey="parent_id", IsForeignKey=true)]
        public EntityRef<TreeModel> parent { get; set; }
    }
}
