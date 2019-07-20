using System.Threading.Tasks;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command CAN
    /// </summary>
    public class CANCommandHandler : BaseCommandHandler
    {
        public CANCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            throw new TaskCanceledException("CAN command found.Aborting the service process.");
        }
    }
}
