using FiscalPrinterSimulatorLibraries.Commands;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class DLECommandHandlerTest
    {
        ThermalFiscalPrinterCommand _command;
        DLECommandHandler _commandHandler;
        [SetUp]
        public void SetUpMethod()
        {
            _command = new ThermalFiscalPrinterCommand(null, null, null, null, null);
            _commandHandler = new DLECommandHandler(_command);
        }


        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(false, false, false, (byte)0x70);
                yield return new TestCaseData(true, false, false, (byte)0x74);
                yield return new TestCaseData(false, true, false, (byte)0x72);
                yield return new TestCaseData(false, false, true, (byte)0x71);
                yield return new TestCaseData(true, true, false, (byte)0x76);
                yield return new TestCaseData(false, true, true, (byte)0x73);
                yield return new TestCaseData(true, false, true, (byte)0x75);
                yield return new TestCaseData(true, true, true, (byte)0x77);
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void When_Pass_Fiscal_Printer_State_Should_Return_OutputCommand_With_One_Byte_Result(bool isInOnlineMode, bool IsInOutOfPaperMode, bool isInInternalErrorMode, byte expectedResult)
        {
            //Arrange
            var state = new FiscalPrinterState()
            {
                IsInOnlineMode = isInOnlineMode,
                IsInOutOfPaperState = IsInOutOfPaperMode,
                IsInInternalErrorState = isInInternalErrorMode
            };
            //Act
            var result = _commandHandler.Handle(state);

            //Assert

            var expected = new List<byte> { expectedResult };

            Assert.IsNotNull(result.OutputCommand);
            Assert.AreEqual(result.OutputReciptBuffer, string.Empty);
            Assert.AreEqual(expected, result.OutputCommand.ToBytesArray());

        }

        [Test]
        public void When_Pass_Fiscal_Printer_State_As_Null_Should_Throws_NullReferenceException()
        {
            //Arrange
            FiscalPrinterState state = null;
            //Act

            //Assert

            Assert.Throws<System.NullReferenceException>(() =>
            {
                _commandHandler.Handle(state);
            });
        }
    }
}
