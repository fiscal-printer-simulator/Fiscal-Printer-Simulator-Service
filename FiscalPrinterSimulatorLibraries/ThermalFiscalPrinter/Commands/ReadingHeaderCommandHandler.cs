using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBGETHDR
    /// </summary>
    public class ReadingHeaderCommandHandler : BaseCommandHandler
    {
        public ReadingHeaderCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

            var responseCommand = new ThermalFiscalPrinterCommand(new string[] { "1" }, "#U", state.FiscalPrinterHeader);
            return new CommandHandlerResponse(responseCommand);
        }
    }
}
