using System;

namespace FiscalPrinterSimulatorLibraries.Extensions
{
    public static class StringExtensions
    {
        public static string PadCenter(this String value, int totalWidth) =>
            value.PadLeft(((totalWidth - value.Length) / 2) + value.Length).PadRight(totalWidth);
    }
}
