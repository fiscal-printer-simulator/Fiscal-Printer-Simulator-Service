using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command DLE
    /// </summary>
    public class DLECommandHandler : BaseCommandHandler
    {
        public DLECommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;
            var baseDLEValue = (byte)0x70;
            baseDLEValue += state.IsInOnlineMode ? (byte)0x04 : (byte)0x00;
            baseDLEValue += state.IsInOutOfPaperState ? (byte)0x02 : (byte)0x00;
            baseDLEValue += state.IsInInternalErrorState ? (byte)0x01 : (byte)0x00;

            var response = new OneByteFiscalPrinterCommand((byte)(baseDLEValue & 0xFF));
            return new CommandHandlerResponse(response);
        }
    }
}
