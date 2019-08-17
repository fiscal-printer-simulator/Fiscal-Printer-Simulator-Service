using System.IO;
using System.Reflection;

namespace FiscalPrinterSimulatorService
{
    public class Constants
    {
        public const string CurrentTranslationsFileNamePattern = "translations-*.json";

        private static string _rootPath = 
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string TranslationsFileDirectoryPath = Path.Combine(_rootPath, "Translations");
        public static string PluginsAssemblyDirectoryPath = Path.Combine(_rootPath, "Plugins");
    }
}
