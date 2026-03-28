using FiscalPrinterSimulatorService.Models;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class SendReceiptOutputDataAction : BaseReduxAction
    {
        public SendReceiptOutputDataAction(string receiptText)
        {
            this.type = ReduxActionType.RECEIVE_RECEIPT_DATA.ToString();
            this.payload.Add("receiptText", receiptText);
        }
    }
}