using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace FiscalPrinterSimulatorService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _serviceProcessInstaller;
        private readonly ServiceInstaller _serviceInstaller;


        public ProjectInstaller()
        {
            this._serviceProcessInstaller = new ServiceProcessInstaller(){Account = ServiceAccount.LocalSystem};

            this._serviceInstaller = new ServiceInstaller
            {
                ServiceName = "Fiscal Printer Simulator",
                Description = "Fiscal Printer Websocket Based Simulator. " +
                "Helps to develop with POS likely applications.",
                StartType = System.ServiceProcess.ServiceStartMode.Automatic,
                DelayedAutoStart = true
            };
            this.Installers.AddRange(new Installer[] {this._serviceProcessInstaller,this._serviceInstaller});
            this.AfterInstall += new InstallEventHandler(this.ProjectInstaller_AfterInstall);
        }


        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(_serviceInstaller.ServiceName))
            {
                sc.Start();
            }
            var resetAfter = 60000;
            Process.Start("cmd.exe", $"/c sc failure \"{_serviceInstaller.ServiceName}\" reset= 0 actions= restart/{resetAfter}/restart/{resetAfter}/restart/{resetAfter}");
        }
    }
}
