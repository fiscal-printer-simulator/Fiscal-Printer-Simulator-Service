using FiscalPrinterSimulatorLibraries.Commands;
using NUnit.Framework;
using System.Text;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands.Thermal
{
    [TestFixture]
    public class ReadingHeaderCommandHandlerTest
    {

        [Test]
        public void When_Passed_Read_Header_Command_Should_Return_HeaderFromState_And_Argument_1()
        {

            //Arrange
            StringBuilder fiscalPrinterHeader = new StringBuilder();
            fiscalPrinterHeader.AppendLine();
            fiscalPrinterHeader.AppendLine("Lorem ipsum dolor sit amet.");
            fiscalPrinterHeader.AppendLine("---------------------------");
            fiscalPrinterHeader.AppendLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque risus.");
            fiscalPrinterHeader.AppendLine("                     XX XX XX                           ");


            var command = new ThermalFiscalPrinterCommand(null, "#u", null);
            var commandHandler = new ReadingHeaderCommandHandler(command);
            var fiscalPrinterState = new FiscalPrinterState()
            {
                FiscalPrinterHeader = fiscalPrinterHeader.ToString()
            };

            //Act
            var result = commandHandler.Handle(fiscalPrinterState);
            //Assert

            var responseCommandString = result.OutputCommand.ToString();

            var indexOfCommandName = responseCommandString.IndexOf("#U");
            var recivedFiscalPrinterHeader = responseCommandString.Substring(indexOfCommandName + 2, responseCommandString.Length - (indexOfCommandName + 6));

            var commandArguments = responseCommandString.Substring(0, indexOfCommandName).Substring(3).Split(';');

            Assert.AreEqual(fiscalPrinterHeader.ToString(), recivedFiscalPrinterHeader);
            Assert.AreEqual(1, commandArguments.Length);
            Assert.AreEqual("1", commandArguments[0]);
        }



        [Test]
        public void When_Passed_Read_Header_Is_Null_Command_Should_Return_HeaderFromState_And_Argument_1()
        {

            //Arrange



            var command = new ThermalFiscalPrinterCommand(null, "#u", null);
            var commandHandler = new ReadingHeaderCommandHandler(command);
            var fiscalPrinterState = new FiscalPrinterState()
            {
                FiscalPrinterHeader = null
            };

            //Act
            var result = commandHandler.Handle(fiscalPrinterState);
            //Assert

            var responseCommandString = result.OutputCommand.ToString();

            var indexOfCommandName = responseCommandString.IndexOf("#U");
            var recivedFiscalPrinterHeader = responseCommandString.Substring(indexOfCommandName + 2, responseCommandString.Length - (indexOfCommandName + 6));

            var commandArguments = responseCommandString.Substring(0, indexOfCommandName).Substring(3).Split(';');

            Assert.AreEqual(string.Empty, recivedFiscalPrinterHeader);
            Assert.AreEqual(1, commandArguments.Length);
            Assert.AreEqual("1", commandArguments[0]);
        }

    }
}
