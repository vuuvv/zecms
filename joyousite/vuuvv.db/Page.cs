using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace vuuvv.db
{
    [Table(Name="pages")]
    public class Page
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true, CanBeNull=false)]
        public int id { get; set; }

        [Column(CanBeNull=false)]
        public int lft { get; set; }

        [Column(CanBeNull=false)]
        public int rgt { get; set; }

        [Column(CanBeNull=false)]
        public int level { get; set; }

        [Column(CanBeNull = true)]
        public int? parent_id { get; set; }

        private EntityRef<Page> _parent;
        [Association(ThisKey="parent_id", OtherKey="id")]
        public Page parent 
        {
            get 
            { 
                return this._parent.Entity; 
            } 
            set
            {
                this._parent.Entity = value;
            } 
        }

        [Column(CanBeNull=false)]
        public string slug { get; set; }

        [Column(CanBeNull=false)]
        public string title { get; set; }

        [Column(CanBeNull=false)]
        public string content { get; set; }
    }
}
