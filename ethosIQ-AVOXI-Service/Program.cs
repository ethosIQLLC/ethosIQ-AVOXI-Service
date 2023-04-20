using System;
using System.ServiceModel;
using System.ServiceProcess;
using ethosIQ_AVOXI_Shared;

namespace ethosIQ_AVOXI_Service
{
    class Program : ServiceBase
    {
        public AVOXIController controller;
        static void Main(string[] args)
        {
            ServiceHost serviceHost;
            Program service = new Program();

            if(args.Length == 0)
            {
                if (Environment.UserInteractive)
                {
                    serviceHost = new ServiceHost(service);
                    serviceHost.Open();

                    Console.WriteLine("0 for Service, 1 for Repost");

                    int selection = 0;

                    try
                    {
                        selection = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    if (selection == 0)
                    {
                        serviceHost = new ServiceHost(service);
                        serviceHost.Open();
                        service.OnStart(args);
                    }
                    else if (selection == 1)
                    {
                        AVOXIController controller;
                        controller = new AVOXIController("AVOXI Controller", "AVOXI Service");
                        controller.Repost();
                    }

                    Console.ReadLine();
                }
                else
                {
                    serviceHost = new ServiceHost(service);
                    serviceHost.Open();
                    Run(service);
                }
            }
            else
            {
                long StartTime = Convert.ToInt64(args[0]);
                long EndTime = Convert.ToInt64(args[1]);
                Console.WriteLine("StartTime: " + StartTime);
                Console.WriteLine("EndTime: " + EndTime);

                AVOXIController controller;
                controller = new AVOXIController("AVOXI Controller", "AVOXI Service");
                controller.Repost(StartTime, EndTime);
            }           
            
        }

        protected override void OnStart(string[] args)
        {
            controller = new AVOXIController("AVOXI Controller", "AVOXI Service");
            controller.Start();
        }

        

        protected override void OnStop()
        {
            controller.Stop();
        }
    }
}
