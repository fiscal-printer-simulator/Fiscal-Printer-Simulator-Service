﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ThermalFiscalPrinterSimulatorLibraries.Commands;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class CANCommandHandlerTest
    {

        [Test]
        public void When_Pass_Any_Parameters_Should_Throw_TaskCanceledException()
        {
            //Assert
            var command = new ThermalFiscalPrinterCommand(null, null, null, string.Empty, string.Empty);
            CANCommandHandler canCommandHandler = new CANCommandHandler(command);

            //Act
            Action handleAction = () => canCommandHandler.Handle(null);

            //Assert

            handleAction.Should().Throw<TaskCanceledException>();


        }

    }
}
