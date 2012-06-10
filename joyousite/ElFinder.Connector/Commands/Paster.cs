using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector.Commands
{
    public class Paste : ICommand
    {
        public string Src { get; set; }

        public string Dst { get; set; }

        public IEnumerable<string> ToCopy { get; set; }

        public bool Cut { get; set; }

        public Paste(string src, string dst, string targets, string cut)
        {
            this.Src = src;

            this.Dst = dst;

            this.ToCopy = targets.Split(",".ToCharArray());

            this.Cut = !string.IsNullOrEmpty(cut) && cut == "1";

        }

        #region ICommand Members       

        public ElFinder.Connector.Responses.Response Execute()
        {
            var path = DirectoryUtils.GetFilePathByHash(Dst, Configuration.Configuration.RootPath);

            object error = null;

            object errData = null;

            foreach (var name in ToCopy)
            {
                var toCopy = DirectoryUtils.GetFilePathByHash(name, Configuration.Configuration.RootPath);

                try
                {
                    var fileInfo = new System.IO.FileInfo(toCopy);
                    if (Cut)
                    {
                        if (!(Src == Dst))
                            fileInfo.MoveTo(path + @"\" + fileInfo.Name);
                    }
                    else
                    {
                        if (!(Src == Dst))
                            fileInfo.CopyTo(path + @"\" + fileInfo.Name);
                        else
                            fileInfo.Duplicate();
                    }

                }
                catch
                {
                    var dirInfo = new System.IO.DirectoryInfo(toCopy);
                    if (Cut)
                    {
                        try
                        {
                            if (!(Src == Dst))
                                dirInfo.MoveTo(path + @"\" + dirInfo.Name);
                        }
                        catch
                        {
                            error = "Can't move directory";
                        }
                    }
                    else
                    {
                        try
                        {
                            if (!(Src == Dst))
                                dirInfo.CopyTo(path + @"\" + dirInfo.Name);
                            else
                                dirInfo.Duplicate();
                        }
                        catch
                        {
                            error = "Can't copy directory";                            
                        }
                    }
                }
            }

            var folder = new Utils.Folder(path);

            var response = new Responses.DirectoryResponse();

            response.cwd = folder.Cwd;

            response.cdc = folder.Contents;

            response.tree = folder.Tree;

            response.error = error;

            response.errorData = errData;

            return response;
        }

        #endregion
    }
}
