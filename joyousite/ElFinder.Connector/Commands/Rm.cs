using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Rm : ICommand
    {
        public string Current { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> ToDelete { get; set; }

        public Rm(string current, string toDelete)
        {
            this.Current = current;

            var arr = toDelete.Split(",".ToCharArray());

            this.ToDelete = arr;

        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetFilePathByHash(Current, Configuration.Configuration.RootPath);

            foreach (var name in ToDelete)
            {
                var toDelete = DirectoryUtils.GetFilePathByHash(name, Configuration.Configuration.RootPath);

                try
                {
                    System.IO.File.Delete(toDelete);
                }
                catch
                {
                    var dir = new System.IO.DirectoryInfo(toDelete);
                    dir.DeleteWithFiles();
                }
            }

            var folder = new Utils.Folder(path);

            var response = new Responses.DirectoryResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            response.tree = folder.Tree;

            return response;
        }

        #endregion
    }
}
