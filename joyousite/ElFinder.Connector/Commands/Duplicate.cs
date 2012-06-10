using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Duplicate : ICommand
    {
        public string Target { get; set; }

        public string Current { get; set; }

        public Duplicate(string current, string target)
        {
            this.Target = target;
            this.Current = current;            
        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetRelativePathByHash(Current, Configuration.Configuration.RootPath);

            var toDuplicate = DirectoryUtils.GetFilePathByHash(Target, Configuration.Configuration.RootPath);

            var isDir = false;

            var newName = string.Empty;

            try
            {
                var fileInfo = new System.IO.FileInfo(toDuplicate);
                newName = fileInfo.Duplicate();
                newName = newName.Hash();                
            }
            catch
            {
                var dirInfo = new System.IO.DirectoryInfo(toDuplicate);
                newName = dirInfo.Duplicate();
                newName = newName.Hash();
                isDir = true;
            }

            var folder = new Utils.Folder(path);

            var response = new Responses.ActionCompletedResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            response.select = new string[] { newName };

            if (isDir)
            {
                response.tree = folder.Tree;
            }

            return response;
        }

        #endregion
    }
}
