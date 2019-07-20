using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalPrinterSimulatorLibraries.Models
{
    public enum ErrorHandlingType
    {
        STATEMENT_WITH_STOP,
        /// <summary>
        /// Error checked by commands LBFSTRS or LBERNQ
        /// </summary>
        WITHOUT_STATEMENT_WITHOUT_STOP,
        STATEMENT_WITH_STOP_AUTOMATE_SEND_ERROR,
        WITHOUT_STATEMENT_WITHOUT_STOP_AUTOMATE_SEND_ERROR
    }
}
