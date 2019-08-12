using FiscalPrinterSimulatorLibraries.Commands;
using FiscalPrinterSimulatorLibraries.Exceptions;
using FiscalPrinterSimulatorLibraries.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using ThermalFiscalPrinterSimulatorLibraries;
using ThermalFiscalPrinterSimulatorLibraries.Commands;
using ThermalFiscalPrinterSimulatorLibraries.Models;
using static ThermalFiscalPrinterSimulatorLibraries.Models.PTUTypes;

namespace ThermalFiscalPrinter.Unit.Tests.Commands
{
    [TestFixture]
    public class ChangeInPTURatesCommandHandlerTest
    {

        [Test]
        public void When_Arguments_Not_Passed_Should_Throw_FP_WrongNumberOfArgumentsException()
        {
            //Arrange
            ThermalFiscalPrinterCommand command = new ThermalFiscalPrinterCommand(null, null, null, null, null);
            ChangePTURatesCommandHandler handler = new ChangePTURatesCommandHandler(command);
            //Act
            //Assert

            Assert.Throws<FP_WrongNumberOfArgumentsException>(() =>
            {
                handler.Handle(null);
            });
        }


        [Test]
        public void When_First_Argument_Is_Not_Numeric_Should_Throw_FP_BadFormatOfArgumentException()
        {
            //Arrange
            FiscalPrinterState state = new FiscalPrinterState();
            var PnArgs = new List<string>() { "d3" };
            ThermalFiscalPrinterCommand command = new ThermalFiscalPrinterCommand(PnArgs, null, null, null, null);
            ChangePTURatesCommandHandler handler = new ChangePTURatesCommandHandler(command);
            //Act
            //Assert

            Assert.Throws<FP_BadFormatOfArgumentException>(() =>
            {
                handler.Handle(null);
            });
        }

        [Test]
        public void When_First_Argument_Is_More_Then_7_Should_Throw_FP_IllegalOperationException()
        {
            //Arrange
            FiscalPrinterState state = new FiscalPrinterState();
            var PnArgs = new List<string>() { "8" };
            ThermalFiscalPrinterCommand command = new ThermalFiscalPrinterCommand(PnArgs, null, null, null, null);
            ChangePTURatesCommandHandler handler = new ChangePTURatesCommandHandler(command);
            //Act
            //Assert

            Assert.Throws<FP_IllegalOperationException>(() =>
            {
                handler.Handle(state);
            });
        }



        [Test]
        public void When_First_Argument_Not_Equals_All_Argument_Count_Without_First_Should_Throw_()
        {
            //Arrange
            FiscalPrinterState state = new FiscalPrinterState();
            System.Console.WriteLine(state.PTURates[(int)PTU.A]);
            var PnArgs = new List<string>() { "3" };
            ThermalFiscalPrinterCommand command = new ThermalFiscalPrinterCommand(PnArgs, null, "3/4", null, null);
            ChangePTURatesCommandHandler handler = new ChangePTURatesCommandHandler(command);
            //Act
            //Assert

            Assert.Throws<FP_WrongNumberOfArgumentsException>(() =>
            {
                handler.Handle(state);
            });
        }


        [Test]
        public void When_Pass_4_Ptu_Rates_Last_3_Should_Be_Inactive()
        {
            //Arrange
            FiscalPrinterState state = new FiscalPrinterState();
            var PnArgs = new List<string>() { "4" };
            ThermalFiscalPrinterCommand command = new ThermalFiscalPrinterCommand(PnArgs, null, "23/7/3/100", null, null);
            ChangePTURatesCommandHandler handler = new ChangePTURatesCommandHandler(command);
            //Act
            _ = handler.Handle(state);

            //Assert
            Assert.AreEqual(23.00, state.PTURates[(int)PTU.A].ActualPercentageValue);
            Assert.AreEqual(7.00, state.PTURates[(int)PTU.B].ActualPercentageValue);
            Assert.AreEqual(3.00, state.PTURates[(int)PTU.C].ActualPercentageValue);
            Assert.AreEqual(Constants.PTUTaxFreeRate, state.PTURates[(int)PTU.D].ActualPercentageValue);
            Assert.AreEqual(Constants.PTUInactiveRate, state.PTURates[(int)PTU.E].ActualPercentageValue);
            Assert.AreEqual(Constants.PTUInactiveRate, state.PTURates[(int)PTU.F].ActualPercentageValue);
            Assert.AreEqual(Constants.PTUTaxFreeRate, state.PTURates[(int)PTU.G].ActualPercentageValue);
        }


        [Test]
        public void When_Pass_6_Right_Parameters_Should_Return_Correct()
        {
            //Arrange
            FiscalPrinterState state = new FiscalPrinterState();
            var PnArgs = new List<string>() { "5" };
            ThermalFiscalPrinterCommand command = new ThermalFiscalPrinterCommand(PnArgs, null, "23/7/6/4,5/2", null, null);
            ChangePTURatesCommandHandler handler = new ChangePTURatesCommandHandler(command);
            //Act
            var result = handler.Handle(state);

            //Assert
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("N I E F I S K A L N Y".PadCenter(Constants.ReciptWidth));
            stringBuilder.AppendLine("Z m i a n a  s t a w e k  P T U".PadCenter(Constants.ReciptWidth));
            stringBuilder.AppendLine("Stare PTU:".PadRight(Constants.ReciptWidth));
            stringBuilder.AppendLine("PTU A".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU B".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU C".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU D".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU E".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU F".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU G".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("---------------------------------------------------------------".Substring(0, Constants.ReciptWidth));
            stringBuilder.AppendLine("Nowe PTU:".PadRight(Constants.ReciptWidth));
            stringBuilder.AppendLine("PTU A".PadRight(Constants.ReciptWidth - 7) + "23,00 %");
            stringBuilder.AppendLine("PTU B".PadRight(Constants.ReciptWidth - 6) + "7,00 %");
            stringBuilder.AppendLine("PTU C".PadRight(Constants.ReciptWidth - 6) + "6,00 %");
            stringBuilder.AppendLine("PTU D".PadRight(Constants.ReciptWidth - 6) + "4,50 %");
            stringBuilder.AppendLine("PTU E".PadRight(Constants.ReciptWidth - 6) + "2,00 %");
            stringBuilder.AppendLine("PTU F".PadRight(Constants.ReciptWidth - 3) + "---");
            stringBuilder.AppendLine("PTU G".PadRight(Constants.ReciptWidth - 9) + "SP.ZW.PTU");
            stringBuilder.AppendLine("N I E F I S K A L N Y".PadCenter(Constants.ReciptWidth));

            System.Console.WriteLine(result.OutputReciptBuffer);
            System.Console.WriteLine(stringBuilder.ToString());
            Assert.AreEqual(stringBuilder.ToString(), result.OutputReciptBuffer);
        }

    }
}
