using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using System;
using System.Linq;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSERM
    /// </summary>
    public class SetupErrorHandlingCommandHandler : BaseCommandHandler
    {
        public SetupErrorHandlingCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

            if (command.PnArguments.Count() != 1)
            {
                throw new FP_WrongNumberOfArgumentsException("Required one Pn Argument that describes type of error handling");
            }
            var errorHandlingTypeArgument = command.PnArguments.First();
            if (!int.TryParse(errorHandlingTypeArgument, out _) || !Enum.TryParse(errorHandlingTypeArgument, out ErrorHandlingType errorHandlingType))
            {
                throw new FP_IllegalOperationException("Error handling type argument must be numeric and be between 0 and 3");
            }

            state.ErrorHandlingType = errorHandlingType;

            return new CommandHandlerResponse();
        }
    }
}
