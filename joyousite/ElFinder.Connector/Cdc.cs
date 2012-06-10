using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;

namespace ElFinder.Connector
{
    public class Cdc
    {
        public string name { get; set; }

        public string hash { get; set; }

        public string url { get; set; }

        public string date { get; set; }

        public string mime { get; set; }

        public long size { get; set; }

        public bool read { get; set; }

        public bool write { get; set; }

        public bool rm { get; set; }

        public Cdc(System.IO.DirectoryInfo dir)
        {
            this.date = dir.LastWriteTime.ToShortDateString();
            this.name = dir.Name;
            this.read = this.write = this.rm = true;
            this.mime = "directory";
            this.size = dir.Size();
            this.hash = dir.FullName.Hash();
            this.url = "/" + dir.PathFromRoot().Replace(@"\", "/");
        }

        public Cdc(System.IO.FileInfo file)
        {
            this.date = file.LastWriteTime.ToShortDateString();
            this.name = file.Name;
            this.mime = file.GetMimeType();
            this.size = file.Length;
            this.read = this.write = this.rm = true;
            this.hash = file.FullName.Hash();
            this.url = "/" + file.Directory.PathFromRoot().Replace(@"\", "/") + "/" + file.Name;
        }
    }
}
