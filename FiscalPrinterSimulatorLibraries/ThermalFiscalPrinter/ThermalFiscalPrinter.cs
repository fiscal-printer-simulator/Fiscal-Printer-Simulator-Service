using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThermalFiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using FiscalPrinterSimulatorLibraries;

namespace ThermalFiscalPrinterSimulatorLibraries
{
    public class ThermalFiscalPrinter : IFiscalPrinter
    {
        private readonly FiscalPrinterState _state;

        public ThermalFiscalPrinter() => _state = new FiscalPrinterState();

        public string ProtocolName => Constants.ProtocolName;

        public IEnumerable<CommandHandlerResponse> HandleReceivedData(string receivedStringData)
        {
            var commandHandlerResults = new List<CommandHandlerResponse>();
            try
            {
                foreach (var command in SplitByCommands(receivedStringData))
                {
                    var commandHandler = ThermalCommandHandlerFactory.Create(command);
                    var commandHandlerResult = commandHandler.Handle(_state);
                    _state.LastCommandSuccess = true;
                    commandHandlerResults.Add(commandHandlerResult);
                }
            }
            catch
            {
                _state.LastCommandSuccess = false;
            }
            return commandHandlerResults;
        }

        private IEnumerable<BaseFiscalPrinterCommand> SplitByCommands(string inputCommandChars)
        {
            string splittingPattern = @"\x1bP((.*)([\^\@\#\$][a-zA-Z])(([^\x1b\\]+[\x0d\n/\?])+(?=[\w\d]{2}))*)([\w\d]{2})*\x1b\\|(\x10)|(\x18)|(\x07)|(\x05)";
            Regex regex = new Regex(splittingPattern);
            MatchCollection matches = regex.Matches(inputCommandChars);

            for (int i = 0; i < matches.Count; i++)
            {
                var matchesGroups = matches[i].Groups;

                var checksumBaseToCalc = matchesGroups[1].Value;
                var pnArguments = matchesGroups[2].Value.Split(';');
                var commandName = "";

                if (matchesGroups.Count > 3 && matchesGroups[3].Success)
                {
                    commandName = matchesGroups[3].Value;
                }
                else if (matchesGroups.Count > 7 && matchesGroups[7].Success)
                {
                    commandName = nameof(Constants.ASCICodeDLE);
                }
                else if (matchesGroups.Count > 8 && matchesGroups[8].Success)
                {
                    commandName = nameof(Constants.ASCICodeCAN);
                }
                else if (matchesGroups.Count > 9 && matchesGroups[9].Success)
                {
                    commandName = nameof(Constants.ASCICodeBEL);
                }
                else if (matchesGroups.Count > 10 && matchesGroups[10].Success)
                {
                    commandName = nameof(Constants.ASCICodeENQ);
                }

                var parameters = matchesGroups[4].Value;
                var passedChecksum = matchesGroups[6].Value;
                yield return new ThermalFiscalPrinterCommand(pnArguments, commandName, parameters, passedChecksum, checksumBaseToCalc);

            }
        }
    }
}
