using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command BEL
    /// </summary>
    public class BELCommandHandler : BaseCommandHandler
    {
        public BELCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            // System.Media.SystemSounds.Beep.Play();
            if (fiscalPrinterState != null)
            {
                fiscalPrinterState.LastCommandSuccess = true;
            }
            return new CommandHandlerResponse();
        }
    }
}
