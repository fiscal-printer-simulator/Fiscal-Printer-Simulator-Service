using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThermalFiscalPrinterSimulatorLibraries;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands.Thermal
{
    [TestFixture]
    public class SetupReciptHeaderCommandHandlerTest
    {

        [Test]
        public void When_Passed_Recipt_Header_Without_EndHeaderSign_Should_Throw_FP_IllegalOperationException()
        {
            //Arrange
            var reciptHeader = string.Join("", Enumerable.Repeat("A", 500));

            var state = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(null, null, reciptHeader);
            var commandHandler = new SetupReciptHeaderCommandHandler(command);

            //Act
            //Assert

            Assert.Throws<FP_IllegalOperationException>(() =>
            {
                commandHandler.Handle(state);
            });

        }


        [Test]
        public void When_Passed_Header_Has_More_Then_500_Characters_Should_Throw_FP_Illegal_Exception()
        {
            //Arrange
            var reciptHeader = string.Join("", Enumerable.Repeat("A", 501));

            var state = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(null, null, reciptHeader + "?");
            var commandHandler = new SetupReciptHeaderCommandHandler(command);

            //Act
            //Assert

            Assert.Throws<FP_IllegalOperationException>(() =>
            {
                commandHandler.Handle(state);
            });

        }

        [Test]
        public void When_Passed_Correct_Less_Then_500_Chars_And_Lines_Split_By_CR_Header_Should_Save_It_In_Fiscal_Printer_State()
        {
            //Arrange

            var reciptHeaderRows = new List<string>
            {
                $"Sample Fiscal Printer Header",
                "Lines must be split by CR",
                "Or LF Characters"
            };

            var reciptHeaderInput = reciptHeaderRows[0] + Convert.ToChar(Constants.ASCICodeCR)
                                  + reciptHeaderRows[1] + Convert.ToChar(Constants.ASCICodeLF)
                                  + reciptHeaderRows[2];

            var state = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(null, null, reciptHeaderInput + "?");
            var commandHandler = new SetupReciptHeaderCommandHandler(command);

            //Act
            var result = commandHandler.Handle(state);
            //Assert
            Assert.IsNull(result.OutputCommand);
            Assert.AreEqual(result.OutputReciptBuffer, string.Empty);

            StringBuilder stringBuilder = new StringBuilder();
            reciptHeaderRows.ForEach(row => stringBuilder.AppendLine(row));

            var expectedReciptHeader = stringBuilder.ToString();

            Console.WriteLine(expectedReciptHeader);
            Console.WriteLine(reciptHeaderInput);

            Assert.AreEqual(expectedReciptHeader, state.FiscalPrinterHeader);
        }

    }
}
