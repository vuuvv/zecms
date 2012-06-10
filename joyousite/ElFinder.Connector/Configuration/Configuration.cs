using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ElFinder.Connector.Configuration
{
    public static class Configuration
    {
        public static bool _isLoaded = false;

        public static string RootPath { get; private set; }

        public static string RootUrl { get; private set; }

        public static string DotFiles { get; private set; }

        public static string UplMaxSize { get; private set; }

        public static IEnumerable<string> DisabledCommands { get; private set; }

        public static IEnumerable<string> DisabledMimeTypes { get; private set; }

        public static void Init(System.Web.HttpContext context)
        {
            if (_isLoaded)
                return;

            var section = (ElFinderSection)ConfigurationManager.GetSection("ElFinder");

            RootPath = context.Server.MapPath(section.Root.Path);

            RootUrl = section.Root.Url;

            DotFiles = section.DotFiles;

            UplMaxSize = section.UplMaxSize + "M";

            if (section.DisabledCommands.Count > 0)
            {
                DisabledCommands = section.DisabledCommands.Cast<NamedElement>().Where(x=>x.Name!=string.Empty).Select(x => x.Name);
            }
            else
            {
                DisabledCommands = new List<string>();
            }

            if (section.DisabledMimeTypes.Count > 0)
            {
                DisabledMimeTypes = section.DisabledMimeTypes.Cast<NamedElement>().Where(x => x.Name != string.Empty).Select(x => x.Name);
            }
            else
            {
                DisabledMimeTypes = new List<string>();
            }
        }
    }
}
