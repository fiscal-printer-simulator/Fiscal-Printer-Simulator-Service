using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using System;
using System.Linq;
using System.Text;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSETHDR
    /// </summary>
    public class SetupReciptHeaderCommandHandler : BaseCommandHandler
    {
        public SetupReciptHeaderCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            var headerEndIndex = command.Parameters.IndexOf((char)(0xFF));
            if(headerEndIndex == -1)
            {
                throw new FP_IllegalOperationException("End Sign of Recipt Header was not found in passed command.");
            }
            var headerValue = command.Parameters.Substring(0, headerEndIndex);
            if (headerValue.Length > 500)
            {
                throw new FP_IllegalOperationException("Header value is too long");
            }
            var headerLines = headerValue.Split(Convert.ToChar(Constants.ASCICodeCR), Convert.ToChar(Constants.ASCICodeLF));


            StringBuilder reciptHeaderBuilder = new StringBuilder();
            headerLines.ToList().ForEach(e =>
            {
                reciptHeaderBuilder.AppendLine(e);
            });

            fiscalPrinterState.FiscalPrinterHeader = reciptHeaderBuilder.ToString();

            return new CommandHandlerResponse();
        }

    }
}
