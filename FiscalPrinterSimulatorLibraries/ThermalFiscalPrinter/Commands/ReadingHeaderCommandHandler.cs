using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiscalPrinterSimulatorLibraries.Models;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBGETHDR
    /// </summary>
    public class ReadingHeaderCommandHandler : BaseCommandHandler
    {
        public ReadingHeaderCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState) =>
            new CommandHandlerResponse(
                new FiscalPrinterCommand(new string[] { "1" }, "#U", fiscalPrinterState.FiscalPrinterHeader)
                );
    }
}
