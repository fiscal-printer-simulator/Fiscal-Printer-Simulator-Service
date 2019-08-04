using System.Collections.Generic;
using System.Linq;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBTRSHDR
    /// </summary>
    public class StartTransactionCommandHandler : BaseCommandHandler
    {
        public StartTransactionCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

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
                    state.LinesOnTransaction = numberOfLines;
                    state.IsInTransactionState = true;
                    state.SlipLines = new List<SlipLine>();
                    return new CommandHandlerResponse();
                }
            }
            catch
            {
                state.LinesOnTransaction = -1;
                state.IsInTransactionState = false;
                throw;
            }
        }
    }
}
