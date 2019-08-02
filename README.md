# Fiscal Printer Simulator Service
[![Build Status](https://travis-ci.org/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service.svg?branch=master)](https://travis-ci.org/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service)

This is service for emulate real fiscal printer device.  
You can test your software integrated with fiscal devices without any costs.
Service is written as pluginable and end user can select which plugins he (or she) needs.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

To run this application you need:
* dotnet core version 2.2 or above
* __Fiscal Printer Simulator Client__ * (nice to have - to check output of running commands)
* Diagnostic tool for simulate curent protocol POS calls. 

### Installing

Steps:
* run command `dotnet restore` on root directory to restore all needed dependencies 

#### That's all :)

## Running the tests

* Just run command `dotnet test` on root directory 

## Deployment

### Implemented Plugins for Fiscal Printer Protocols:
- #### Polish __[PL]__
  - [x] - Thermal [[Documentation]](http://www.soft-bit.pl/downloads/all/Posnet/pliki/THS-I-DEV-26-003_Specyfikacja_protokolu_w_drukarkach_Thermal-A_EJ.pdf)  
  - [ ] - Posnet  [[Documentation]](http://www.soft-bit.pl/downloads/all/Posnet/pliki/DBC-I-DEV-45-021_specyfikacja_protokolu_Posnet_w_drukarkach.pdf)  
  - [ ] - New XML Protocol  [[Documentation]](https://www.novitus.pl/sites/default/files/dla-programistow/drukarki-fiskalne/opis_protokolu_komunikacyjnego_xml_wersja_polska_12.10.2015.pdf)  

## Built With
* [Fleck](https://github.com/statianzo/Fleck) - lightweight library to websocket communication
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) - simple but verry good Json parser

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service/tags). 

## Authors

* **Michal Wojcik** - *Initial work* - [WojcikMM](https://github.com/WojcikMM)

See also the list of [contributors](https://github.com/fiscal-printer-simulator/Fiscal-Printer-Simulator-Service/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
