using static FiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace FiscalPrinterSimulatorLibraries.Models
{
    public class SlipLine
    {
        public string ProductName { get; set; }
        public double Ammount { get; set; }
        public PTU PTU { get; set; }
        public double TotalPrice { get; set; }
        public double ProductPrice { get; set; }
        public double DiscountValue { get; set; }

    }
}
