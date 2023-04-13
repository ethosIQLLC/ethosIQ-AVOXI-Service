using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace ethosIQ_AVOXI_Service
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private readonly ServiceProcessInstaller process;
        private readonly ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            service = new ServiceInstaller
            {
                ServiceName = "ethosIQ-AVOXI-Service"
            };
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
