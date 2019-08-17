using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace FiscalPrinterSimulatorService
{
    [RunInstaller(true)]
    public partial class FiscalPrinterSimulatorServiceInstaller : System.Configuration.Install.Installer
    {
        public FiscalPrinterSimulatorServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
