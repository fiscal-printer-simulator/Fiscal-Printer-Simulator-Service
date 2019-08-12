using System;
using System.Collections.Generic;
using System.Text;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Models;
using NUnit.Framework;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class SendLineToClientDisplayCommandHandlerTest
    {


        [Test]
        public void When_Passed_Command_Without_Arguments_Should_Throw_Exception()
        {
            //Arrange
            var fiscalPrinterState = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(new List<string> { }, "$d", string.Empty);
            var commandHandler = new SendLineToClientDisplayCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_WrongNumberOfArgumentsException>(() =>
            {
                commandHandler.Handle(fiscalPrinterState);
            });
        }


        [Test]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(103)]
        [TestCase(500)]
        public void When_Passed_Command_With_Bad_First_Argument_Should_Throw_Exception(int firstCommandArgument)
        {
            //Arrange
            var fiscalPrinterState = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(new List<string> { firstCommandArgument.ToString() }, "$d", string.Empty);
            var commandHandler = new SendLineToClientDisplayCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_BadFormatOfArgumentException>(() =>
            {
                commandHandler.Handle(fiscalPrinterState);
            });
        }


        [Test]
        [TestCase(101)]
        [TestCase(102)]
        public void When_Passed_Command_With_Parameter_101_Or_102_But_Text_To_Display_Has_Over_20_Characters_Then_Should_Throw_Error(int firstCommandArgument)
        {
            //Arrange
            var passedTooLongText = "".PadRight(21,'x');
            var fiscalPrinterState = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(new List<string> { firstCommandArgument.ToString() }, "$d", passedTooLongText);
            var commandHandler = new SendLineToClientDisplayCommandHandler(command);
            //Act
            //Assert
            Assert.Throws<FP_BadFormatOfArgumentException>(() =>
            {
                commandHandler.Handle(fiscalPrinterState);
            });
        }

        [Test]
        public void When_Passed_Command_With_Argument_101_And_Paramer_Has_Less_Then_20_Characters_Should_Return_Correct_Output_Line_Display_Result()
        {
            //Arrange
            var passedParameterText = "".PadRight(20, 'x');
            var expectedResult = new ClientLineDisplayOutput() { LineNumber = 0 , OutputText = passedParameterText };
            var fiscalPrinterState = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(new List<string> { "101" }, "$d", passedParameterText);
            var commandHandler = new SendLineToClientDisplayCommandHandler(command);
            //Act
            var result = commandHandler.Handle(fiscalPrinterState);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.LineNumber, result.ClientLineDisplayOutputLine.LineNumber);
            Assert.AreEqual(expectedResult.OutputText, result.ClientLineDisplayOutputLine.OutputText);
        }

        [Test]
        public void When_Passed_Command_With_Argument_102_And_Paramer_Has_Less_Then_20_Characters_Should_Return_Correct_Output_Line_Display_Result()
        {
            //Arrange
            var passedParameterText = "".PadRight(20, 'x');
            var expectedResult = new ClientLineDisplayOutput() { LineNumber = 1, OutputText = passedParameterText };
            var fiscalPrinterState = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(new List<string> { "102" }, "$d", passedParameterText);
            var commandHandler = new SendLineToClientDisplayCommandHandler(command);
            //Act
            var result = commandHandler.Handle(fiscalPrinterState);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.LineNumber, result.ClientLineDisplayOutputLine.LineNumber);
            Assert.AreEqual(expectedResult.OutputText, result.ClientLineDisplayOutputLine.OutputText);
        }

    }
}
