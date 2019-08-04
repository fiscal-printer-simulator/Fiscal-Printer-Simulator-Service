using System.Collections.Generic;
using System.Text;
using FiscalPrinterSimulatorLibraries.Extensions;
using FiscalPrinterSimulatorLibraries.Models;
using static ThermalFiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace ThermalFiscalPrinterSimulatorLibraries.Models
{
    public class FiscalPrinterState : IFiscalPrinterState
    {
        public FiscalPrinterState()
        {
            TransactionCounter = 0;
            LinesOnTransaction = -1;
            TimeDiffrenceInMinutes = int.MinValue;
            NextFiscalPrinterReciptId = 1;
            DiscountCalculationType = DiscountCalculationMethod.Standard;
            SlipLines = new List<SlipLine>();
            PTURates = new List<PTUType>
            {
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.A},
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.B},
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.C},
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.D},
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.E},
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.F},
                new PTUType{ ActualPercentageValue = 101, TotalValueOfSalesInType = 0 , Type = PTU.G},
            };
            FiscalPrinterHeader = CreateDefaulHeader();
        }

        public string CashierLogin { get; set; }
        public string PrinterCode { get; set; }


        public DiscountCalculationMethod DiscountCalculationType;
        public List<SlipLine> SlipLines;
        public string FiscalPrinterHeader;

        public int TimeDiffrenceInMinutes;
        public bool IsInOnlineMode;
        public bool IsInOutOfPaperState;
        public bool IsInInternalErrorState;

        public bool IsInFiscalState;
        public bool LastCommandSuccess;
        public bool IsInTransactionState;
        public bool LastTransactionSuccess;

        public int TransactionCounter { get; set; }
        public int LinesOnTransaction { get; set; }
        public int NextFiscalPrinterReciptId;

        public List<PTUType> PTURates { get; }

        public double ActualDrawerAmmount { get; set; }

        public ErrorHandlingType ErrorHandlingType { get; set; }

        private string CreateDefaulHeader()
        {
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.AppendLine("Michal Wojcik".PadCenter(Constants.ReciptWidth));
            headerBuilder.AppendLine(".Net Developer".PadCenter(Constants.ReciptWidth));
            headerBuilder.AppendLine("40-000 Katowice".PadCenter(Constants.ReciptWidth));
            headerBuilder.AppendLine("NIP 999-99-99-999".PadCenter(Constants.ReciptWidth));
            headerBuilder.AppendLine();
            return headerBuilder.ToString();
        }
    }
}
