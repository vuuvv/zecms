using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ElFinder.Connector.Utils
{
    public static class FileUtils
    {
        public static string GetMimeType(this FileInfo file)
        {
            return GetMimeType(file.Extension.ToLower());
        }

        public static string GetMimeType(string ext)
        {
            string mimeType = "application/unknown";            
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static string Duplicate(this FileInfo file)
        {
            var parentPath = file.DirectoryName;

            var name = file.Name;

            var ext = string.Empty;

            var nameArr = name.Split(".".ToCharArray());

            if (nameArr.Length > 1)
            {

                ext = "." + nameArr[nameArr.Length - 1];
                name = name.Remove(name.LastIndexOf("."));
            }

            var newName = string.Format(@"{0}\{1} copy{2}", parentPath, name, ext);

            if (!File.Exists(newName))
            {
                file.CopyTo(newName);
            }
            else
            {
                for (int i = 1; i < 100; i++)
                {
                    newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, i, ext);
                    if (!File.Exists(newName))
                    {
                        file.CopyTo(newName);
                        break;
                    }
                }
            }

            return newName;
        }
    }
}
