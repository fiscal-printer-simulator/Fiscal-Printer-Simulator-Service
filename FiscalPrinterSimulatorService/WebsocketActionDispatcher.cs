using Fleck;
using System;
using Newtonsoft.Json;
using FiscalPrinterSimulatorService.Models;
using FiscalPrinterSimulatorService.ReduxActions;
using System.Collections.Generic;
using System.IO.Ports;

namespace FiscalPrinterSimulatorService
{
    public static class WebsocketActionDispatcher
    {
        public static void SendMessage(this IWebSocketConnection connection, BaseReduxAction action) => connection.Send(JsonConvert.SerializeObject(action));

        public static void Dispatch(this List<IWebSocketConnection> connections, string message, SerialPort serialPort)
        {
            List<BaseReduxAction> actionsToSend = new List<BaseReduxAction>();

            var baseAction = JsonConvert.DeserializeObject<BaseReduxAction>(message);
            if (!Enum.TryParse(baseAction.type, out ReduxActionType actionType))
            {
                throw new Exception("Bad action format type");
            }

            switch (actionType)
            {
                case ReduxActionType.UNKNOWN:
                    break;
                case ReduxActionType.RECEIVED_AVALIBLE_COM_PORTS:
                    break;
                case ReduxActionType.CONNECT_TO_COM_PORT:
                    if (!baseAction.payload.TryGetValue("portName", out object portName))
                    {
                        throw new ArgumentNullException("portName", "Action Payload not contain required parameter");
                    }
                    try
                    {
                        serialPort.PortName = portName.ToString();
                        serialPort.Open();

                        actionsToSend.Add(new BaseReduxAction() { type = ReduxActionType.CONNECT_TO_COM_PORT_SUCCESS.ToString() });
                    }
                    catch
                    {
                        actionsToSend.Add(new BaseReduxAction() { type = ReduxActionType.CONNECT_TO_COM_PORT_FAILED.ToString() });
                    }
                    break;

                case ReduxActionType.DISCONNECT_FROM_COM:
                    try
                    {
                        serialPort.Close();
                        actionsToSend.Add(new BaseReduxAction() { type = ReduxActionType.DISCONNECT_FROM_COM_SUCCESS.ToString() });
                    }
                    catch
                    {
                        actionsToSend.Add(new BaseReduxAction() { type = ReduxActionType.DISCONNECT_FROM_COM_FAILED.ToString() });
                    }
                    break;

                case ReduxActionType.RECEIVE_RECIPT_DATA:
                    break;
                default:
                    break;
            }
            actionsToSend.ForEach(action => connections.ForEach(connection => connection.SendMessage(action)));

        }

    }
}