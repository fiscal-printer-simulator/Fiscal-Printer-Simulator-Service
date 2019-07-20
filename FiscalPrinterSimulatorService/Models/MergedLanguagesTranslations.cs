using System.Collections.Generic;

namespace FiscalPrinterSimulatorService.Models
{
    public class MergedLanguagesTranslations
    {
        public Dictionary<string, IEnumerable<string>> languages { get; set; }
        public Dictionary<string, IEnumerable<string>> translation { get; set; }
    }
}
