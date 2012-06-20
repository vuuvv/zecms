using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace vuuvv.db
{
    public abstract class Model
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true, CanBeNull=false)]
        public int id { get; set; }
    }

    public interface TreeModel<T>
    {
        [Column(CanBeNull=false)]
        public int lft { get; set; }

        [Column(CanBeNull=false)]
        public int rgt { get; set; }

        [Column(CanBeNull=false)]
        public int level { get; set; }

        [Association(ThisKey="parent_id", IsForeignKey=true)]
        public EntityRef<T> parent { get; set; }
    }
}
