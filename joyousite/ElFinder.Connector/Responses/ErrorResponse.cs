using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Responses
{
    public class ErrorResponse : Response
    {
        public override ContentType ContentType
        {
            get { return ContentType.Json; }
        }

        public string error { get; protected set; }

        public ErrorResponse(string error)
        {
            this.error = error;
        }
    }
}
