using FiscalPrinterSimulatorService.Models;
using System.IO.Ports;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class ActualServerStateAction : BaseReduxAction
    {
        public ActualServerStateAction(SerialPort serialPort)
        {
            this.type = ReduxActionType.ACTUAL_FISCAL_PRINTER_SIMULATOR_STATE.ToString();
            this.payload.Add("ConnectedToCom", serialPort.IsOpen);
            this.payload.Add("ConnectedPortName", serialPort.IsOpen ? serialPort.PortName : "");
        }
    }
}