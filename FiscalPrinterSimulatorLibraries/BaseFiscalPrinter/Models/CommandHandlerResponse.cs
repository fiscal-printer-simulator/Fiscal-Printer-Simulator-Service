using FiscalPrinterSimulatorLibraries.Commands;

namespace FiscalPrinterSimulatorLibraries.Models
{
    public class CommandHandlerResponse
    {
        public CommandHandlerResponse() { }
        public CommandHandlerResponse(BaseFiscalPrinterCommand command) => OutputCommand = command;
        public CommandHandlerResponse(string receiptBuffer) => OutputReciptBuffer = receiptBuffer;
        public CommandHandlerResponse(BaseFiscalPrinterCommand command, string receiptBuffer)
        {
            OutputCommand = command;
            OutputReciptBuffer = receiptBuffer;
        }

        public BaseFiscalPrinterCommand OutputCommand { get; set; }
        public string OutputReciptBuffer { get; set; } = string.Empty;
    }
}
