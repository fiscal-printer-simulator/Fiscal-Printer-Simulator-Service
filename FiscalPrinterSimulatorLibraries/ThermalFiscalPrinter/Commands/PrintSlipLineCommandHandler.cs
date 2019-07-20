using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBTRSLN
    /// </summary>
    public class PrintSlipLineCommandHandler : BaseCommandHandler
    {
        public PrintSlipLineCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {


            throw new System.NotImplementedException();
        }
    }
}
