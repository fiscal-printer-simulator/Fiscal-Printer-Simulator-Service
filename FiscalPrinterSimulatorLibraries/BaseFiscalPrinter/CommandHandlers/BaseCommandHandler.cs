using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    public abstract class BaseCommandHandler 
    {
        protected FiscalPrinterCommand command;

        public BaseCommandHandler(FiscalPrinterCommand command)
        {
            this.command = command;
        }

        public abstract CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState);
    }
}
