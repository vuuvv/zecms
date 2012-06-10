using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Upload : ICommand
    {
        public string Current { get; set; }

        public System.Web.HttpFileCollection Files { get; set; }

        public Upload(string current, System.Web.HttpFileCollection files)
        {
            this.Current = current;
            this.Files = files;
        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetRelativePathByHash(Current, Configuration.Configuration.RootPath);

            

            IList<string> select = new List<string>();

            var error = string.Empty;

            for (var i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                if (file.ContentLength < 1)
                    continue;

                var nameArr = file.FileName.Split(".".ToCharArray());

                var ext = "";
                if (nameArr.Length > 1)
                {
                    ext = "." + nameArr[nameArr.Length - 1];                                        
                }

                var mime = FileUtils.GetMimeType(ext);
                

                if (Configuration.Configuration.DisabledMimeTypes.Contains(mime))
                {
                    error += string.Format("{0} type is unacceptable\n", file.FileName);
                }
                else
                {
                    file.SaveAs(string.Format(@"{0}\{1}", path, file.FileName));
                }
                select.Add(string.Format(@"{0}\{1}", path, file.FileName).Hash());
            }

            var response = new Responses.ActionCompletedResponse();

            var folder = new Utils.Folder(path);

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            response.select = select;

            response.error = error;

            //response.tree = folder.Tree;

            return response;
        }

        #endregion
    }
}
