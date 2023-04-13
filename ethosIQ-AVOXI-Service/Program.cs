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
            if (Environment.UserInteractive)
            {
                serviceHost = new ServiceHost(service);
                serviceHost.Open();
                service.OnStart(args);
                Console.ReadLine();
            }
            else
            {
                serviceHost = new ServiceHost(service);
                serviceHost.Open();
                Run(service);
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
