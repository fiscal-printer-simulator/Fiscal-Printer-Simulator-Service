using System.Text;

namespace ThermalFiscalPrinterSimulatorLibraries
{
    public class Constants
    {
        /// <summary>
        /// Name of used Fiscal Printer Protocol
        /// </summary>
        public const string ProtocolName = "POSNET Thermal";

        /// <summary>
        /// Version of used Fiscal Printer Protocol
        /// </summary>
        public const double ProtocolVersion = 4.01;

        /// <summary>
        /// Basic encoding used by Thermal protocol to send fiscal printer commands
        /// </summary>
        public static Encoding ThermalBaseEncoding = Encoding.ASCII;

        /// <summary>
        /// Send FP Status (Base)
        /// </summary>
        public const byte ASCICodeDLE = 0x10;

        /// <summary>
        /// Abort Transaction
        /// </summary>
        public const byte ASCICodeCAN = 0x18;

        /// <summary>
        /// Beep
        /// </summary>
        public const byte ASCICodeBEL = 0x07;

        /// <summary>
        /// Send FP Status (Transactions)
        /// </summary>
        public const byte ASCICodeENQ = 0x05;

        /// <summary>
        /// FirstBitEveryOtherCommand "ESC"
        /// </summary>
        /// 
        public const byte ASCICodeESC = 0x1B;

        /// <summary>
        /// Second Bit of Every Other Command
        /// </summary>
        public const byte ASCICodeP = 0x50;

        /// <summary>
        /// End Bit Other Command "\"
        /// </summary>
        public const byte ASCICodeEndCommand = 0x5C;

        /// <summary>
        /// Inactive PTU Rate it means item can't be sell with this rate type.
        /// </summary>
        public const double PTUInactiveRate = 101;
        /// <summary>
        /// Tax Free PTU Rate. This means that the goods are exempt from taxation.
        /// </summary>
        public const double PTUTaxFreeRate = 100;

        /// <summary>
        /// Max number of characters in one recipt line.
        /// </summary>
        public const int ReciptWidth = 40;

        /// <summary>
        /// Carriage Return ASCI Sign. It can be used to separate parameters in a command.
        /// </summary>
        public const byte ASCICodeCR = 0x0d;
        /// <summary>
        /// Line Feed.  It can be used to separate parameters in a command.
        /// </summary>
        public const byte ASCICodeLF = 0x0a;
    }
}
