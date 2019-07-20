using FiscalPrinterSimulatorLibraries.Commands;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ThermalFiscalPrinter.Unit.Tests.Commands.Thermal
{
    [TestFixture]
    public class CANCommandHandlerTest
    {

        [Test]
        public void When_Pass_Any_Parameters_Should_Throw_TaskCanceledException()
        {
            //Assert
            var command = new FiscalPrinterCommand(null, null, null, string.Empty, string.Empty);
            CANCommandHandler canCommandHandler = new CANCommandHandler(command);

            //Act
            Action handleAction = () => canCommandHandler.Handle(null);

            //Assert

            handleAction.Should().Throw<TaskCanceledException>();


        }

    }
}
