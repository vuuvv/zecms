using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElFinder.Connector.Utils
{
    public class Folder
    {
        public string Path { get; protected set; }

        public System.IO.DirectoryInfo DirectoryInfo { get; protected set; }

        public Folder(string path)
        {
            this.Path = path;

            var folder = new System.IO.DirectoryInfo(Path);

            this.DirectoryInfo = folder;

            _cwd = new Cwd(folder);

            _contents = new List<Cdc>();

            _tree = new TreeNode();

            FillCdcs(ref _contents, folder);

            FillTree(ref _tree, new System.IO.DirectoryInfo(Configuration.Configuration.RootPath));
            
        }

        private Cwd _cwd;

        public Cwd Cwd
        {
            get
            {                
                return _cwd;
            }
        }

        private IList<Cdc> _contents;

        public IList<Cdc> Contents
        {
            get
            {
                return _contents;
            }
        }

        private TreeNode _tree;

        public TreeNode Tree
        {
            get
            {
                return _tree;
            }
        }

        private void FillCdcs(ref IList<Cdc> contents, System.IO.DirectoryInfo directoryInfo)
        {
            var dirs = directoryInfo.GetDirectories();

            foreach (var dir in dirs)
            {
                var cdc = new Cdc(dir);

                contents.Add(cdc);
            }

            var files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                var fileCdc = new Cdc(file);                
                contents.Add(fileCdc);
            }
        }

        private void FillTree(ref TreeNode tree, System.IO.DirectoryInfo directoryInfo)
        {
            tree.name = directoryInfo.Name;
            tree.hash = directoryInfo.FullName.Hash();
            tree.read = tree.write = true;
            IList<TreeNode> treeDirs = new List<TreeNode>();
            var dirs = directoryInfo.GetDirectories();

            foreach (var dir in dirs)
            {
                var child = new TreeNode();
                child.name = dir.Name;
                child.hash = dir.FullName.Hash();
                child.read = child.write = true;
                treeDirs.Add(child);

                FillTree(ref child, dir);
            }

            tree.dirs = treeDirs;
        }
    }
}
