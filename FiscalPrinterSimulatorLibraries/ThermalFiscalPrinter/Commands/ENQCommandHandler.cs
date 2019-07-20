using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command ENQ
    /// </summary>
    public class ENQCommandHandler : BaseCommandHandler
    {
        public ENQCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            var enqResponse = (byte)0x60;
            enqResponse += fiscalPrinterState.IsInFiscalState ? (byte)0x08 : (byte)0x00;
            enqResponse += fiscalPrinterState.LastCommandSuccess ? (byte)0x04 : (byte)0x00;
            enqResponse += fiscalPrinterState.IsInTransactionState ? (byte)0x02 : (byte)0x00;
            enqResponse += fiscalPrinterState.LastTransactionSuccess ? (byte)0x01 : (byte)0x00;

            var response = new OneByteFiscalPrinterCommand((byte)(enqResponse & 0xFF));
            return new CommandHandlerResponse(response);
        }
    }
}
