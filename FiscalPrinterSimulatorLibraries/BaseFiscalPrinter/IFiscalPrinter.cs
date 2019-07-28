using FiscalPrinterSimulatorLibraries.Models;
using System.Collections.Generic;

namespace FiscalPrinterSimulatorLibraries
{
    public interface IFiscalPrinter
    {
        IEnumerable<CommandHandlerResponse> HandleReceivedData(string command);
    }
}
