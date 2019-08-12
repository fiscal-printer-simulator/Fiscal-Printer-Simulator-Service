using FiscalPrinterSimulatorLibraries.Models;
using FiscalPrinterSimulatorService.Models;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class SendClientLineDisplayOutputLine : BaseReduxAction
    {
        public SendClientLineDisplayOutputLine(ClientLineDisplayOutput clientLineDisplay)
        {
            switch (clientLineDisplay.LineNumber)
            {
                case 0:
                    this.type = ReduxActionType.RECEIVE_LINE_DISPLAY_FIRST_LINE_DATA.ToString();
                    break;
                case 1:
                    this.type = ReduxActionType.RECEIVE_LINE_DISPLAY_SECOND_LINE_DATA.ToString();
                    break;
                default:
                    this.type = ReduxActionType.UNKNOWN.ToString();
                    break;
            }

            this.payload.Add("lineDisplayOutputLineText", clientLineDisplay.OutputText);
        }
    }
}