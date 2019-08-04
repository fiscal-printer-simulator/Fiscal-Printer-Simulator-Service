using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command ENQ
    /// </summary>
    public class ENQCommandHandler : BaseCommandHandler
    {
        public ENQCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;
            var enqResponse = (byte)0x60;
            enqResponse += state.IsInFiscalState ? (byte)0x08 : (byte)0x00;
            enqResponse += state.LastCommandSuccess ? (byte)0x04 : (byte)0x00;
            enqResponse += state.IsInTransactionState ? (byte)0x02 : (byte)0x00;
            enqResponse += state.LastTransactionSuccess ? (byte)0x01 : (byte)0x00;

            var response = new OneByteFiscalPrinterCommand((byte)(enqResponse & 0xFF));
            return new CommandHandlerResponse(response);
        }
    }
}
