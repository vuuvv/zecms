using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ElFinder.Connector.Utils
{
    public static class DirectoryUtils
    {
        public static string GetRelativePathByHash(string hash, string path)
        {
            var result = string.Empty;

            if (string.Compare(hash, path.Hash()) == 0)
            {
                return path;
            }

            var directoryInfo = new DirectoryInfo(path);
            var dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (string.Compare(hash, (path + "\\" + dir.Name).Hash()) == 0)
                {
                    return path + @"\" + dir.Name;
                }

                result = GetRelativePathByHash(hash, path + @"\" + dir.Name);
                if (result != string.Empty)
                    return result;
            }

            return result;
        }

        public static string GetFilePathByHash(string hash, string path)
        {
            var result = string.Empty;

            if (string.Compare(hash, path.Hash()) == 0)
            {
                return path;
            }

            var directoryInfo = new DirectoryInfo(path);
            var dirs = directoryInfo.GetDirectories();
            var files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                if (string.Compare(hash, (path + "\\" + file.Name).Hash()) == 0)
                {
                    return path + @"\" + file.Name;
                }
            }
            
            foreach (var dir in dirs)
            {
                if (string.Compare(hash, (path + "\\" + dir.Name).Hash()) == 0)
                {
                    return path + @"\" + dir.Name;
                }

                result = GetFilePathByHash(hash, path + @"\" + dir.Name);
                if (result != string.Empty)
                    return result;
            }

            return result;
        }
        public static string GetFullPath(string relPath)
        {
            return string.Format("{0}/{1}", Configuration.Configuration.RootPath, relPath);
        }

        public static string RelativePath(this DirectoryInfo dir, string path)
        {
            return path + @"\" + dir.Name;
        }

        public static string PathFromRoot(this DirectoryInfo dir)
        {
            var path = dir.Name;

            
            string result = dir.Name;
            while (dir.FullName != Configuration.Configuration.RootPath)
            {                
                var parent = dir.Parent;
                result = parent.Name + @"\" + result;
                dir = parent;
            }
            
            return result;

        }

        public static long Size(this DirectoryInfo d)
        {
            long Size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                Size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                Size += di.Size();
            }
            return (Size);
        }
        public static string Hash(this string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        public static string Duplicate(this DirectoryInfo dir)
        {
            var parentPath = dir.Parent.FullName;

            var name = dir.Name;

            var newName = string.Format(@"{0}\{1} copy", parentPath, name);

            if (!Directory.Exists(newName))
            {
                dir.CopyTo(newName);
            }
            else
            {
                for (int i = 1; i < 100; i++)
                {
                    newName = string.Format(@"{0}\{1} copy {2}", parentPath, name, i);
                    if (!Directory.Exists(newName))
                    {
                        dir.CopyTo(newName);
                        break;
                    }
                }
            }

            return newName;
        }

        public static void CopyTo(this DirectoryInfo dir, string destPath)
        {
            var newDir = Directory.CreateDirectory(destPath);
            var files = dir.GetFiles();
            var dirs = dir.GetDirectories();
            foreach (var file in files)
            {
                file.CopyTo(destPath + @"\" + file.Name);
            }
            foreach (var d in dirs)
            {
                d.CopyTo(destPath + @"\" + d.Name);
            }
        }

        public static void DeleteWithFiles(this DirectoryInfo dir)
        {
            var files = dir.GetFiles();
            var dirs = dir.GetDirectories();
            foreach (var file in files)
            {
                file.Delete();
            }
            foreach (var d in dirs)
            {
                d.DeleteWithFiles();                
            }
            dir.Delete();
        }
    }
}
