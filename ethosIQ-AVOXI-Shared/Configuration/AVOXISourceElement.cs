using System;

using System.Configuration;

namespace ethosIQ_AVOXI_Shared.Configuration
{
    public class AVOXISourceElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }
        [ConfigurationProperty("AccessToken", IsRequired = true)]
        public string AccessToken
        {
            get { return (string)this["AccessToken"]; }
            set { this["AccessToken"] = value; }
        }
    }
}
