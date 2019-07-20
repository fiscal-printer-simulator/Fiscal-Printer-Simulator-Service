using System.Collections.Generic;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class BaseReduxAction
    {
        public string type { get; set; }
        public Dictionary<string, object> payload { get; set; } = new Dictionary<string, object>();
    }
}