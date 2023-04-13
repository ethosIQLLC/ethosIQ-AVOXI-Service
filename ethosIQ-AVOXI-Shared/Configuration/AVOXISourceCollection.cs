using System;

using System.Configuration;

namespace ethosIQ_AVOXI_Shared.Configuration
{
    public class AVOXISourceCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new AVOXISourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AVOXISourceElement)element).Name;
        }
    }
}
