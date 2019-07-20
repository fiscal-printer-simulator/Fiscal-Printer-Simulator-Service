using System;
using System.Linq;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSETRAB
    /// </summary>
    public class SetupDiscountCalculationMethodCommandHandler : BaseCommandHandler
    {
        public SetupDiscountCalculationMethodCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            if (fiscalPrinterState.TimeDiffrenceInMinutes == int.MinValue)
            {
                throw new FP_IllegalOperationException("Timer RTC not initialized.");
            }
            if (command.PnArguments.Count() != 0)
            {
                throw new FP_WrongNumberOfArgumentsException("Fiscal Printer Expects only one prameter.");
            }
            else if (Enum.TryParse(command.PnArguments.First(), out DiscountCalculationMethod discountType))
            {
                fiscalPrinterState.DiscountCalculationType = discountType;
                return new CommandHandlerResponse();
            }
            else
            {
                throw new FP_BadFormatOfArgumentException("Argument must be a bit type (0 or 1 value)");
            }

        }
    }
}
