using System;
using System.ServiceProcess;

namespace FiscalPrinterSimulatorService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new FiscalPrinterSimulatorService();
            if (Environment.UserInteractive)
            {
                service.RunServiceAsConsoleApp(args);
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
