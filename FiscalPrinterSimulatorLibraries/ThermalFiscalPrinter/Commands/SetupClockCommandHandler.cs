using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;
using System;
using System.Linq;
using System.Text;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSETCK
    /// </summary>
    public class SetupClockCommandHandler : BaseCommandHandler
    {
        public SetupClockCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {

            bool containsNotSupportedLength = command.PnArguments.Any(m => m.Length < 1 || m.Length > 2);
            bool containsNoRightParameterLength = command.PnArguments.Count() != 6;
            int[] arguments;

            if (containsNotSupportedLength)
            {
                throw new FP_BadFormatOfArgumentException("Arguments must contains one or two digits.");
            }

            if (containsNoRightParameterLength)
            {
                throw new FP_WrongNumberOfArgumentsException("Required 6 parameters.");
            }

            try
            {
                arguments = command.PnArguments.Select(m => int.Parse(m)).ToArray();
            }
            catch
            {
                throw new FP_BadFormatOfArgumentException("Argument must contain numeric values.");
            }

            var actualDate = fiscalPrinterState.TimeDiffrenceInMinutes == int.MinValue ? DateTime.Now : DateTime.Now.AddMinutes(fiscalPrinterState.TimeDiffrenceInMinutes);
            var passedDate = new DateTime(2000 + arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
            var minutesDiffrence = Convert.ToInt32(Math.Round(passedDate.Subtract(actualDate).TotalMinutes));

            if ((minutesDiffrence > 60 || minutesDiffrence < -60) && fiscalPrinterState.IsInFiscalState)
            {
                throw new FP_IllegalOperationException("In Fiscal State change Date and Time is possible only.");
            }

            fiscalPrinterState.TimeDiffrenceInMinutes = minutesDiffrence;

            StringBuilder reciptBody = new StringBuilder();
            string actualDateFormatted = actualDate.ToString("yyyy-MM-dd,HH:mm");
            string passedDateFormatted = passedDate.ToString("yyyy-MM-dd,HH:mm");
            reciptBody.AppendLine();
            reciptBody.AppendLine("PROGRAMOWANIE ZEGARA".PadCenter(Constants.ReciptWidth));
            reciptBody.AppendLine();
            reciptBody.AppendLine($"Zegar przed zmianą:".PadRight(Constants.ReciptWidth - actualDateFormatted.Length) + actualDateFormatted);
            reciptBody.AppendLine($"Zegar po zmianie:".PadRight(Constants.ReciptWidth - passedDateFormatted.Length) + passedDateFormatted);
            reciptBody.AppendLine();

            return new CommandHandlerResponse(reciptBody.ToString());
        }

    }
}
