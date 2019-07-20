using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBIDRQ
    /// </summary>
    public class SendTypeAndSoftwareVersionCommandHandler : BaseCommandHandler
    {
        public SendTypeAndSoftwareVersionCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState) =>
            new CommandHandlerResponse(
                new FiscalPrinterCommand(new string[] { "1" }, "#v", "POSNET Thermal/4.01")
                );
    }
}
