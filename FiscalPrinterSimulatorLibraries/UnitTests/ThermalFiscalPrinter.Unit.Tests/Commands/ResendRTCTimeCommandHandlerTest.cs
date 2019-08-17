using System;
using System.Collections.Generic;
using FiscalPrinterSimulatorLibraries.Exceptions;
using NUnit.Framework;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;


namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class ResendRTCTimeCommandHandlerTest
    {

        [Test]
        public void When_RTC_Is_Not_Initialized_Should_Throw_Error()
        {
            //Arrange
            var state = new FiscalPrinterState();
            var command = new ThermalFiscalPrinterCommand(null, "#c", string.Empty);
            var commandHandler = new ResendRTCTimeCommandHandler(command);

            //Act
            //Assert
            Assert.Throws<FP_IllegalOperationException>(() =>
            {
                commandHandler.Handle(state);
            });
        }

        [Test]
        public void When_RTC_Is_Initialized_Should_Return_Output_Command()
        {
            //Arrange
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = 0 };
            var command = new ThermalFiscalPrinterCommand(null, "#c", string.Empty);
            var commandHandler = new ResendRTCTimeCommandHandler(command);
            var actualDateTime = DateTime.Now;
            var expectedOutputCommandParameters = CalculateOutputMethodParameters(actualDateTime);

            //Act
            var result = commandHandler.Handle(state);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.OutputCommand);
        }

        [Test]
        public void When_RTC_Is_Initialized_Should_Return_Correct_Output_Command()
        {
            //Arrange
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = 0 };
            var command = new ThermalFiscalPrinterCommand(null, "#c", string.Empty);
            var commandHandler = new ResendRTCTimeCommandHandler(command);
            var actualDateTime = DateTime.Now;
            var expectedOutputCommandParameters = CalculateOutputMethodParameters(actualDateTime);

            //Act
            var result = commandHandler.Handle(state);
            //Assert
            Assert.AreEqual(expectedOutputCommandParameters, result.OutputCommand.Parameters);
        }



        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(54)]
        [TestCase(199)]
        public void When_RTC_Is_Initialized_With_Positive_Time_Difference_Should_Return_Correct_Output_Command(int timeDifferenceInMinutes)
        {
            //Arrange
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = timeDifferenceInMinutes };
            var command = new ThermalFiscalPrinterCommand(null, "#c", string.Empty);
            var commandHandler = new ResendRTCTimeCommandHandler(command);
            var actualDateTime = DateTime.Now.AddMinutes(timeDifferenceInMinutes);
            var expectedOutputCommandParameters = CalculateOutputMethodParameters(actualDateTime);

            //Act
            var result = commandHandler.Handle(state);
            //Assert
            Assert.AreEqual(expectedOutputCommandParameters, result.OutputCommand.Parameters);
        }


        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-54)]
        [TestCase(-199)]
        public void When_RTC_Is_Initialized_With_Negative_Time_Difference_Should_Return_Correct_Output_Command(int timeDifferenceInMinutes)
        {
            //Arrange
            var state = new FiscalPrinterState() { TimeDiffrenceInMinutes = timeDifferenceInMinutes };
            var command = new ThermalFiscalPrinterCommand(null, "#c", string.Empty);
            var commandHandler = new ResendRTCTimeCommandHandler(command);
            var actualDateTime = DateTime.Now.AddMinutes(timeDifferenceInMinutes);
            var expectedOutputCommandParameters = CalculateOutputMethodParameters(actualDateTime);

            //Act
            var result = commandHandler.Handle(state);
            //Assert
            Assert.AreEqual(expectedOutputCommandParameters, result.OutputCommand.Parameters);
        }


        private string CalculateOutputMethodParameters(DateTime actualDateTime)
        {
            var actualDateTimeCollection = new List<int>
            {
                actualDateTime.Year,
                actualDateTime.Month,
                actualDateTime.Day,
                actualDateTime.Hour,
                actualDateTime.Minute,
                0
            };
            return string.Join(";", actualDateTimeCollection);
        }
    }
}
