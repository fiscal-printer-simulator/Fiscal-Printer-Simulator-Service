# Fiscal Printer Simulator Service
[![Build Status](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service/actions/workflows/ci.yml)

Windows service that emulates a real fiscal printer device over a serial (COM) port.  
It allows POS developers to test fiscal-printer integration without access to physical hardware.

The service exposes a **WebSocket API** (port `8181` by default) so the companion  
[Fiscal Printer Simulator Client](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Client)
can connect and display live receipt printouts, line-display messages and drawer events.

## Architecture

```
POS Application
    │  (serial / COM port)
    ▼
FiscalPrinterSimulatorService  (.NET Framework 4.7.2 · Windows Service)
    │  plugin loader
    ├─► ThermalFiscalPrinterSimulatorLibraries  (Posnet Thermal protocol)
    │       └─ CommandHandlerFactory → per-command handlers
    │
    │  WebSocket (Fleck · port 8181)
    ▼
Fiscal Printer Simulator Client  (JavaScript / React)
```

The service uses a **plugin architecture**: protocol implementations are discovered at runtime as
`*FiscalPrinterSimulatorLibraries.dll` files dropped next to the executable. Adding a new protocol
requires only a new class library that implements `IFiscalPrinter`.

## Getting Started

### Prerequisites

* .NET Framework 4.7.2 (or the full .NET Framework SDK that includes it)
* Windows (required for `System.ServiceProcess` and `System.IO.Ports`)
* [Fiscal Printer Simulator Client](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Client) *(optional – to visualise running commands)*
* [Fiscal Printer Diagnostic Tool](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Diagnostic-Tool) *(optional – to send test commands)*

### Build & Run

```bash
# restore NuGet packages
dotnet restore

# build (Debug / x64)
dotnet build -c Debug -p:Platform=x64

# run as a console app (useful during development)
dotnet run --project FiscalPrinterSimulatorService
```

The WebSocket port can be overridden via the `FP_SERVICE_PORT` environment variable.

### Run as a Windows Service

Use the MSI installer produced by the  
[Fiscal-Printer-Simulator-Setup-Builder](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Setup-Builder)
repository, or install manually with `sc.exe` / `InstallUtil`.

## Running the Tests

```bash
dotnet test
```

Tests use **NUnit 3**, **Moq** and **FluentAssertions** and cover individual command handlers.

## Implemented Protocol Plugins

| Region | Protocol | Status | Specification |
|--------|----------|--------|---------------|
| 🇵🇱 Poland | **Posnet Thermal** | ✅ Implemented | [THS-I-DEV-26-003](http://www.soft-bit.pl/downloads/all/Posnet/pliki/THS-I-DEV-26-003_Specyfikacja_protokolu_w_drukarkach_Thermal-A_EJ.pdf) |
| 🇵🇱 Poland | Posnet Standard | ⬜ Planned | [DBC-I-DEV-45-021](http://www.soft-bit.pl/downloads/all/Posnet/pliki/DBC-I-DEV-45-021_specyfikacja_protokolu_Posnet_w_drukarkach.pdf) |
| 🇵🇱 Poland | Novitus XML | ⬜ Planned | [Novitus XML v1.2](https://www.novitus.pl/sites/default/files/dla-programistow/drukarki-fiskalne/opis_protokolu_komunikacyjnego_xml_wersja_polska_12.10.2015.pdf) |

## Built With

* [Fleck](https://github.com/statianzo/Fleck) – lightweight WebSocket server library
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) – JSON serialisation

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the code of conduct and the
process for submitting pull requests.

## Versioning

[SemVer](http://semver.org/) is used for versioning.
See [tags on this repository](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service/tags).

## Authors

* **Michal Wojcik** – *Initial work* – [WojcikMM](https://github.com/WojcikMM)

See also the list of [contributors](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service/graphs/contributors).

## License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.
