using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command DLE
    /// </summary>
    public class DLECommandHandler : BaseCommandHandler
    {
        public DLECommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            var baseDLEValue = (byte)0x70;
            baseDLEValue += fiscalPrinterState.IsInOnlineMode ? (byte)0x04 : (byte)0x00;
            baseDLEValue += fiscalPrinterState.IsInOutOfPaperState ? (byte)0x02 : (byte)0x00;
            baseDLEValue += fiscalPrinterState.IsInInternalErrorState ? (byte)0x01 : (byte)0x00;

            var response = new OneByteFiscalPrinterCommand((byte)(baseDLEValue & 0xFF));
            return new CommandHandlerResponse(response);
        }
    }
}
