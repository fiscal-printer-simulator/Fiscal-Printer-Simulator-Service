using System;
using System.Collections.Generic;
using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinterSimulatorLibraries.Commands
{
    /// <summary>
    /// Command handler for command LBSENDCK
    /// </summary>
    public class ResendRTCTimeCommandHandler : BaseCommandHandler
    {
        public ResendRTCTimeCommandHandler(BaseFiscalPrinterCommand command) : base(command)
        {
        }

        public override CommandHandlerResponse Handle(IFiscalPrinterState fiscalPrinterState)
        {
            var state = fiscalPrinterState as FiscalPrinterState;

            if (state.TimeDiffrenceInMinutes == int.MinValue)
            {
                throw new FiscalPrinterSimulatorLibraries.Exceptions.FP_IllegalOperationException("RTC not initialized.");
            }
            var actualRTCTime = DateTime.Now.AddMinutes(state.TimeDiffrenceInMinutes);
            var arrayOfRTCParameters = new List<int> {
                actualRTCTime.Year,
                actualRTCTime.Month,
                actualRTCTime.Day,
                actualRTCTime.Hour,
                actualRTCTime.Minute,
                0
            };

            var commandParameters = string.Join(';', arrayOfRTCParameters);
            var outputCommand = new ThermalFiscalPrinterCommand(new string[] { "1" }, "#C", commandParameters);

            return new CommandHandlerResponse(outputCommand);
        }
    }
}
