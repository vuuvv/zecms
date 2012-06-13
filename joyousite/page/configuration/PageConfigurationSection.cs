using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace vuuvv.page.configuration
{
    class PageConfigurationSection : ConfigurationSection
    {
        public PageConfigurationSection()
            : base()
        {
        }

        [ConfigurationProperty("Rules")]
        public RuleElementsCollection rules
        {
            get
            {
                return (RuleElementsCollection)this["Rule"];
            }
            set
            {
                this["Rule"] = value;
            }
        }
    }

    class RuleElement : ConfigurationElement
    {
        public RuleElement()
            : base()
        {
        }

        [ConfigurationProperty("Pattern")]
        public string Pattern
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
        public string Handler
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
    }

    class RuleElementsCollection : ConfigurationElementCollection
    {
        public RuleElementsCollection()
        {
            RuleElement rule = (RuleElement)CreateNewElement();
            BaseAdd(rule);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).Pattern;
        }

        new public RuleElement this[string Name]
        {
            get
            {
                return (RuleElement)BaseGet(Name);
            }
        }

        public int IndexOf(RuleElement rule)
        {
            return BaseIndexOf(rule);
        }
    }
}
