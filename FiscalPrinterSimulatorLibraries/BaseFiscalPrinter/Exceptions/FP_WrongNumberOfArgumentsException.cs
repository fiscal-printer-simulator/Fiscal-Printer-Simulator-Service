using System;

namespace FiscalPrinterSimulatorLibraries.Exceptions
{
    public class FP_WrongNumberOfArgumentsException : Exception
    {
        public FP_WrongNumberOfArgumentsException() : base()
        {

        }

        public FP_WrongNumberOfArgumentsException(string message) :base(message)
        {

        }
    }
}
