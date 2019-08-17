using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FiscalPrinterSimulatorService
{
    static class Program
    {
        static void Main()
        {
            var args = new string[0];
            var service = new FiscalPrinterSimulatorService();
            if (Environment.UserInteractive)
            {
                service.RunServiceAsConsoleApp(args);
            }
            else
            {
                ServiceBase.Run(service);
            }
           // ServiceBase.Run(service);
        }
    }
}
