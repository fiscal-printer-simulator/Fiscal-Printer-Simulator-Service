using System.IO.Ports;
using FiscalPrinterSimulatorService.Models;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class AvaliblePortsAction : BaseReduxAction
    {
        public AvaliblePortsAction()
        {
            this.type = ReduxActionType.RECEIVED_AVALIBLE_COM_PORTS.ToString();
            this.payload.Add("avalibleCOMPorts", SerialPort.GetPortNames());
        }
    }
}