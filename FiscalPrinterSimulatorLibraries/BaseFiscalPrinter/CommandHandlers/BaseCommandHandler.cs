using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    public abstract class BaseCommandHandler 
    {
        protected BaseFiscalPrinterCommand command;

        public BaseCommandHandler(BaseFiscalPrinterCommand command)
        {
            this.command = command;
        }

        public abstract CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState);
    }
}
