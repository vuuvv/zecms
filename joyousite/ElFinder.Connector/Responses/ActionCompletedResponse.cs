using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Responses
{
    public class ActionCompletedResponse : DirectoryResponse
    {
        public IEnumerable<string> select { get; set; }    
    }
}
