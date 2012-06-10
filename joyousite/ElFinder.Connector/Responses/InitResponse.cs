using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Responses
{
    public class InitResponse : DirectoryResponse
    {
        public object @params
        {
            get
            {
                return
                    new
                    {
                        url = Configuration.Configuration.RootUrl,
                        dotFiles = Configuration.Configuration.DotFiles,
                        uplMaxSize = Configuration.Configuration.UplMaxSize,
                        extract = new string[] { },
                        archives = new string[] { }
                    };
            }
        }

        public IEnumerable<string> disabled
        {
            get
            {
                return Configuration.Configuration.DisabledCommands;
            }
        }
    
    }
}
