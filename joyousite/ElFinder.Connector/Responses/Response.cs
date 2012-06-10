using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Responses
{
    public abstract class Response
    {
        public abstract ContentType ContentType { get; }

        public object error { get; set; }

        public object errorData { get; set; }
    }
}
