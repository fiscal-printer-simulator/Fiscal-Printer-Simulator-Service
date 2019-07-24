using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    ///  Command handler for command LBTREXITCAN
    /// </summary>
    public class CancelTransactionCommandHandler : BaseCommandHandler
    {
        public CancelTransactionCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            fiscalPrinterState.IsInTransactionState = false;

            if (!fiscalPrinterState.SlipLines.Any())
            {
                return new CommandHandlerResponse();
            }


            StringBuilder cancelReceiptBuilder = new StringBuilder();
            cancelReceiptBuilder.AppendLine("A N U L O W A N Y".PadCenter(Constants.ReciptWidth));
            var commandParameters =command.Parameters.Split((char)Constants.ASCICodeCR).Where(m=> !string.IsNullOrWhiteSpace(m)).ToArray();

            var printerNo = "";
            var cashierLogin = "";
            var actualTime = new DateTime().AddMinutes(fiscalPrinterState.TimeDiffrenceInMinutes).ToString("HH:mm");
            var specialNumberRow = "";

            if(commandParameters.Length == 1)
            {
                specialNumberRow = commandParameters[0].PadCenter(Constants.ReciptWidth);

            }
            else if(commandParameters.Length > 0)
            {
                printerNo = commandParameters[0];
                cashierLogin = commandParameters.Length > 1 ? commandParameters[1] : "";
                specialNumberRow = commandParameters.Length == 3 ? commandParameters[2].PadCenter(Constants.ReciptWidth) : "";

                cancelReceiptBuilder.AppendLine($"   #{printerNo}     {cashierLogin}".PadRight(Constants.ReciptWidth - actualTime.Length) + actualTime);
            }
            if (!string.IsNullOrWhiteSpace(specialNumberRow))
            {
                cancelReceiptBuilder.AppendLine(specialNumberRow);
            }

            return new CommandHandlerResponse(cancelReceiptBuilder.ToString());

            
        }
    }
}
