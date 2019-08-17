using System;
using System.Linq;
using System.Text;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBLOGOUT
    /// </summary>
    public class CashierLogoutCommandHandler : BaseCommandHandler
    {
        public CashierLogoutCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

            if (state.TimeDiffrenceInMinutes == int.MinValue)
            {
                throw new FP_IllegalOperationException("RTC clock not initialized.");
            }

            if (state.IsInTransactionState)
            {
                throw new FP_IllegalOperationException("Cannot handle this command in transaction state.");
            }


            var parameters = command.Parameters.Split((char)Constants.ASCICodeCR).Where(m => !string.IsNullOrWhiteSpace(m));
            if (parameters.Count() != 2)
            {
                throw new FP_WrongNumberOfArgumentsException("Expected two parameters to this command.");
            }

            var cashierLogin = parameters.ElementAt(0);

            if (cashierLogin.Length > 32)
            {
                throw new FP_BadFormatOfArgumentException("Cashier login must have maximum 32 characters.");
            }

            var printerCode = parameters.ElementAt(1);

            if (printerCode.Length > 8)
            {
                throw new FP_BadFormatOfArgumentException("Printer code must have maximum 8 characters.");
            }

            state.CashierLogin = cashierLogin;
            state.PrinterCode = printerCode;

            StringBuilder reciptBuilder = new StringBuilder();
            reciptBuilder.AppendLine(state.FiscalPrinterHeader);
            var leftLineOfDate = DateTime.Now.AddMinutes(state.TimeDiffrenceInMinutes).ToString("yyyy-MM-dd");
            var printoutNumber = state.NextFiscalPrinterReciptId.ToString();
            reciptBuilder.AppendLine(leftLineOfDate.PadRight(Constants.ReciptWidth - printoutNumber.Length) + printoutNumber);
            reciptBuilder.AppendLine("N I E F I S K A L N Y".PadCenter(Constants.ReciptWidth));
            reciptBuilder.AppendLine("Zakończenie pracy kasjera");
            reciptBuilder.AppendLine("Kasjer".PadRight(Constants.ReciptWidth - cashierLogin.Length) + cashierLogin);
            reciptBuilder.AppendLine("Numer kasy".PadRight(Constants.ReciptWidth - printerCode.Length) + printerCode);
            reciptBuilder.AppendLine();
            reciptBuilder.AppendLine("N I E F I S K A L N Y".PadCenter(Constants.ReciptWidth));
            var rightLineOfTime = DateTime.Now.AddMinutes(state.TimeDiffrenceInMinutes).ToString("HH-mm-ss");
            reciptBuilder.AppendLine($"    #{printerCode}     {cashierLogin}".PadRight(Constants.ReciptWidth - rightLineOfTime.Length) + rightLineOfTime);
            reciptBuilder.AppendLine("12345678".PadCenter(Constants.ReciptWidth));
            reciptBuilder.AppendLine();

            state.NextFiscalPrinterReciptId += 1;

            return new CommandHandlerResponse(reciptBuilder.ToString());
        }
    }
}
