using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Open : ICommand
    {
        public string Target { get; set; }

        public Open(string target)
        {
            this.Target = target;
        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetRelativePathByHash(Target, Configuration.Configuration.RootPath);

            var folder = new Utils.Folder(path);

            var response = new Responses.DirectoryResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            return response;
        }

        #endregion
    }
}
