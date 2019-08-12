using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThermalFiscalPrinterSimulatorLibraries;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class SetupClockCommandHandlerTest
    {
        FiscalPrinterState _state;

        [SetUp]
        public void SetupTests()
        {
            _state = new FiscalPrinterState()
            {

            };
        }



        [Test]
        [TestCase(-5, -5)]
        [TestCase(0, 0)]
        [TestCase(19, 19)]
        [TestCase(99, 99)]
        public void When_Pass_6_Parameters_From_DateTime_Now_Should_Calculate_Right_Diff_State_Value_And_Create_Output_Commands(int numberOfMinutesToAdd, int expectedValue)
        {
            //Arrange

            var dateTime = DateTime.Now.AddMinutes(numberOfMinutesToAdd);

            var dateArguments = new List<string>() {
                dateTime.Year.ToString().Substring(2,2),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString(),
                dateTime.Second.ToString()
            };

            var command = new ThermalFiscalPrinterCommand(dateArguments, "$c", null, null, null);
            var handler = new SetupClockCommandHandler(command);

            //Act
            _ = handler.Handle(_state);

            //Assert

            Assert.AreEqual(_state.TimeDiffrenceInMinutes, expectedValue);
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]

        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void When_Pass_Less_Then_6_Parameters_Should_Throw_FP_WrongNumberOfArgumentsException(int numberOfParameters)
        {
            //Arrange
            var dateArguments = Enumerable.Range(1, numberOfParameters).Select(m => m.ToString());

            var command = new ThermalFiscalPrinterCommand(dateArguments, "$c", null, null, null);
            var handler = new SetupClockCommandHandler(command);

            //Act
            //Assert
            Assert.Throws<FP_WrongNumberOfArgumentsException>(() =>
            {
                handler.Handle(_state);
            });
        }


        [Test]
        public void When_Pass_6_Parameters_But_One_No_Numeric_Should_Throw_FP_BadFormatOfArgumentException()
        {
            //Arrange
            var dateArguments = Enumerable.Range(1, 5).Select(m => m.ToString()).Append("6y");

            var command = new ThermalFiscalPrinterCommand(dateArguments, "$c", null, null, null);
            var handler = new SetupClockCommandHandler(command);

            //Act
            //Assert
            Assert.Throws<FP_BadFormatOfArgumentException>(() =>
            {
                handler.Handle(_state);
            });
        }


        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(60)]
        [TestCase(-60)]
        public void When_FP_is_In_Fiscal_State_And_Pass_6_Date_Arguments_With_Time_Plus_Minus_One_Hour_Should_Return_OK(int numberOfMinutesToAdd)
        {
            //Arrange

            var dateTime = DateTime.Now.AddMinutes(numberOfMinutesToAdd);

            var dateArguments = new List<string>() {
                dateTime.Year.ToString().Substring(2,2),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString(),
                dateTime.Second.ToString()
            };

            _state.IsInFiscalState = true;

            var command = new ThermalFiscalPrinterCommand(dateArguments, "$c", null, null, null);
            var handler = new SetupClockCommandHandler(command);

            //Act
            _ = handler.Handle(_state);

            //Assert

            Assert.AreEqual(_state.TimeDiffrenceInMinutes, numberOfMinutesToAdd);
        }


        [TestCase(61)]
        [TestCase(-61)]
        public void When_FP_is_In_Fiscal_State_And_Pass_6_Date_Arguments_With_Time_Plus_Minus__More_Then_One_Hour_Should_Throw_FP(int numberOfMinutesToAdd)
        {
            //Arrange

            var dateTime = DateTime.Now.AddMinutes(numberOfMinutesToAdd);

            var dateArguments = new List<string>() {
                dateTime.Year.ToString().Substring(2,2),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString(),
                dateTime.Second.ToString()
            };

            _state.IsInFiscalState = true;

            var command = new ThermalFiscalPrinterCommand(dateArguments, "$c", null, null, null);
            var handler = new SetupClockCommandHandler(command);

            //Act

            //Assert
            Assert.Throws<FP_IllegalOperationException>(() =>
            {
                _ = handler.Handle(_state);
            });
        }


        [Test]
        public void When_Pass_Right_Command_With_Correct_Arguments_Should_Create_Correct_PrintOut()
        {

            //Arrange

            var newDateTime = DateTime.Now.AddMinutes(5);
            var actualFPDateTime = _state.TimeDiffrenceInMinutes != int.MinValue ? DateTime.Now.AddMinutes(_state.TimeDiffrenceInMinutes) : DateTime.Now;

            var dateArguments = new List<string>() {
                newDateTime.Year.ToString().Substring(2,2),
                newDateTime.Month.ToString(),
                newDateTime.Day.ToString(),
                newDateTime.Hour.ToString(),
                newDateTime.Minute.ToString(),
                newDateTime.Second.ToString()
            };

            _state.IsInFiscalState = true;

            var command = new ThermalFiscalPrinterCommand(dateArguments, "$c", null, null, null);
            var handler = new SetupClockCommandHandler(command);

            StringBuilder reciptBody = new StringBuilder();
            reciptBody.AppendLine();
            reciptBody.AppendLine("PROGRAMOWANIE ZEGARA".PadCenter(Constants.ReciptWidth));
            reciptBody.AppendLine();
            reciptBody.AppendLine($"Zegar przed zmianą:     {actualFPDateTime.ToString("yyyy-MM-dd,HH:mm")}");
            reciptBody.AppendLine($"Zegar po zmianie:       {newDateTime.ToString("yyyy-MM-dd,HH:mm")}");
            reciptBody.AppendLine();

            var expectedReciptBody = reciptBody.ToString();

            //Act
            var response = handler.Handle(_state);
            //Assert

            Assert.AreEqual(expectedReciptBody, response.OutputReciptBuffer);
        }
    }
}
