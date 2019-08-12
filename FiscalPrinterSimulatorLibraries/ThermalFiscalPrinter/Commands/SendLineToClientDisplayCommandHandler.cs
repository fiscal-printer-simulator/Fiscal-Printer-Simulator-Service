using System;
using System.Linq;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBDSPLINE
    /// </summary>
    public class SendLineToClientDisplayCommandHandler : BaseCommandHandler
    {
        private const string _firstLineIdentity = "101";
        private const string _secondLineIdentity = "102";

        public SendLineToClientDisplayCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            if (!command.PnArguments.Any() || command.PnArguments.Count() > 1)
            {
                throw new FP_WrongNumberOfArgumentsException("Wrong number of arguments. Expected only one.");
            }

            var firstArgumentValue = command.PnArguments.ElementAt(0);
            
            if(firstArgumentValue != _firstLineIdentity && firstArgumentValue != _secondLineIdentity)
            {
                throw new FP_BadFormatOfArgumentException("Passed wrong argument expected 101 or 102.");
            }

            if(command.Parameters.Length > 20)
            {
                throw new FP_BadFormatOfArgumentException("Too much characters defined to display on Client Line display. Maximum is 20.");
            }
            

            var outputClientLineResult = new ClientLineDisplayOutput()
            {
                LineNumber = firstArgumentValue == _firstLineIdentity ? 0 : 1,
                OutputText = command.Parameters
            };

            return new CommandHandlerResponse(outputClientLineResult);
        }
    }
}
