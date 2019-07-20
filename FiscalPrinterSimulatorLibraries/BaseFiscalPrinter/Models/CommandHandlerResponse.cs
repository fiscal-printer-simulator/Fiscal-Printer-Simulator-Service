using FiscalPrinterSimulatorLibraries.Commands;

namespace FiscalPrinterSimulatorLibraries.Models
{
    public class CommandHandlerResponse
    {
        public CommandHandlerResponse() { }
        public CommandHandlerResponse(FiscalPrinterCommand command) => OutputCommand = command;
        public CommandHandlerResponse(string receiptBuffer) => OutputReciptBuffer = receiptBuffer;
        public CommandHandlerResponse(FiscalPrinterCommand command, string receiptBuffer)
        {
            OutputCommand = command;
            OutputReciptBuffer = receiptBuffer;
        }

        public FiscalPrinterCommand OutputCommand { get; set; }
        public string OutputReciptBuffer { get; set; } = string.Empty;
    }
}
