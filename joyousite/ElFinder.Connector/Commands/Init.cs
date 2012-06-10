using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Commands
{
    public class Init : ICommand
    {
        #region ICommand Members

        public ElFinder.Connector.Responses.Response Execute()
        {
            var folder = new Utils.Folder(Configuration.Configuration.RootPath);

            var response = new Responses.InitResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            response.tree = folder.Tree;

            return response;
        }

        #endregion
    }
}
