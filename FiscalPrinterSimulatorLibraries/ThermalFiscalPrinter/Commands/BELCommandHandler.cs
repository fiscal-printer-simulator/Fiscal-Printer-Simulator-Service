using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command BEL
    /// </summary>
    public class BELCommandHandler : BaseCommandHandler
    {
        public BELCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;
            // System.Media.SystemSounds.Beep.Play();
            if (fiscalPrinterState != null)
            {
                state.LastCommandSuccess = true;
            }
            return new CommandHandlerResponse();
        }
    }
}
