using FiscalPrinterSimulatorLibraries.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalPrinterSimulatorLibraries
{
    public interface IFiscalPrinter
    {
        IEnumerable<CommandHandlerResponse> HandleReceivedData(string command);
    }
}
