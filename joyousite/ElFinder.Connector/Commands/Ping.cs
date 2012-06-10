using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Ping : ICommand
    {
        public Ping()
        {
            
        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            return new Responses.PingResponse();
        }
        #endregion
    }
}
