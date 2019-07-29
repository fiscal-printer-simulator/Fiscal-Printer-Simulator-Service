using FiscalPrinterSimulatorLibraries.Models;
using System.Collections.Generic;

namespace FiscalPrinterSimulatorLibraries
{
    public interface IFiscalPrinter
    {
        string ProtocolName { get; }
        IEnumerable<CommandHandlerResponse> HandleReceivedData(string command);
    }
}
