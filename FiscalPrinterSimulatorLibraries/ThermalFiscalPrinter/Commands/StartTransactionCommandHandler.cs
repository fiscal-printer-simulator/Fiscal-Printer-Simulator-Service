using System.Collections.Generic;
using System.Linq;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBTRSHDR
    /// </summary>
    public class StartTransactionCommandHandler : BaseCommandHandler
    {
        public StartTransactionCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            try
            {
                if (!command.PnArguments.Any())
                {
                    throw new FP_WrongNumberOfArgumentsException("Command requires parameter with description about number of lines in transaction.");
                }
                else if (!int.TryParse(command.PnArguments.First(), out int numberOfLines))
                {
                    throw new FP_BadFormatOfArgumentException("Number of lines argument must be a number.");
                }
                else if (numberOfLines < 0 || numberOfLines > 80)
                {
                    throw new FP_IllegalOperationException("Number of lines argument must have value between 0 and 80.");
                }
                else
                {
                    fiscalPrinterState.LinesOnTransaction = numberOfLines;
                    fiscalPrinterState.IsInTransactionState = true;
                    fiscalPrinterState.SlipLines = new List<SlipLine>();
                    return new CommandHandlerResponse();
                }
            }
            catch
            {
                fiscalPrinterState.LinesOnTransaction = -1;
                fiscalPrinterState.IsInTransactionState = false;
                throw;
            }
        }
    }
}
