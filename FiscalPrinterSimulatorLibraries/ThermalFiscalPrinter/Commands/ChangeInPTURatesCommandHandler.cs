using System;
using System.Linq;
using System.Text;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;
using static FiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSETPTU
    /// </summary>
    public class ChangePTURatesCommandHandler : BaseCommandHandler
    {
        public ChangePTURatesCommandHandler(FiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(FiscalPrinterState fiscalPrinterState)
        {
            if (command.PnArguments is null || !command.PnArguments.Any())
            {
                throw new FP_WrongNumberOfArgumentsException("Arguments missing");
            }

            if (!int.TryParse(command.PnArguments.First(), out int numberOfRates))
            {
                throw new FP_BadFormatOfArgumentException("First argument is not in numeric format");
            }

            if (numberOfRates > fiscalPrinterState.PTURates.Count)
            {
                throw new FP_IllegalOperationException("Maximum of PTU Rates is 7.");
            }

            var ptuParameters = command.Parameters.Split('/').Where(m => !string.IsNullOrEmpty(m)).ToArray();

            if (ptuParameters.Length != numberOfRates)
            {
                throw new FP_WrongNumberOfArgumentsException("First argument not contain right number of PTU rates.");
            }



            StringBuilder sb = new StringBuilder();

            sb.AppendLine("N I E F I S K A L N Y".PadCenter(Constants.ReciptWidth));
            sb.AppendLine("Z m i a n a  s t a w e k  P T U".PadCenter(Constants.ReciptWidth));
            sb.AppendLine("Stare PTU:".PadRight(Constants.ReciptWidth));
            fiscalPrinterState.PTURates.ForEach(key =>
            {
                sb.AppendLine(PrintPTUValuesOnRecipt(key.Type, fiscalPrinterState));
            });
            sb.AppendLine("-".PadRight(Constants.ReciptWidth, '-'));

            ChangePTURatesByCommand(fiscalPrinterState, ptuParameters);

            sb.AppendLine("Nowe PTU:".PadRight(Constants.ReciptWidth));
            fiscalPrinterState.PTURates.ForEach(key =>
            {
                sb.AppendLine(PrintPTUValuesOnRecipt(key.Type, fiscalPrinterState));
            });
            sb.AppendLine("N I E F I S K A L N Y".PadCenter(Constants.ReciptWidth));

            var lastErrorCode = "0";
            var fiscalState = fiscalPrinterState.IsInFiscalState ? "1" : "0";
            var transactionState = fiscalPrinterState.IsInTransactionState ? "1" : "0";
            var lastTransactionState = fiscalPrinterState.LastTransactionSuccess ? "1" : "0";

            var outputCommandArguments = new string[]
            {
                 lastErrorCode ,
                fiscalState,
                transactionState,
                lastTransactionState,
                "0",
                "1",
                DateTime.Now.Year.ToString().Substring(2,2),
                DateTime.Now.Month.ToString(),
                DateTime.Now.Day.ToString()
            };

            var outputCommandParametersString =
                string.Join(";", outputCommandArguments) + "/" +
                string.Join("/", fiscalPrinterState.PTURates.Select(m => m.ActualPercentageValue)) + (fiscalPrinterState.NextFiscalPrinterReciptId - 1) + "/" +
                string.Join("/", fiscalPrinterState.PTURates.Select(m => m.TotalValueOfSalesInType)) + "/" + fiscalPrinterState.ActualDrawerAmmount + "ABC12345678";

            var outputCommand = new FiscalPrinterCommand(outputCommandArguments, "#X", outputCommandParametersString);


            return new CommandHandlerResponse(outputCommand, sb.ToString());
        }

        private string PrintPTUValuesOnRecipt(PTU PTUType, FiscalPrinterState state)
        {
            var ptuValue = state.PTURates.First(m => m.Type == PTUType).ActualPercentageValue;

            var ptuRateString = ptuValue == Constants.PTUInactiveRate ? "---" : ptuValue == Constants.PTUTaxFreeRate ? "SP.ZW.PTU" : ptuValue.ToString("0.00") + " %";
            var numberOfSpace = Constants.ReciptWidth - ptuRateString.Length;
            return $"PTU {PTUType.ToString()}".PadRight(numberOfSpace) + ptuRateString;
        }



        private static void ChangePTURatesByCommand(FiscalPrinterState fiscalPrinterState, string[] ptuParameters)
        {
            if (ptuParameters.Length > 0)
            {
                fiscalPrinterState.PTURates[(int)PTU.A].ActualPercentageValue =
                    double.TryParse(ptuParameters[0], out double rateValue) ?
                    rateValue :
                    Constants.PTUInactiveRate;
            }
            if (ptuParameters.Length > 1)
            {
                fiscalPrinterState.PTURates[(int)PTU.B].ActualPercentageValue =
                                   double.TryParse(ptuParameters[1], out double rateValue) ?
                                   rateValue :
                                   Constants.PTUInactiveRate;
            }
            if (ptuParameters.Length > 2)
            {
                fiscalPrinterState.PTURates[(int)PTU.C].ActualPercentageValue =
                                   double.TryParse(ptuParameters[2], out double rateValue) ?
                                   rateValue :
                                   Constants.PTUInactiveRate;
            }
            if (ptuParameters.Length > 3)
            {
                fiscalPrinterState.PTURates[(int)PTU.D].ActualPercentageValue =
                                   double.TryParse(ptuParameters[3], out double rateValue) ?
                                   rateValue :
                                   Constants.PTUInactiveRate;
            }
            if (ptuParameters.Length > 4)
            {
                fiscalPrinterState.PTURates[(int)PTU.E].ActualPercentageValue =
                                   double.TryParse(ptuParameters[4], out double rateValue) ?
                                   rateValue :
                                   Constants.PTUInactiveRate;
            }
            if (ptuParameters.Length > 5)
            {
                fiscalPrinterState.PTURates[(int)PTU.F].ActualPercentageValue =
                                   double.TryParse(ptuParameters[5], out double rateValue) ?
                                   rateValue :
                                   Constants.PTUInactiveRate;
            }
            if (ptuParameters.Length > 6)
            {
                fiscalPrinterState.PTURates[(int)PTU.G].ActualPercentageValue =
                                   double.TryParse(ptuParameters[6], out double rateValue) ?
                                   rateValue :
                                   Constants.PTUInactiveRate;
            }
            else
            {
                fiscalPrinterState.PTURates[(int)PTU.G].ActualPercentageValue = Constants.PTUTaxFreeRate;
            }
        }
    }
}
