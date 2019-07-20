using System.IO;
using System.Reflection;

namespace FiscalPrinterSimulatorService
{
    public class Constants
    {
        public const string MergedTranslationsFileName = "_translations.json";
        public const string CurrentTranslationsFileNamePattern = "translations-*.json";

        public static string TranslationsFileDirectoryPath = 
                   Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Translations");
    }
}
