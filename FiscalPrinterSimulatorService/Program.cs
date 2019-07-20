using System;
using System.ServiceProcess;

namespace FiscalPrinterSimulatorService
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
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
