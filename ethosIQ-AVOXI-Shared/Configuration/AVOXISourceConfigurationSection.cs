using System;
using System.Configuration;

namespace ethosIQ_AVOXI_Shared.Configuration
{
    public class AVOXISourceConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "AVOXISourceConfiguration";
        public const string CollectionName = "AVOXISources";

        [ConfigurationProperty(CollectionName)]
        [ConfigurationCollection(typeof(AVOXISourceCollection), AddItemName = "AVOXISource")]
        public AVOXISourceCollection AVOXISources => (AVOXISourceCollection)base[CollectionName];
    }
}
