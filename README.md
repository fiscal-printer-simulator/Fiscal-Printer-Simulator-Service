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

Add additional notes about how to deploy this on a live system

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

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
