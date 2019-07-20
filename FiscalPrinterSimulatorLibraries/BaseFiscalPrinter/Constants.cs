using System.Text;

namespace FiscalPrinterSimulatorLibraries
{
    public class Constants
    {
        /// <summary>
        /// Basic encoding used by Thermal protocol to send fiscal printer commands
        /// </summary>
        public static Encoding ThermalBaseEncoding = Encoding.ASCII;

        /// <summary>
        /// Send FP Status (Base)
        /// </summary>
        public static byte ASCICodeDLE = 0x10;

        /// <summary>
        /// Abort Transaction
        /// </summary>
        public static byte ASCICodeCAN = 0x18;

        /// <summary>
        /// Beep
        /// </summary>
        public static byte ASCICodeBEL = 0x07;

        /// <summary>
        /// Send FP Status (Transactions)
        /// </summary>
        public static byte ASCICodeENQ = 0x05;

        /// <summary>
        /// FirstBitEveryOtherCommand "ESC"
        /// </summary>
        /// 
        public static byte ASCICodeESC = 0x1B;

        /// <summary>
        /// Second Bit of Every Other Command
        /// </summary>
        public static byte ASCICodeP = 0x50;

        /// <summary>
        /// End Bit Other Command "\"
        /// </summary>
        public static byte ASCICodeEndCommand = 0x5C;

        /// <summary>
        /// Inactive PTU Rate it means item can't be sell with this rate type.
        /// </summary>
        public static double PTUInactiveRate = 101;
        /// <summary>
        /// Tax Free PTU Rate. This means that the goods are exempt from taxation.
        /// </summary>
        public static double PTUTaxFreeRate = 100;

        /// <summary>
        /// Max number of characters in one recipt line.
        /// </summary>
        public static int ReciptWidth = 40;

        /// <summary>
        /// Carriage Return ASCI Sign. It can be used to separate parameters in a command.
        /// </summary>
        public static byte ASCICodeCR = 0x0d;
        /// <summary>
        /// Line Feed.  It can be used to separate parameters in a command.
        /// </summary>
        public static byte ASCICodeLF = 0x0a;
    }
}
