using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElFinder.Connector.Utils;
namespace ElFinder.Connector
{
    public class Cwd
    {
        public string name { get; set; }

        public string hash { get; set; }

        public string rel { get; set; }

        public string date { get; set; }

        public string mime { get { return "directory"; } }

        public int size { get; set; }

        public bool read { get; set; }

        public bool write { get; set; }

        public bool rm { get; set; }        

        public Cwd(System.IO.DirectoryInfo dir)
        {
            this.date = dir.LastWriteTime.ToShortDateString();
            this.name = dir.Name;
            this.read = this.write = this.rm = true;
            this.hash = dir.FullName.Hash();
            this.size = 10;
            this.rel = dir.PathFromRoot();
        }
    }
}
