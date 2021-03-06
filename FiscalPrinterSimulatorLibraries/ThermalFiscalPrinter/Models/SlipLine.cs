﻿using static ThermalFiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace ThermalFiscalPrinterSimulatorLibraries.Models
{
    public class SlipLine
    {
        public string ProductName { get; set; }
        public double Ammount { get; set; }
        public PTU PTU { get; set; }
        public double TotalPrice { get; set; }
        public double ProductPrice { get; set; }
        public double DiscountValue { get; set; }
        public double TotalWithDiscount
        {
            get
            {
                return DiscountValue < 1 ? TotalPrice * (1 - DiscountValue) : TotalPrice - DiscountValue;
            }
        }
    }
}
