using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBIDRQ
    /// </summary>
    public class SendTypeAndSoftwareVersionCommandHandler : BaseCommandHandler
    {
        public SendTypeAndSoftwareVersionCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState) =>
            new CommandHandlerResponse(
                new ThermalFiscalPrinterCommand(new string[] { "1" }, "#v", $"{Constants.ProtocolName}/{Constants.ProtocolVersion}")
                );
    }
}
