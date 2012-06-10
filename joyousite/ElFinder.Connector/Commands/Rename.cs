using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Rename : ICommand
    {
        public string Target { get; set; }

        public string Current { get; set; }

        public string Name { get; set; }

        public Rename(string current, string target, string name)
        {
            this.Target = target;
            this.Current = current;
            this.Name = name;
        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetRelativePathByHash(Current, Configuration.Configuration.RootPath);

            var toRename = DirectoryUtils.GetFilePathByHash(Target, Configuration.Configuration.RootPath);

            var isDir = false;

            try
            {
                System.IO.File.Move(toRename, path + "/" + Name);
            }
            catch
            {
                System.IO.Directory.Move(toRename, path + "/" + Name);
                isDir = true;
            }

            var folder = new Utils.Folder(path);

            var response = new Responses.DirectoryResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            if (isDir)
            {
                response.tree = folder.Tree;
            }

            return response;
        }

        #endregion
    }
}
