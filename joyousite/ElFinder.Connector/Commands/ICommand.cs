using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Commands
{
    public interface ICommand
    {
        Responses.Response Execute();        
    }
}
