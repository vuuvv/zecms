using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ElFinder.Connector.Configuration
{
    public sealed class ElFinderSection :
    ConfigurationSection
    {
        public ElFinderSection()
            : base()
        {

        }

        [ConfigurationProperty("dotFiles")]
        public string DotFiles
        {
            get
            {
                return this["dotFiles"].ToString();
            }
            set
            {
                this["dotFiles"] = value;
            }
        }

        [ConfigurationProperty("uplMaxSize")]
        public int UplMaxSize
        {
            get
            {
                return (int)this["uplMaxSize"];
            }
            set
            {
                this["uplMaxSize"] = value;
            }
        }

        [ConfigurationProperty("Root")]
        public RootElement Root
        {
            get
            {
                return (RootElement)this["Root"];
            }
            set
            {
                this["Root"] = value;
            }
        }

        [ConfigurationProperty("DisabledCommands")]
        public NamedElementsCollection DisabledCommands
        {
            get
            {
                return (NamedElementsCollection)this["DisabledCommands"];
            }
            set
            {
                this["DisabledCommands"] = value;
            }
        }

        [ConfigurationProperty("DisabledMimeTypes")]
        public NamedElementsCollection DisabledMimeTypes
        {
            get
            {
                return (NamedElementsCollection)this["DisabledMimeTypes"];
            }
            set
            {
                this["DisabledMimeTypes"] = value;
            }
        }
    }

    public sealed class RootElement : ConfigurationElement
    {
        public RootElement()
            : base() 
        {
        }

        [ConfigurationProperty("Path")]
        public string Path
        {
            get
            {
                return this["Path"].ToString();
            }
            set
            {
                this["Path"] = value;
            }
        }

        [ConfigurationProperty("Url")]
        public string Url
        {
            get
            {
                return this["Url"].ToString();
            }
            set
            {
                this["Url"] = value;
            }
        }
    }

    public class NamedElement : ConfigurationElement
    {
        public NamedElement()
            : base()
        {
            
        }

        [ConfigurationProperty("Name",
            IsRequired = true,
            IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }
            set
            {
                this["Name"] = value;
            }
        }        
    }



    public class NamedElementsCollection :
        ConfigurationElementCollection
    {
        public NamedElementsCollection()
        {
            NamedElement command =
                (NamedElement)CreateNewElement();
            BaseAdd(command);
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new NamedElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((NamedElement)element).Name;
        }

        new public NamedElement this[string Name]
        {
            get
            {
                return (NamedElement)BaseGet(Name);
            }
        }

        public int IndexOf(NamedElement command)
        {
            return BaseIndexOf(command);
        }        
    }
}
