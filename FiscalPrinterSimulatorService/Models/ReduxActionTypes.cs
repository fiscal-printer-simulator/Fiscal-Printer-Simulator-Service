﻿namespace FiscalPrinterSimulatorService.Models
{
    public enum ReduxActionType
    {
        UNKNOWN = 0,

        RECEIVED_AVALIBLE_COM_PORTS,

        CONNECT_TO_COM_PORT,
        CONNECT_TO_COM_PORT_SUCCESS,
        CONNECT_TO_COM_PORT_FAILED,

        DISCONNECT_FROM_COM,
        DISCONNECT_FROM_COM_SUCCESS,
        DISCONNECT_FROM_COM_FAILED,

        RECEIVE_RECIPT_DATA,
        ACTUAL_FISCAL_PRINTER_SIMULATOR_STATE,
        ACTUAL_TRANSLATION_FOR_SIMULATOR_CLIENT
    }
}
