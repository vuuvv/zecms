using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class MkDir : ICommand
    {
        public string Current { get; set; }

        public string Name { get; set; }

        public MkDir(string current, string name)
        {
            this.Current = current;

            this.Name = name;
        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetRelativePathByHash(Current, Configuration.Configuration.RootPath);

            System.IO.Directory.CreateDirectory(path + @"\" + Name);

            var folder = new Utils.Folder(path);

            var response = new Responses.ActionCompletedResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            response.tree = folder.Tree;

            response.select = new string[] { (path + @"\" + Name).Hash() };

            return response;
        }

        #endregion
    }
}
