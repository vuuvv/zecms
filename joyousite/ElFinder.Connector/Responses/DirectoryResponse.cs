using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Responses
{
    public class DirectoryResponse : Response
    {
        public override ContentType ContentType
        {
            get { return ContentType.Json; }
        }

        public Cwd cwd { get; set; }

        public IList<Cdc> cdc { get; set; }

        public TreeNode tree { get; set; }

    }
}
