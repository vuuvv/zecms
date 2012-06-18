using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    [Table(name = "pages")]
    public class Page : TreeModel
    {
        [Column]
        public string slug { get; set; }
        [Column]
        public string title { get; set; }
        [Column]
        public string content { get; set; }
        [Column]
        public bool is_published { get; set; }
        [Column]
        public int in_navigation { get; set; }
    }
}
