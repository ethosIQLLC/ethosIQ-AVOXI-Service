using System;
using System.Collections.Generic;
using System.Configuration;

namespace ethosIQ_AVOXI_Shared.Configuration
{
    public class AVOXISourceConfiguration
    {
        private static List<AVOXISourceElement> AVOXISources = new List<AVOXISourceElement>();

        public static List<AVOXISourceElement> GetConfigurationFromFile()
        {
            AVOXISourceConfigurationSection CustomSection = (AVOXISourceConfigurationSection)ConfigurationManager.GetSection(AVOXISourceConfigurationSection.SectionName);
            if(CustomSection != null)
            {
                foreach(AVOXISourceElement AVOXISource in CustomSection.AVOXISources)
                {
                    AVOXISources.Add(AVOXISource);
                }
            }

            return AVOXISources;
        }
    }
}
