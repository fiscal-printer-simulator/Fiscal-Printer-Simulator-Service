using Fleck;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using FiscalPrinterSimulatorLibraries;
using FiscalPrinterSimulatorService.ReduxActions;

namespace FiscalPrinterSimulatorService
{
    public class FiscalPrinterSimulatorService : ServiceBase
    {
        private static readonly object _locker = new object();
        private IFiscalPrinter _printer;
        private readonly WebSocketServer _server;
        private readonly SerialPort _serialPort;
        private readonly List<IWebSocketConnection> _connections;


        public FiscalPrinterSimulatorService()
        {
            var websocketPort = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FP_SERVICE_PORT")) ?
                Environment.GetEnvironmentVariable("FP_SERVICE_PORT")
                : "8181";
            _serialPort = new SerialPort();
            _connections = new List<IWebSocketConnection>();
            _server = new WebSocketServer($"ws://0.0.0.0:{websocketPort}");
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialPort_DataReceived);
            this.ServiceName = "Fiscal Printer Simulator";
            LoadFiscalPrinterHandlingPlugins();
        }

        public void RunServiceAsConsoleApp(string[] args)
        {
            this.OnStart(args);
            Console.WriteLine("Service starts successfully");
            Console.WriteLine("Click any key to continue...");
            Console.ReadKey();
            this.OnStop();
            Console.WriteLine("Service stops successfully");
            Console.WriteLine("Click any key to continue...");
            Console.ReadKey();
        }

        protected override void OnStart(string[] args)
        {
            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    lock (_locker)
                    {
                        _connections.Add(socket);
                    }
                    WebsocketActionDispatcher.SendMessage(socket, new ActualTranslationsForSimulatorClientAction());
                    WebsocketActionDispatcher.SendMessage(socket, new ActualServerStateAction(_serialPort));
                    WebsocketActionDispatcher.SendMessage(socket, new AvaliblePortsAction());
                };
                socket.OnClose = () => _connections.Remove(socket);
                socket.OnMessage = message =>
                {
                    try
                    {
                        _connections.Dispatch(message, _serialPort);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
            });
        }

        protected override void OnStop()
        {
            lock (_locker)
            {
                _connections.ForEach(m => m.Close());
            }
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as SerialPort;
            if (serialPort is null)
                throw new Exception("Invalid operation occured");

            var commandHandlersResponses = _printer.HandleReceivedData((sender as SerialPort).ReadExisting());

            foreach (var commandHandlerResponse in commandHandlersResponses)
            {
                if (!string.IsNullOrEmpty(commandHandlerResponse.OutputReciptBuffer))
                {
                    _connections.ForEach(connection =>
                    WebsocketActionDispatcher.SendMessage(connection,
                        new SendReceiptOuptutDataAction(commandHandlerResponse.OutputReciptBuffer)
                        ));
                }
                if (commandHandlerResponse.OutputCommand != null)
                {
                    serialPort.Write(commandHandlerResponse.OutputCommand.ToString());
                }
            }
        }


        private void LoadFiscalPrinterHandlingPlugins()
        {
            var path = "./Plugins";
#if DEBUG && x64
            path = @"..\..\..\..\FiscalPrinterSimulatorLibraries\ThermalFiscalPrinter\bin\x64\";
#elif DEBUG && x86
            path = @"..\..\..\..\FiscalPrinterSimulatorLibraries\ThermalFiscalPrinter\bin\x86\";
#endif

            var plugins = Directory.GetFiles(path, "*FiscalPrinterSimulatorLibraries.dll", SearchOption.TopDirectoryOnly)
                .Select(m=> Path.GetFullPath(m))
                .Where(m =>!m.Contains("Base"))
                .Select(m => Assembly.LoadFrom(m))
                .Where(m => m != null)
                .SelectMany(m => m.GetTypes())
                .ToList()
                .Where(m => !m.IsInterface && !m.IsAbstract && m.GetInterface(typeof(IFiscalPrinter).FullName) != null)
                .Select(m=> (IFiscalPrinter)Activator.CreateInstance(m))
                .ToList();

            if (plugins.Count() == 1)
            {
                _printer = plugins.First();
            }


        }
    }
}
