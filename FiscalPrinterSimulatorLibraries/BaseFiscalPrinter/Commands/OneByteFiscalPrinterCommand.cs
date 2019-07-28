using System;
using System.Linq;

namespace FiscalPrinterSimulatorLibraries.Commands
{
    public class OneByteFiscalPrinterCommand : BaseFiscalPrinterCommand
    {
        public OneByteFiscalPrinterCommand(byte byteChar)
            : base(Enumerable.Empty<string>(), Convert.ToChar(byteChar).ToString(), string.Empty) { }

        public override string ToString() => Name.ToString();

        public override byte[] ToBytesArray() => _encoding.GetBytes(Name);

    }
}
