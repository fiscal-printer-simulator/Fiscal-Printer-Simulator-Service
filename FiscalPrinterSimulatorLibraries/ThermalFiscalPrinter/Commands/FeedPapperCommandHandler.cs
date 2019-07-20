using System.Linq;
using System.Text;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBFEED
    /// </summary>
    public class FeedPapperCommandHandler : BaseCommandHandler
    {
        public FeedPapperCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            int minFeedPapperCount = 0;
            int maxFeedPapperCount = 20;

            if (fiscalPrinterState.IsInTransactionState)
            {
                throw new FP_IllegalOperationException("Option not avalible in fiscal transaction state.");
            }

            if (command.PnArguments.Count() != 1)
            {
                throw new FP_WrongNumberOfArgumentsException("Expected one parameter defining the number of lines to be pulled out.");
            }
            if (!int.TryParse(command.PnArguments.First(), out int feedPaperLinesCount) || feedPaperLinesCount < minFeedPapperCount || feedPaperLinesCount > maxFeedPapperCount)
            {
                throw new FP_BadFormatOfArgumentException("Number of feeded lines must be between 0 and 20.");
            }
            StringBuilder reciptBuilder = new StringBuilder();

            for (int i = 0; i < feedPaperLinesCount; i++) { reciptBuilder.AppendLine(""); }

            return new CommandHandlerResponse(reciptBuilder.ToString());
        }
    }
}
