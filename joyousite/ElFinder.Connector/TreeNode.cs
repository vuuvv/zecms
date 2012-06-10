using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector
{
    public class TreeNode
    {
        public string name { get; set; }

        public string hash { get; set; }

        public bool read { get; set; }

        public bool write { get; set; }

        public IEnumerable<TreeNode> dirs { get; set; }

    }
}
