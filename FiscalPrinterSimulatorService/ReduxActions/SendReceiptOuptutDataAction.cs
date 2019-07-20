using FiscalPrinterSimulatorService.Models;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class SendReceiptOuptutDataAction : BaseReduxAction
    {
        public SendReceiptOuptutDataAction(string receiptText)
        {
            this.type = ReduxActionType.RECEIVE_RECIPT_DATA.ToString();
            this.payload.Add("receiptText", receiptText);
        }
    }
}