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
    public class SetupDiscountCalculationMethodCommandHandler : BaseCommandHandler
    {
        public SetupDiscountCalculationMethodCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

            if (state.TimeDiffrenceInMinutes == int.MinValue)
            {
                throw new FP_IllegalOperationException("Timer RTC not initialized.");
            }
            if (command.PnArguments.Count() != 0)
            {
                throw new FP_WrongNumberOfArgumentsException("Fiscal Printer Expects only one prameter.");
            }
            else if (Enum.TryParse(command.PnArguments.First(), out DiscountCalculationMethod discountType))
            {
                state.DiscountCalculationType = discountType;
                return new CommandHandlerResponse();
            }
            else
            {
                throw new FP_BadFormatOfArgumentException("Argument must be a bit type (0 or 1 value)");
            }

        }
    }
}
