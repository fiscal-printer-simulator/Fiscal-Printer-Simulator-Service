using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class ENQCommandHandlerTest
    {
        ThermalFiscalPrinterCommand _command;
        ENQCommandHandler _commandHandler;

        [SetUp]
        public void SetUpMethod()
        {
            _command = new ThermalFiscalPrinterCommand(null, null, null, null, null);
            _commandHandler = new ENQCommandHandler(_command);
        }


        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(false, false, false, false, (byte)0x60);
                yield return new TestCaseData(true, false, false, false, (byte)0x68);
                yield return new TestCaseData(false, true, false, false, (byte)0x64);
                yield return new TestCaseData(false, false, true, false, (byte)0x62);
                yield return new TestCaseData(false, false, false, true, (byte)0x61);
                yield return new TestCaseData(true, true, false, false, (byte)0x6c);
                yield return new TestCaseData(false, true, true, false, (byte)0x66);
                yield return new TestCaseData(false, false, true, true, (byte)0x63);
                yield return new TestCaseData(true, false, true, false, (byte)0x6a);
                yield return new TestCaseData(false, true, false, true, (byte)0x65);
                yield return new TestCaseData(true, false, false, true, (byte)0x69);
                yield return new TestCaseData(true, true, true, false, (byte)0x6e);
                yield return new TestCaseData(false, true, true, true, (byte)0x67);
                yield return new TestCaseData(true, true, true, true, (byte)0x6f);

            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void When_Pass_Fiscal_Printer_State_Should_Return_OutputCommand_With_One_Byte_Result(bool isInFiscalState, bool lastCommandSuccess, bool isInTransactionState, bool lastTransactionSuccess, byte expectedResult)
        {
            //Arrange
            var state = new FiscalPrinterState()
            {
                IsInFiscalState = isInFiscalState,
                LastCommandSuccess = lastCommandSuccess,
                IsInTransactionState = isInTransactionState,
                LastTransactionSuccess = lastTransactionSuccess
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
