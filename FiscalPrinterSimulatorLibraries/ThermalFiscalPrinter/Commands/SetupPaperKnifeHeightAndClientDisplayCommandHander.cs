using System;
using System.Linq;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSETRAB
    /// </summary>
    public class SetupPaperKnifeHeightAndClientDisplayCommandHander : BaseCommandHandler
    {
        public SetupPaperKnifeHeightAndClientDisplayCommandHander(BaseFiscalPrinterCommand command) : base(command)
        {
        }
        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

            if (!command.PnArguments.Any())
            {
                throw new FP_WrongNumberOfArgumentsException("There is no arguments");
            }
            var firstArgument = command.PnArguments.ElementAt(0);
            if (firstArgument == "0" || firstArgument == "1")
            {
                if (state.TimeDiffrenceInMinutes == int.MinValue)
                {
                    throw new FP_IllegalOperationException("Timer RTC not initialized.");
                }
                Enum.TryParse(firstArgument, out DiscountCalculationMethod discountType);
                state.DiscountCalculationType = discountType;
                return new CommandHandlerResponse();
            }
            else if (command.PnArguments.Count() >= 2)
            {
                var secondArgument = command.PnArguments.ElementAt(1);
                if (secondArgument != "0" || secondArgument != "1")
                {
                    throw new FP_BadFormatOfArgumentException("Second argument must be a bit type (0 or 1 value)");
                }
                if (firstArgument == "3")
                {
                    //TODO: Save value to state ? 
                   // var papperSaveFunctionIsOn = secondArgument == "1";
                }
                else if (firstArgument == "4")
                {
                    //TODO: Save value to state ? 
                   // var showTransactionOnClientDisplay = secondArgument == "1";
                }
                else if (firstArgument == "6")
                {
                    //TODO: Save value to state ? 
                   // var KnifeHightUpper = secondArgument == "1";
                }
                else if (command.PnArguments.Count() == 3 && firstArgument == "5")
                {
                    var thirdArgument = command.PnArguments.ElementAt(2);
                    if (secondArgument == "0")
                    {
                        if (!Enum.TryParse<BacklightOption>(thirdArgument, out _))
                        {
                            throw new FP_BadFormatOfArgumentException("Backlight option support only [0,1,2] arguments");
                        }
                        //TODO: Save value to state ? 
                    }
                    if (secondArgument == "1")
                    {
                        if(!int.TryParse(thirdArgument,out int backlightBrightness) || backlightBrightness < 0 || backlightBrightness > 15)
                        {
                            throw new FP_BadFormatOfArgumentException("Backlight brightness must have value between 0 and 15");
                        }
                        //TODO: Save value to state ? 
                    }
                    if (secondArgument == "2")
                    {
                        if (!int.TryParse(thirdArgument, out int contrastValue) || contrastValue < 0 || contrastValue > 31)
                        {
                            throw new FP_BadFormatOfArgumentException("Contrast must have value between 0 and 31");
                        }
                        //TODO: Save value to state ? 
                    }
                }
            }

            throw new FP_BadFormatOfArgumentException("Cannot resolve this command. Bad first command argument");
        }
    }
}
