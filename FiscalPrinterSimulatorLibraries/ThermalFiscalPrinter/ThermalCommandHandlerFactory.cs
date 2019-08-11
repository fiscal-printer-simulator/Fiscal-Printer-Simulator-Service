using System;
using FiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Commands;

namespace ThermalFiscalPrinterSimulatorLibraries
{
    public class ThermalCommandHandlerFactory
    {
        public static BaseCommandHandler Create(BaseFiscalPrinterCommand command)
        {
            switch (command.Name)
            {
                case "$c":
                    return new SetupClockCommandHandler(command);
                case nameof(Constants.ASCICodeDLE):
                    return new DLECommandHandler(command);
                case nameof(Constants.ASCICodeENQ):
                    return new ENQCommandHandler(command);
                case nameof(Constants.ASCICodeCAN):
                    return new CANCommandHandler(command);
                case nameof(Constants.ASCICodeBEL):
                    return new BELCommandHandler(command);
                case "$p":
                    return new ChangePTURatesCommandHandler(command);
                case "$f":
                    return new SetupReciptHeaderCommandHandler(command);
                case "#u":
                    return new ReadingHeaderCommandHandler(command);
                case "$r":
                    return new SetupPaperKnifeHeightAndClientDisplayCommandHander(command);
                case "#l":
                    return new FeedPapperCommandHandler(command);
                case "#e":
                    return new SetupErrorHandlingCommandHandler(command);
                case "#v":
                    return new SendTypeAndSoftwareVersionCommandHandler(command);

                case "$h":
                    return new StartTransactionCommandHandler(command);
                case "$l":
                    return new PrintSlipLineCommandHandler(command);
                case "$e":
                    return new CancelOrApproveTransactionCommandHandler(command);


                default:
                    throw new NotImplementedException("Command not exists yet.");
            }
        }
    }
}
