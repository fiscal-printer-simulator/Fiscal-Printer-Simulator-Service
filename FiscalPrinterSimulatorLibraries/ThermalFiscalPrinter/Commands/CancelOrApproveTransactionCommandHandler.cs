using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    //TODO: Handling for "Pc" argument - resolve it globaly.

    /// <summary>
    ///  Command handler for command LBTREXITCAN && LBTREXIT
    /// </summary>
    public class CancelOrApproveTransactionCommandHandler : BaseCommandHandler
    {
        public CancelOrApproveTransactionCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;
            if (!state.IsInTransactionState)
            {
                throw new FP_IllegalOperationException("Fiscal Printer is not in transaction state.");
            }

            if (!command.PnArguments.Any())
            {
                throw new FP_WrongNumberOfArgumentsException("This command required at least one argument.");
            }
            else if (command.PnArguments.ElementAt(0) == "0")
            {
                return CancelTransactionHandle(state);
            }

            int optionalTotalDiscountPercentage = 0;
            int ammountOfAdditionalLines = 0;
            double passedTotalAmmount = 0;
            double passedPaidValue = 0;
            double discountValueForTransaction = 0;
            TotalDiscountType discountType = TotalDiscountType.NO_DISCOUNT;
            double totalAmmountWithoutDiscounts = state.SlipLines.Sum(m => m.TotalWithDiscount);

            if (command.PnArguments.Count() < 2 || command.PnArguments.Count() == 3 || command.PnArguments.Count() == 5 || command.PnArguments.Count() > 6)
            {
                throw new FP_WrongNumberOfArgumentsException("Transaction approval requires at least 2 arguments.");

            }
            else
            {
                if (!int.TryParse(command.PnArguments.ElementAt(1), out optionalTotalDiscountPercentage))
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
                        if (!Enum.TryParse<TotalDiscountType>(command.PnArguments.ElementAt(4), out discountType))
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
                else if (string.IsNullOrWhiteSpace(state.CashierLogin) || string.IsNullOrWhiteSpace(state.PrinterCode))
                {
                    var passedParameterCode = parametersMatch.Groups[1].Value;
                    state.PrinterCode = passedParameterCode.Substring(0, 1);
                    state.CashierLogin = passedParameterCode.Substring(1);
                }

                var additionalLines = !parametersMatch.Groups[2].Success ?
                                        new string[0]
                                        : parametersMatch.Groups[2].Value
                                        .Split((char)Constants.ASCICodeCR)
                                        .Where(m => !string.IsNullOrWhiteSpace(m))
                                        .ToArray();
                if (ammountOfAdditionalLines != additionalLines.Length)
                {
                    throw new FP_IllegalOperationException("Passed additional lines argument and passed lines are not equal.");
                }

                if (!double.TryParse(parametersMatch.Groups[5].Value, out passedPaidValue))
                {
                    throw new FP_BadFormatOfArgumentException("Bad formatting of PAID value.");
                }
                else
                {
                    passedTotalAmmount = Math.Round(passedPaidValue, 2);
                }

                if (!double.TryParse(parametersMatch.Groups[7].Value, out passedTotalAmmount))
                {
                    throw new FP_BadFormatOfArgumentException("Bad formatting of TOTAL value.");
                }
                else
                {
                    passedTotalAmmount = Math.Round(passedTotalAmmount, 2);
                }


                if ((discountType != TotalDiscountType.NO_DISCOUNT && discountValueForTransaction != 0) || optionalTotalDiscountPercentage != 0)
                {
                    var totalPriceWithDiscount = CalculateTOTALSumWithDiscount(discountType,
                                                                                discountValueForTransaction,
                                                                                totalAmmountWithoutDiscounts,
                                                                                optionalTotalDiscountPercentage);

                    if (Math.Round(totalPriceWithDiscount, 2) != passedTotalAmmount)
                    {
                        throw new FP_IllegalOperationException("Wrong passed TOTAL value. It not equals with passed fiscal printer line and total discount.");
                    }
                }



                if (command.PnArguments.Count() == 6
                    && discountType != TotalDiscountType.NO_DISCOUNT
                    && !double.TryParse(parametersMatch.Groups[9].Value, out discountValueForTransaction))
                {
                    throw new FP_BadFormatOfArgumentException("Bad formatting of DISCOUNT value.");
                }


                StringBuilder approveTransactionBuilder = new StringBuilder();
                approveTransactionBuilder.AppendLine("".PadRight(Constants.ReciptWidth, '-'));
                if (discountType != TotalDiscountType.NO_DISCOUNT)
                {
                    var subTotalLineRight = $"{totalAmmountWithoutDiscounts.ToString("0.00")} ";
                    var totalDiscountValueInPLN = Math.Round(totalAmmountWithoutDiscounts - passedTotalAmmount, 2);
                    var discountDescriptionText = totalDiscountValueInPLN < 0 ? "narzut" : "rabat";

                    var discountValue = Math.Abs(totalDiscountValueInPLN).ToString("0.00") + " ";

                    approveTransactionBuilder.AppendLine("Podsuma".PadRight(Constants.ReciptWidth - subTotalLineRight.Length) + subTotalLineRight);
                    approveTransactionBuilder.AppendLine($"    {discountDescriptionText}".PadRight(Constants.ReciptWidth - discountValue.Length) + discountValue);
                }

                var discountPercentage = ConvertDiscountToPercentage(discountType, discountValueForTransaction, totalAmmountWithoutDiscounts, optionalTotalDiscountPercentage);

                var ptuActualValues = state.PTURates
                    .Where(m => m.ActualPercentageValue < 100)
                    .ToDictionary(m => m.Type, m => m.ActualPercentageValue);

                var PTUsOverview = state.SlipLines
                    .GroupBy(m => m.PTU)
                    .Select(m => new
                    {
                        type = m.Key,
                        sum = m.Sum(slip => slip.TotalWithDiscount) * discountPercentage,
                        ptuPercentage = ptuActualValues[m.Key],
                        ptuVal =
                        ((m.Sum(slip => slip.TotalWithDiscount) * discountPercentage) * (ptuActualValues[m.Key] / 100))
                        /
                        (1 + (ptuActualValues[m.Key] / 100))
                    });

                foreach (var ptuOVerview in PTUsOverview)
                {
                    var totalInPTU = ptuOVerview.sum.ToString("0.00") + " ";
                    var totalInPTULeftLine = $"Sprzed. opodatk. {ptuOVerview.type.ToString()}"
                        .PadRight(Constants.ReciptWidth - totalInPTU.Length);
                    var totalPTUVal = ptuOVerview.ptuVal.ToString("0.00")+ " ";
                    var totalPTUValLeftLine = $"Kwota PTU {ptuOVerview.type.ToString()} {ptuOVerview.ptuPercentage} %"
                        .PadRight(Constants.ReciptWidth - totalPTUVal.Length);


                    approveTransactionBuilder.AppendLine(totalInPTULeftLine + totalInPTU);
                    approveTransactionBuilder.AppendLine(totalPTUValLeftLine + totalPTUVal);
                }
                var totalPTUsValue = PTUsOverview.Sum(m => m.ptuVal).ToString("0.00") + " ";
                approveTransactionBuilder.AppendLine("ŁĄCZNA KWOTA PTU".PadRight(Constants.ReciptWidth - totalPTUsValue.Length) + totalPTUsValue);

                var totalSumArray = passedTotalAmmount.ToString("0.00").ToArray();
                var totalSum = string.Join(" ", totalSumArray) + " ";
                approveTransactionBuilder.AppendLine("S U M A".PadRight(Constants.ReciptWidth - totalSum.Length) + totalSum);

                approveTransactionBuilder.AppendLine("".PadRight(Constants.ReciptWidth, '-'));

                if (passedPaidValue > passedTotalAmmount)
                {

                    var paidValue = passedPaidValue.ToString("0.00");
                    approveTransactionBuilder.AppendLine("Gotówka".PadRight(Constants.ReciptWidth - totalSum.Length) + totalSum);

                    var changeValue = (passedPaidValue - passedTotalAmmount).ToString("0.00");
                    approveTransactionBuilder.AppendLine("Reszta".PadRight(Constants.ReciptWidth - changeValue.Length) + changeValue);
                }

                var printId = new Random().Next(0, 9999).ToString("0000");
                var footerLeftLine = $"{printId} #Kasa: {state.PrinterCode}  Kasjer: {state.CashierLogin}";
                var transactionTime = DateTime.Now.AddMinutes(state.TimeDiffrenceInMinutes).ToString("HH:mm");
                approveTransactionBuilder.AppendLine(footerLeftLine.PadRight(Constants.ReciptWidth - transactionTime.Length) + transactionTime);

                var fiscalIdAndLogo = "{PL} ABC " + new Random().Next(10000000, 99999999);
                approveTransactionBuilder.AppendLine(fiscalIdAndLogo.PadCenter(Constants.ReciptWidth));
                approveTransactionBuilder.AppendLine();

                foreach (var additionalLine in additionalLines)
                {
                    approveTransactionBuilder.AppendLine(additionalLine.PadCenter(Constants.ReciptWidth));
                }

                state.IsInTransactionState = false;

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
            var actualTime =DateTime.Now.AddMinutes(fiscalPrinterState.TimeDiffrenceInMinutes).ToString("HH:mm");
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

        private double CalculateTOTALSumWithDiscount(TotalDiscountType DiscountType,
                                                     double DiscountValue,
                                                     double TotalPrice,
                                                     int OptionalPercentageDiscountValue)
        {
            switch (DiscountType)
            {
                case TotalDiscountType.NO_DISCOUNT:
                    break;
                case TotalDiscountType.VALUE_DISCOUNT:
                    return TotalPrice - DiscountValue;
                case TotalDiscountType.PERCENTAGE_DISCOUNT:
                    return TotalPrice * (DiscountValue / 100);
                case TotalDiscountType.VALUE_COAT:
                    return TotalPrice + DiscountValue;
                case TotalDiscountType.PERCENTAGE_COAT:
                    return TotalPrice * (1 + (DiscountValue / 100));
                default:
                    break;
            }
            return OptionalPercentageDiscountValue == 0 ? TotalPrice : TotalPrice * (100 / OptionalPercentageDiscountValue);
        }

        private double ConvertDiscountToPercentage(TotalDiscountType DiscountType,
                                                     double DiscountValue,
                                                     double TotalPrice,
                                                     int OptionalPercentageDiscountValue)
        {
            var discountPercentageValue = CalculateTOTALSumWithDiscount(DiscountType, DiscountValue, TotalPrice, OptionalPercentageDiscountValue) / TotalPrice;

            return Math.Round(discountPercentageValue, 2);
        }
    }
}
