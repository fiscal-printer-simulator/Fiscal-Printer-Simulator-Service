using System;
using System.Linq;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    public class OneByteFiscalPrinterCommand : FiscalPrinterCommand
    {
        public OneByteFiscalPrinterCommand(byte byteChar)
            : base(Enumerable.Empty<string>(), Convert.ToChar(byteChar).ToString(), string.Empty) { }

        public override string ToString() => Name.ToString();

        public override byte[] ToBytesArray() => Constants.ThermalBaseEncoding.GetBytes(Name);

    }
}
