using System;
using System.Linq;
using System.Text;
using FiscalPrinterSimulatorLibraries;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;
using static ThermalFiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    ///  Command handler for command LBTRSLN
    /// </summary>
    public class PrintSlipLineCommandHandler : BaseCommandHandler
    {
        public PrintSlipLineCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;
            var slipLine = new SlipLine();


            DiscountType discountType = DiscountType.NO_DISCOUNT;
            DiscountDescription discountDescription = DiscountDescription.NONE;

            string discountDescriptionText = "";
            string productDescription = "";
            double discountValueAmmount = 0;



            if (!command.PnArguments.Any())
            {
                throw new FP_WrongNumberOfArgumentsException("This command required at least one argument");
            }

            if (command.PnArguments.Count() >= 1)
            {
                if (!int.TryParse(command.PnArguments.ElementAt(0), out int lineNumber))
                {
                    throw new FP_BadFormatOfArgumentException("first argument must be a number");
                }
            }
            if (command.PnArguments.Count() >= 2)
            {
                if (!Enum.TryParse<DiscountType>(command.PnArguments.ElementAt(1), out discountType))
                {
                    throw new FP_BadFormatOfArgumentException("Unknkown Pr command argument");
                }
            }

            if (command.PnArguments.Count() >= 3)
            {
                if (!Enum.TryParse<DiscountDescription>(command.PnArguments.ElementAt(1), out discountDescription))
                {
                    throw new FP_BadFormatOfArgumentException("Unknkown Po command argument");
                }
            }

            if (command.PnArguments.Count() == 4 && command.PnArguments.ElementAt(3) != "1")
            {
                throw new FP_BadFormatOfArgumentException("Bad command formatting. Check your arguments.");
            }
            if (command.PnArguments.Count() > 4)
            {
                throw new FP_BadFormatOfArgumentException("Bad number of arguments for this command.");
            }

            var commandParameters = command.Parameters.Split('/', (char)Constants.ASCICodeCR).Where(m => !string.IsNullOrEmpty(m)).ToArray();

            if (commandParameters.Length == 0)
            {
                throw new FP_WrongNumberOfArgumentsException("Cannot get any parameters from command.");
            }
            if (commandParameters.Length >= 1)
            {
                if (string.IsNullOrWhiteSpace(commandParameters[0]))
                {
                    throw new FP_BadFormatOfArgumentException("Product name cannot be null");
                }
                else if (commandParameters[0].Length > 40)
                {
                    throw new FP_BadFormatOfArgumentException("Product name cannot have more then 40 characters.");
                }
                else
                {
                    slipLine.ProductName = commandParameters[0];
                }
            }
            if (commandParameters.Length >= 2)
            {
                if (!double.TryParse(commandParameters[1], out double ammount))
                {
                    throw new FP_BadFormatOfArgumentException("Cannot parse ammount of product. Check formatting.");
                }
                else if (ammount == 0)
                {
                    throw new FP_BadFormatOfArgumentException("Ammoutn cannot be 0.");
                }
                else
                {
                    slipLine.Ammount = ammount;
                }
            }
            if (commandParameters.Length >= 3)
            {
                if (!Enum.TryParse<PTU>(commandParameters[2], out PTU ptu))
                {
                    throw new FP_BadFormatOfArgumentException("Cannot parse PTU type. Check if you pass right parameter.");
                }
                else
                {
                    slipLine.PTU = ptu;
                }
            }
            if (commandParameters.Length >= 4)
            {
                if (!double.TryParse(commandParameters[3], out double price))
                {
                    throw new FP_BadFormatOfArgumentException("Cannot parse product price. Check if you pass right parameter.");
                }
                else
                {
                    slipLine.ProductPrice = price;
                }
            }

            if (commandParameters.Length >= 5)
            {
                if (!double.TryParse(commandParameters[4], out double brutto))
                {
                    throw new FP_BadFormatOfArgumentException("Cannot parse product brutto price. Check if you pass right parameter.");
                }
                else
                {
                    slipLine.TotalPrice = brutto;
                }
            }

            if (commandParameters.Length >= 6)
            {
                if (!double.TryParse(commandParameters[5], out double discountAmmount))
                {
                    throw new FP_BadFormatOfArgumentException("Cannot parse product discount ammout. Check if you pass right parameter.");
                }
                else
                {
                    slipLine.DiscountValue = discountAmmount;
                }
            }

            if (commandParameters.Length >= 7)
            {
                if (discountDescription == DiscountDescription.CUSTOM && string.IsNullOrWhiteSpace(commandParameters[6]))
                {
                    throw new FP_BadFormatOfArgumentException("Description cannot be empty in custom type");
                }
                else if (discountDescription == DiscountDescription.CUSTOM && commandParameters[6].Length > 20)
                {
                    throw new FP_BadFormatOfArgumentException("Description cannot have more then 20 characters.");
                }
                else
                {
                    discountDescriptionText = commandParameters[6];
                }
            }

            if (commandParameters.Length == 8)
            {
                productDescription = commandParameters[7];
            }
            if (commandParameters.Length > 8)
            {
                throw new FP_WrongNumberOfArgumentsException("Too many parameters for this command.");
            }

            StringBuilder slipBuilder = new StringBuilder();


            if (!state.SlipLines.Any())
            {
                var fiscalPrinterDate = new DateTime().AddMinutes(state.TimeDiffrenceInMinutes).ToString("YYYY-MM-DD");
                var transactionCounter = state.TransactionCounter.ToString();
                slipBuilder.AppendLine(fiscalPrinterDate.PadRight(Constants.ReciptWidth - transactionCounter.Length) + transactionCounter);
                slipBuilder.AppendLine("P A R A G O N  F I S K A L N Y".PadCenter(Constants.ReciptWidth));
                slipBuilder.AppendLine("".PadLeft(Constants.ReciptWidth, '-'));

            }


            if (slipLine.Ammount * slipLine.ProductPrice != slipLine.TotalPrice)
            {
                throw new FP_IllegalOperationException("Ammount x Price is not equal brutto value");
            }
            else
            {
                discountValueAmmount = slipLine.TotalPrice;
            }

            string financialSlipText = $"{slipLine.Ammount}x{slipLine.ProductPrice.ToString("0.00")}    {slipLine.TotalPrice.ToString("0.00")}{slipLine.PTU.ToString()}";
            if (Constants.ReciptWidth - financialSlipText.Length < slipLine.ProductName.Length)
            {
                slipBuilder.AppendLine(slipLine.ProductName.PadRight(Constants.ReciptWidth));
                slipBuilder.AppendLine(financialSlipText.PadLeft(Constants.ReciptWidth));
            }
            else
            {
                var paddingValue = Constants.ReciptWidth - financialSlipText.Length;
                slipBuilder.AppendLine(slipLine.ProductName.PadRight(paddingValue) + financialSlipText);
            }

            if (!string.IsNullOrWhiteSpace(productDescription))
            {
                slipBuilder.AppendLine("Opis: " + productDescription);
            }

            if (discountDescription != DiscountDescription.NONE && slipLine.DiscountValue != 0)
            {

                discountDescriptionText = discountDescription == DiscountDescription.CUSTOM
                                        ? discountDescriptionText
                                        : GetDiscountDescription(discountDescription);


                bool isPercentageDiscount = slipLine.DiscountValue < 1 && slipLine.DiscountValue > 0;


                var discountValueText = isPercentageDiscount
                                        ? (slipLine.DiscountValue * 100).ToString() + " %"
                                        : slipLine.DiscountValue.ToString("0.00");

                discountValueAmmount -= isPercentageDiscount ? slipLine.TotalPrice * slipLine.DiscountValue : slipLine.DiscountValue;


                var discountSlipLineLeftPart = $"   {discountDescriptionText} {discountValueText} =";
                var discountSlipLineRightPart = $"-{(slipLine.TotalPrice - discountValueAmmount).ToString("0.00")} ";


                slipBuilder.AppendLine(discountSlipLineLeftPart.PadRight(
                                Constants.ReciptWidth - discountSlipLineRightPart.Length)
                                + discountSlipLineRightPart);

            }

            slipBuilder.AppendLine($"{discountValueAmmount.ToString("0.00")}{slipLine.PTU.ToString()}".PadLeft(Constants.ReciptWidth));


            state.SlipLines.Add(slipLine);
            return new CommandHandlerResponse(slipBuilder.ToString());
        }


        private string GetDiscountDescription(DiscountDescription descriptionType)
        {
            switch (descriptionType)
            {
                case DiscountDescription.NONE:
                    break;
                case DiscountDescription.SPECIAL:
                    return "rabat spacjalny";
                case DiscountDescription.OCASIONAL:
                    return "return okolicznościowy";
                case DiscountDescription.CHANCE:
                    return "okazja";
                case DiscountDescription.CHRISTMAS:
                    return "rabat świąteczny";
                case DiscountDescription.REGULAR_CUSTOMER:
                    return "stały klient";
                case DiscountDescription.ANNIVERSARY:
                    return "rabat jubileuszowy";
                case DiscountDescription.BIRTHDAY:
                    return "rabat urodzinowy";
                case DiscountDescription.EMPLOYEE:
                    return "rabat dla pracownika";
                case DiscountDescription.PROMOTION:
                    return "promocja";
                case DiscountDescription.REWARD:
                    return "nagroda";
                case DiscountDescription.SALE:
                    return "wyprzedaż";
                case DiscountDescription.DISCOUNT:
                    return "przecena";
                case DiscountDescription.SEASONAL:
                    return "rabat sezonowy";
                case DiscountDescription.NIGHTLY:
                    return "rabat nocny";
                case DiscountDescription.STAFF:
                    return "obsługa";
                case DiscountDescription.CUSTOM:
                    break;
                default:
                    break;
            }
            return string.Empty;
        }

    }
}
