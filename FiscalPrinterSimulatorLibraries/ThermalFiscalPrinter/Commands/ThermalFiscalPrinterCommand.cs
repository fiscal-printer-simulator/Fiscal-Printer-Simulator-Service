using System.Collections.Generic;
using FiscalPrinterSimulatorLibraries.Commands;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    public class ThermalFiscalPrinterCommand : BaseFiscalPrinterCommand
    {
        public ThermalFiscalPrinterCommand(IEnumerable<string> PnArguments, string Name, string Parameters) : base(PnArguments, Name, Parameters)
        {
        }

        public ThermalFiscalPrinterCommand(IEnumerable<string> PnArguments, string Name, string Parameters, string PassedChecksum, string ChecksumBaseToCalculation) : base(PnArguments, Name, Parameters, PassedChecksum, ChecksumBaseToCalculation)
        {
        }


        public override string ToString()
        {
            return Constants.ASCICodeESC + "P" + string.Join(";", PnArguments) + Name + Parameters + Constants.ASCICodeESC + Constants.ASCICodeEndCommand;
        }
    }
}
