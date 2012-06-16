using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace vuuvv.page.configuration
{
    public class PageSection : ConfigurationSection
    {
        public PageSection()
            : base()
        {
        }

        [ConfigurationProperty("Routes")]
        public RouteElementsCollection routes
        {
            get
            {
                return (RouteElementsCollection)this["Routes"];
            }
            set
            {
                this["Routes"] = value;
            }
        }

        [ConfigurationProperty("Template")]
        public string template
        {
            get
            {
                return (string)this["Template"];
            }
            set
            {
                this["Template"] = value;
            }
        }
    }

    public class RouteElement : ConfigurationElement
    {
        public RouteElement()
            : base()
        {
        }

        [ConfigurationProperty("Pattern")]
        public string pattern
        {
            get
            {
                return (string)this["Pattern"];
            }
            set
            {
                this["Pattern"] = value;
            }
        }

        [ConfigurationProperty("Handler")]
        public string handler
        {
            get
            {
                return (string)this["Handler"];
            }
            set
            {
                this["Handler"] = value;
            }
        }

        // if true, not get data from database
        [ConfigurationProperty("Ignore")]
        public bool ignore
        {
            get
            {
                return (string)this["Ignore"] != "false";
            }
            set
            {
                this["Ignore"] = value.ToString();
            }
        }
    }

    public class RouteElementsCollection : ConfigurationElementCollection
    {
        public RouteElementsCollection()
            :base()
        {
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RouteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RouteElement)element).pattern;
        }

        new public RouteElement this[string Name]
        {
            get
            {
                return (RouteElement)BaseGet(Name);
            }
        }

        public RouteElement this[int index]
        {
            get
            {
                return (RouteElement)BaseGet(index);
            }
        }

        public int IndexOf(RouteElement rule)
        {
            return BaseIndexOf(rule);
        }
    }
}
