using System.IO.Ports;
using FiscalPrinterSimulatorService.Models;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class AvailablePortsAction : BaseReduxAction
    {
        public AvailablePortsAction()
        {
            this.type = ReduxActionType.RECEIVED_AVAILABLE_COM_PORTS.ToString();
            this.payload.Add("availableCOMPorts", SerialPort.GetPortNames());
        }
    }
}