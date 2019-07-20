using System.Collections.Generic;
using static FiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace FiscalPrinterSimulatorLibraries.Models
{
    public class FiscalPrinterState
    {
        public FiscalPrinterState()
        {
            LinesOnTransaction = -1;
            TimeDiffrenceInMinutes = int.MinValue;
            NextFiscalPrinterReciptId = 1;
            DiscountCalculationType = DiscountCalculationMethod.Standard;
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
        }
        public DiscountCalculationMethod DiscountCalculationType;

        public string FiscalPrinterHeader;

        public int TimeDiffrenceInMinutes;
        public bool IsInOnlineMode;
        public bool IsInOutOfPaperState;
        public bool IsInInternalErrorState;

        public bool IsInFiscalState;
        public bool LastCommandSuccess;
        public bool IsInTransactionState;
        public bool LastTransactionSuccess;

        public int LinesOnTransaction { get; set; }
        public int NextFiscalPrinterReciptId;

        public List<PTUType> PTURates { get; }

        public double ActualDrawerAmmount { get; set; }

        public ErrorHandlingType ErrorHandlingType { get; set; }
    }
}
