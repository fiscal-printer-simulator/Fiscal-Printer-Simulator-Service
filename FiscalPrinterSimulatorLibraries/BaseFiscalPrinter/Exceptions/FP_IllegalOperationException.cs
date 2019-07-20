using System;

namespace FiscalPrinterSimulatorLibraries.Exceptions
{
    public class FP_IllegalOperationException:Exception
    {
        public FP_IllegalOperationException():base()
        {

        }

        public FP_IllegalOperationException(string message):base(message)
        {

        }
    }
}
