using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Common
{
    class QueryEx : ConfigurationElement
    {
        [ConfigurationProperty("Query", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["Query"]; }
        }
    }

    class FeaturesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new QueryEx();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueryEx)element).Name;
        }
    }

    class FeatureList : ConfigurationSection
    {
        [ConfigurationProperty("Feature")]
        public FeaturesCollection Features
        {
            get { return (FeaturesCollection)this["Feature"]; }
        }
    }
}
