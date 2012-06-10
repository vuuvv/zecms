using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Responses
{
    public class PingResponse : Response
    {
        public override ContentType ContentType
        {
            get { return ContentType.Ping; }
        }
    }
}
