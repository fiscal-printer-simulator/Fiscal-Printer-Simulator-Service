using static ThermalFiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace ThermalFiscalPrinterSimulatorLibraries.Models
{
    public class PTUTypes
    {
        public enum PTU
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G

        }
    }

    public class PTUType
    {
        public PTU Type { get; set; }
        public double ActualPercentageValue { get; set; }
        public double TotalValueOfSalesInType { get; set; }
    }
}
