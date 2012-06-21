using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    [MyTable(name = "pages")]
    public class MyPage : MyTreeModel
    {
        [MyColumn]
        public string slug { get; set; }
        [MyColumn]
        public string title { get; set; }
        [MyColumn]
        public string content { get; set; }
        [MyColumn]
        public bool is_published { get; set; }
        [MyColumn]
        public int in_navigation { get; set; }
    }
}
