using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Models;
using FluentAssertions;
using NUnit.Framework;

namespace ThermalFiscalPrinter.Unit.Tests.Commands.Thermal
{
    [TestFixture]
    public class BELCommandHandlerTests
    {
        [Test]
        public void When_Pass_Any_Parameters_Should_Return_Empty_Object_And_Mark_Fiscal_PrinterLastCommand_As_True()
        {
            //Arrange
            var fiscalPrinterState = new FiscalPrinterState();
            var command = new FiscalPrinterCommand(null, "BEL", null, string.Empty, string.Empty);
            var _bELCommandHandler = new BELCommandHandler(command);
            //Act
            var result = _bELCommandHandler.Handle(fiscalPrinterState);
            //Assert
            result.Should().NotBeNull();
            fiscalPrinterState.LastCommandSuccess.Should().BeTrue();
        }

        [Test]
        public void When_Pass_Null_As_Fiscal_Printer_State_Should_Return_Empty_Object()
        {
            //Arrange 
            FiscalPrinterState fiscalPrinterState = null;
            var command = new FiscalPrinterCommand(null, "BEL", null, string.Empty, string.Empty);
            var _bELCommandHandler = new BELCommandHandler(command);

            //Act
            var result = _bELCommandHandler.Handle(null);

            //Assert
            result.Should().NotBeNull();
            fiscalPrinterState.Should().BeNull();
        }


    }
}
