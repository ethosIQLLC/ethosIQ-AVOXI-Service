using ethosIQ_Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Configuration;
using ethosIQ_AVOXI_Shared.Configuration;
using ethosIQ_Database;
using System.Threading.Tasks;


namespace ethosIQ_AVOXI_Shared
{
    public class AVOXIController : ethosIQController
    {
        public AVOXIController(string Name, string LogName) : base(Name, LogName)
        {
            this.Name = Name;
            this.LogName = LogName;
            Sources = new List<ethosIQSource>();

        }

        public override void GetSources()
        {
            Thread.Sleep(10000);

            try
            {              
                foreach (AVOXISourceElement element in AVOXISourceConfiguration.GetConfigurationFromFile())
                {
                    Sources.Add(new AVOXISource(element.Name, element.AccessToken));
                }
                Sources[0].AddCollectionDatabase(AVOXICollectionDatabaseConfiguration.GetCollectionDatabase());
                Database db = Sources[0].CollectionDatabase;
            }
            catch (Exception exception)
            {
                LogError("Failed to get configured sources: " + exception.Message + "\n" + exception.StackTrace);
            }
        }

        public override bool Start()
        {
            try
            {
                GetSources();

                if (Sources.Count > 0)
                {
                    Console.WriteLine("Starting " + Sources.Count + " sources...");
                    foreach (ethosIQSource source in Sources)
                    {
                        Task.Run(() => source.Start());
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                LogError("Failed to start AVOXI Service: " + exception.Message);

                return false;
            }          

            
        }

        public override bool Stop()
        {
            foreach (ethosIQSource source in Sources)
            {
                Task.Run(() => source.Stop());
            }

            return true;
        }

        public void Repost(long startDate, long endDate)
        {
            try
            {
                

                
                    GetSources();
                   
                    foreach(ethosIQSource source in Sources)
                    {
                        AVOXISource avoxiSource = (AVOXISource)source;
                        avoxiSource.Repost(startDate, endDate);
                    }
                


            }
            catch (Exception exception)
            {
                LogError("Failed to start AVOXI Service: " + exception.Message);

            }
        }

        public void Repost()
        {
            Console.WriteLine("Enter start time in UNIX time, in seconds: ");
            long startTime = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Enter end time in UNIX time, in seconds: ");
            long endTime = Convert.ToInt64(Console.ReadLine());

            
                GetSources();
               
                foreach (ethosIQSource source in Sources)
                {
                    AVOXISource avoxiSource = (AVOXISource)source;
                    avoxiSource.Repost(startTime, endTime);
                }
            
        }

        public override bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
