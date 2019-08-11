using System;
using System.Text;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using NUnit.Framework;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class CasherLoginCommandHandlerTest
    {
        [Test]
        public void When_RTC_Clock_Not_Initialize_Should_Throw_Error()
        {
            //Arrange
            var state = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(null, "#p", null, string.Empty, string.Empty);
            var handler = new CashierLoginCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_IllegalOperationException>(() =>
            {
                handler.Handle(state);
            });
        }

        [Test]
        public void When_Pass_Command_Without_Parameters_Should_Throw_Error()
        {
            //Arrange
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = 0 };
            var command = new ThermalFiscalPrinterCommand(null, "#p", string.Empty, string.Empty, string.Empty);
            var handler = new CashierLoginCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_WrongNumberOfArgumentsException>(() =>
            {
                handler.Handle(state);
            });


        }



        [Test]
        public void When_Pass_Cashier_Login_With_More_Then_32_Characters_Should_Throw_Error()
        {
            //Arrange
            var crChar = (char)ThermalFiscalPrinterSimulatorLibraries.Constants.ASCICodeCR;
            var cashierLogin = "a2f".PadRight(33);
            var printerCode = "333s333s";
            var passedParameters = $"{cashierLogin}{crChar}{printerCode}{crChar}";
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = 0 };
            var command = new ThermalFiscalPrinterCommand(null, "#p", passedParameters, string.Empty, string.Empty);
            var handler = new CashierLoginCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_BadFormatOfArgumentException>(() =>
            {
                handler.Handle(state);
            });
        }

        [Test]
        public void When_Pass_Printer_Code_With_More_Then_8_Characters_Should_Throw_Error()
        {
            //Arrange
            var crChar = (char)ThermalFiscalPrinterSimulatorLibraries.Constants.ASCICodeCR;
            var cashierLogin = "test3";
            var printerCode = "as3".PadRight(9);
            var passedParameters = $"{cashierLogin}{crChar}{printerCode}{crChar}";
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = 0 };
            var command = new ThermalFiscalPrinterCommand(null, "#p", passedParameters, string.Empty, string.Empty);
            var handler = new CashierLoginCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_BadFormatOfArgumentException>(() =>
            {
                handler.Handle(state);
            });
        }

        [Test]
        public void When_Pass_Command_With_Right_Parameters_Shoudl_Return_Correct_Printout()
        {
            //Arrange
            var crChar = (char)ThermalFiscalPrinterSimulatorLibraries.Constants.ASCICodeCR;
            var cashierLogin = "test99";
            var printerCode = "993A";
            var passedParameters = $"{cashierLogin}{crChar}{printerCode}{crChar}";
            var state = new FiscalPrinterState()
            {
                TimeDiffrenceInMinutes = 0,
                FiscalPrinterHeader = "HEADER TEST".PadCenter(ThermalFiscalPrinterSimulatorLibraries.Constants.ReciptWidth)
            };
            var command = new ThermalFiscalPrinterCommand(null, "#p", passedParameters, string.Empty, string.Empty);
            var handler = new CashierLoginCommandHandler(command);
            //Act

            var result = handler.Handle(state);
            var dateToString = DateTime.Now.ToString("yyyy-MM-dd");
            var timeToString = DateTime.Now.ToString("HH-mm-ss");
            //Assert
            StringBuilder expectedPrintout = new StringBuilder();

            expectedPrintout.AppendLine("              HEADER TEST               ");
            expectedPrintout.AppendLine($"{dateToString}                             1");
            expectedPrintout.AppendLine("         N I E F I S K A L N Y          ");
            expectedPrintout.AppendLine("Rozpoczęcie pracy kasjera");
            expectedPrintout.AppendLine($"Kasjer                            {cashierLogin}");
            expectedPrintout.AppendLine($"Numer kasy                          {printerCode}");
            expectedPrintout.AppendLine();
            expectedPrintout.AppendLine("         N I E F I S K A L N Y          ");
            expectedPrintout.AppendLine($"    #{printerCode}     {cashierLogin}            {timeToString}");
            expectedPrintout.AppendLine("                12345678                ");
            expectedPrintout.AppendLine();
            Assert.AreEqual(expectedPrintout.ToString(), result.OutputReciptBuffer);
        }



    }
}
