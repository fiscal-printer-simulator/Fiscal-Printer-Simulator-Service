﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    public class BaseFiscalPrinterCommand
    {
        protected readonly Encoding _encoding = Encoding.ASCII;

        public IEnumerable<string> PnArguments { get; }
        public string Name { get; }
        public string Parameters { get; }
        public bool IsChecksumValid { get; }
        public string ChecksumString { get; }

        public BaseFiscalPrinterCommand(IEnumerable<string> PnArguments, string Name, string Parameters, string PassedChecksum, string ChecksumBaseToCalculation)
        {
            this.Name = Name;
            this.Parameters = Parameters;
            this.PnArguments = PnArguments ?? new List<string>();
            this.ChecksumString = CalculateCheckSum(ChecksumBaseToCalculation);
            this.IsChecksumValid = this.ChecksumString == PassedChecksum;
        }


        public BaseFiscalPrinterCommand(IEnumerable<string> PnArguments, string Name, string Parameters)
        {
            this.Name = Name;
            this.PnArguments = PnArguments ?? new List<string>();
            this.Parameters = Parameters;
            this.ChecksumString = CalculateCheckSum(string.Join(";", this.PnArguments ?? new List<string>()) + Name + Parameters);
        }

        private string CalculateCheckSum(string checksumContent)
        {
            if (checksumContent is null)
            {
                return null;
            }
            var check = (byte)0xFF;
            _encoding.GetBytes(checksumContent).ToList().ForEach(@byte => check ^= @byte);

            return string.IsNullOrEmpty(checksumContent) ? "" : check.ToString("X2");
        }

       
        public virtual byte[] ToBytesArray()
        {
            return _encoding.GetBytes(this.ToString());
        }

    }
}
