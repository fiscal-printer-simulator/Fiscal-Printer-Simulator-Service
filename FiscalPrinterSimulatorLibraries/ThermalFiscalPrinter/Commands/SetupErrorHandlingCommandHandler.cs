using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using System;
using System.Linq;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSERM
    /// </summary>
    public class SetupErrorHandlingCommandHandler : BaseCommandHandler
    {
        public SetupErrorHandlingCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            if (command.PnArguments.Count() != 1)
            {
                throw new FP_WrongNumberOfArgumentsException("Required one Pn Argument that describes type of error handling");
            }
            var errorHandlingTypeArgument = command.PnArguments.First();
            if (!int.TryParse(errorHandlingTypeArgument, out _) || !Enum.TryParse(errorHandlingTypeArgument, out ErrorHandlingType errorHandlingType))
            {
                throw new FP_IllegalOperationException("Error handling type argument must be numeric and be between 0 and 3");
            }

            fiscalPrinterState.ErrorHandlingType = errorHandlingType;

            return new CommandHandlerResponse();
        }
    }
}
