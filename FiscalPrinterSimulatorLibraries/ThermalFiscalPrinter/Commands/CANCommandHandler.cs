using System.Threading.Tasks;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command CAN
    /// </summary>
    public class CANCommandHandler : BaseCommandHandler
    {
        public CANCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            throw new TaskCanceledException("CAN command found.Aborting the service process.");
        }
    }
}
