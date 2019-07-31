using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    //TODO: Handling for "Pc" argument - resolve it globaly.

    /// <summary>
    ///  Command handler for command LBTREXITCAN && LBTREXIT
    /// </summary>
    public class CancelOrApproveTransactionCommandHandler : BaseCommandHandler
    {
        public CancelOrApproveTransactionCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            if (!fiscalPrinterState.IsInTransactionState)
            {
                throw new FP_IllegalOperationException("Fiscal Printer is not in transaction state.");
            }

            if (!command.PnArguments.Any())
            {
                throw new FP_WrongNumberOfArgumentsException("This command required at least one argument.");
            }
            else if (command.PnArguments.ElementAt(0) == "0")
            {
                return CancelTransactionHandle(fiscalPrinterState);
            }

            int percentOfTotalDiscount = 0;
            int ammountOfAdditionalLines = 0;
            double passedTotalAmmount = 0;
            double passedPaidValue = 0;
            double passedDiscountValue = 0;
            DiscountType discountType = DiscountType.NO_DISCOUNT;


            if (command.PnArguments.Count() < 2 || command.PnArguments.Count() == 3 || command.PnArguments.Count() == 5 || command.PnArguments.Count() > 6)
            {
                throw new FP_WrongNumberOfArgumentsException("Transaction approval requires at least 2 arguments.");

            }
            else
            {
                if (!int.TryParse(command.PnArguments.ElementAt(1), out percentOfTotalDiscount))
                {
                    throw new FP_BadFormatOfArgumentException("Percent of total discount must be a number between 0 and 99");
                }

                if (command.PnArguments.Count() >= 4)
                {

                    if (!int.TryParse(command.PnArguments.ElementAt(2), out ammountOfAdditionalLines)
                        || ammountOfAdditionalLines < 0 || ammountOfAdditionalLines > 3)
                    {
                        throw new FP_BadFormatOfArgumentException("Number of additional lines must be a number between 0 and 3.");
                    }

                    if (command.PnArguments.Count() == 6)
                    {
                        if (!Enum.TryParse<DiscountType>(command.PnArguments.ElementAt(4), out discountType))
                        {
                            throw new FP_BadFormatOfArgumentException("Invaild type of discount declared.");
                        }
                    }

                }

                var parametersMatch = new Regex(@"(\w{3})\r((.*\r){0,3})(([\d\.\,]+)\/)(([\d\.\,]+)\/)(([\d\.\,]+)\/)*").Match(command.Parameters);
                if (!parametersMatch.Success)
                {
                    throw new FP_WrongNumberOfArgumentsException("Command code is not ok. Please check it out and try again.");
                }
                if (!parametersMatch.Groups[1].Success)
                {
                    throw new FP_BadFormatOfArgumentException("Parameter \"code\" is in wrong formatting. It must have 3 characters.");
                }
                else if (string.IsNullOrWhiteSpace(fiscalPrinterState.CashierLogin) || string.IsNullOrWhiteSpace(fiscalPrinterState.PrinterCode))
                {
                    var passedParameterCode = parametersMatch.Groups[1].Value;
                    fiscalPrinterState.PrinterCode = passedParameterCode.Substring(0, 1);
                    fiscalPrinterState.CashierLogin = passedParameterCode.Substring(1);
                }

                var additionalLines = !parametersMatch.Groups[2].Success ?
                                        new string[0]
                                        : parametersMatch.Groups[2].Value
                                        .Split((char)Constants.ASCICodeCR)
                                        .Where(m=> !string.IsNullOrWhiteSpace(m))
                                        .ToArray();
                if (ammountOfAdditionalLines != additionalLines.Length)
                {
                    throw new FP_IllegalOperationException("Passed additional lines argument and passed lines are not equal.");
                }

                if (!double.TryParse(parametersMatch.Groups[5].Value, out passedPaidValue))
                {
                    throw new FP_BadFormatOfArgumentException("Bad formatting of PAID value.");
                }

                if (!double.TryParse(parametersMatch.Groups[7].Value, out passedTotalAmmount))
                {
                    throw new FP_BadFormatOfArgumentException("Bad formatting of TOTAL value.");
                }
                else if (passedTotalAmmount != fiscalPrinterState.SlipLines.Sum(m => m.TotalWithDiscount))
                {
                    throw new FP_IllegalOperationException("Wrong passed TOTAL value. It not equals with passed fiscal printer line.");
                    // TODO: Discount Handling
                }

                if (command.PnArguments.Count() == 6
                    && discountType != DiscountType.NO_DISCOUNT
                    && !double.TryParse(parametersMatch.Groups[9].Value, out passedDiscountValue))
                {
                    throw new FP_BadFormatOfArgumentException("Bad formatting of DISCOUNT value.");
                }


                StringBuilder approveTransactionBuilder = new StringBuilder();
                approveTransactionBuilder.AppendLine("".PadRight(Constants.ReciptWidth, '-'));
                if (discountType != DiscountType.NO_DISCOUNT)
                {
                    //TODO: discount handling
                }

                var ptuActualValues = fiscalPrinterState.PTURates
                    .Where(m => m.ActualPercentageValue < 100)
                    .ToDictionary(m => m.Type, m => m.ActualPercentageValue);

                var PTUsOverview = fiscalPrinterState.SlipLines
                    .GroupBy(m => m.PTU)
                    .Select(m => new
                    {
                        type = m.Key,
                        sum = m.Sum(slip => slip.TotalWithDiscount),
                        ptuPercentage = ptuActualValues[m.Key],
                        ptuVal = 
                        (m.Sum(slip => slip.TotalWithDiscount) * (ptuActualValues[m.Key] / 100))
                        /
                        (1 + (ptuActualValues[m.Key] / 100))
                    });

                foreach (var ptuOVerview in PTUsOverview)
                {
                    var totalInPTU = ptuOVerview.sum.ToString("0.00");
                    var totalInPTULeftLine = $"Sprzed. opodatk. {ptuOVerview.type.ToString()}"
                        .PadRight(Constants.ReciptWidth - totalInPTU.Length);
                    var totalPTUVal = ptuOVerview.ptuVal.ToString("0.00");
                    var totalPTUValLeftLine = $"Kwota PTU {ptuOVerview.type.ToString()} {ptuOVerview.ptuPercentage} %"
                        .PadRight(Constants.ReciptWidth - totalPTUVal.Length);


                    approveTransactionBuilder.AppendLine(totalInPTULeftLine + totalInPTU);
                    approveTransactionBuilder.AppendLine(totalPTUValLeftLine + totalPTUVal);
                }
                var totalPTUsValue = PTUsOverview.Sum(m => m.ptuVal).ToString("0.00");
                approveTransactionBuilder.AppendLine("ŁĄCZNA KWOTA PTU".PadRight(Constants.ReciptWidth - totalPTUsValue.Length) + totalPTUsValue);

                var totalSum = passedTotalAmmount.ToString("0.00");
                approveTransactionBuilder.AppendLine("S U M A".PadRight(Constants.ReciptWidth - totalSum.Length) + totalSum);

                approveTransactionBuilder.AppendLine("".PadRight(Constants.ReciptWidth, '-'));

                if (passedPaidValue > passedTotalAmmount)
                {

                    var paidValue = passedPaidValue.ToString("0.00");
                    approveTransactionBuilder.AppendLine("Gotówka".PadRight(Constants.ReciptWidth - totalSum.Length) + totalSum);

                    var changeValue = (passedPaidValue - passedTotalAmmount).ToString("0.00");
                    approveTransactionBuilder.AppendLine("Reszta".PadRight(Constants.ReciptWidth - changeValue.Length) + changeValue);
                }

                var printId = new Random().Next(0,9999).ToString("0000");
                var footerLeftLine = $"{printId} #Kasa: {fiscalPrinterState.PrinterCode}  Kasjer: {fiscalPrinterState.CashierLogin}";
                var transactionTime = DateTime.Now.AddMinutes(fiscalPrinterState.TimeDiffrenceInMinutes).ToString("HH:mm");
                approveTransactionBuilder.AppendLine(footerLeftLine.PadRight(Constants.ReciptWidth - transactionTime.Length) + transactionTime);

                var fiscalIdAndLogo = "{PL} ABC "+new Random().Next(10000,99999);
                approveTransactionBuilder.AppendLine(fiscalIdAndLogo.PadCenter(Constants.ReciptWidth));
                approveTransactionBuilder.AppendLine();

                foreach (var additionalLine in additionalLines)
                {
                    approveTransactionBuilder.AppendLine(additionalLine.PadCenter(Constants.ReciptWidth));
                }

                fiscalPrinterState.IsInTransactionState = false;

                return new CommandHandlerResponse(approveTransactionBuilder.ToString());

            }
        }

        private CommandHandlerResponse CancelTransactionHandle(FiscalPrinterState fiscalPrinterState)
        {
            fiscalPrinterState.IsInTransactionState = false;

            if (!fiscalPrinterState.SlipLines.Any())
            {
                return new CommandHandlerResponse();
            }


            StringBuilder cancelReceiptBuilder = new StringBuilder();
            cancelReceiptBuilder.AppendLine("A N U L O W A N Y".PadCenter(Constants.ReciptWidth));
            var commandParameters = command.Parameters.Split((char)Constants.ASCICodeCR).Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();

            var printerNo = "";
            var cashierLogin = "";
            var actualTime = new DateTime().AddMinutes(fiscalPrinterState.TimeDiffrenceInMinutes).ToString("HH:mm");
            var specialNumberRow = "";
            if (commandParameters.Length == 1)
            {
                specialNumberRow = commandParameters[0].PadCenter(Constants.ReciptWidth);

            }
            else if (commandParameters.Length > 0)
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
