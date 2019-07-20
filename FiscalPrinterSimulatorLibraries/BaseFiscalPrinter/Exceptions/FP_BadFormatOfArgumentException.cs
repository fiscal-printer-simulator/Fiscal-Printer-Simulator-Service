using System;

namespace FiscalPrinterSimulatorLibraries.Exceptions
{
    public class FP_BadFormatOfArgumentException : Exception
    {
        public FP_BadFormatOfArgumentException() : base()
        {

        }

        public FP_BadFormatOfArgumentException(string message) :base(message)
        {

        }
    }
}
