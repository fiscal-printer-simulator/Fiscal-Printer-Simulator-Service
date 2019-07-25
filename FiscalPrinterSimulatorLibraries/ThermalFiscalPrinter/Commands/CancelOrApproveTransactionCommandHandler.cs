﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


                // here will be continous command handling




                throw new NotImplementedException("Not implemented yet. I'm working on it.");

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