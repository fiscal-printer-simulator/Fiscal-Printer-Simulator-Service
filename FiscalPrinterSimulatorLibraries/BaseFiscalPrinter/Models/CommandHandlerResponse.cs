using FiscalPrinterSimulatorLibraries.Commands;

namespace FiscalPrinterSimulatorLibraries.Models
{
    public class CommandHandlerResponse
    {
        public CommandHandlerResponse() { }
        public CommandHandlerResponse(BaseFiscalPrinterCommand command) => OutputCommand = command;
        public CommandHandlerResponse(string receiptBuffer) => OutputReceiptBuffer = receiptBuffer;
        public CommandHandlerResponse(ClientLineDisplayOutput clientLineDisplayOutputLine) => 
            ClientLineDisplayOutputLine = clientLineDisplayOutputLine;
        public CommandHandlerResponse(BaseFiscalPrinterCommand command, string receiptBuffer)
        {
            OutputCommand = command;
            OutputReceiptBuffer = receiptBuffer;
        }

        public BaseFiscalPrinterCommand OutputCommand { get; set; }
        public string OutputReceiptBuffer { get; set; } = string.Empty;
        public ClientLineDisplayOutput ClientLineDisplayOutputLine { get; set; }
    }
}
