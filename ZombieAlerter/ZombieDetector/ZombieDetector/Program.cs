using System;
using System.IO.Ports;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using GT = Gadgeteer;
using Gadgeteer.Modules.Seeed;

namespace ZombieDetector
{
    public partial class Program
    {
        private Bluetooth _bluetooth;
        private Bluetooth.Client _client;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            _bluetooth = new Bluetooth(4);
            _bluetooth.DebugPrintEnabled = true;
            _client = _bluetooth.ClientMode;
            _bluetooth.SetDeviceName("Gadgeteer");
            _bluetooth.SetPinCode("1234");
            _bluetooth.DataReceived += bluetooth_DataReceived;

            button.ButtonPressed += (sender, state) => _client.EnterPairingMode();

            var monitor = new GT.Timer(500);
            monitor.Tick += timer =>
                {
                    if (_bluetooth.IsConnected)
                    {
                        //_client.Send("xxx");
                    }
                };
            monitor.Start();
            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");

            var sensePuss = new GT.Timer(1000);
            sensePuss.Tick += timer =>
                {
                    relays.Relay3 = moistureSensor.GetMoistureReading() > 500;
                    Debug.Print(moistureSensor.GetMoistureReading().ToString());
                };
            sensePuss.Start();
        }

        /*
        public void SendCommand(string command)
        {
            if (_bluetoothStream != null)
            {
                var buffer = System.Text.Encoding.Default.GetBytes(command);
                _bluetoothStream.Write(buffer, 0, buffer.Length);
            }
        }
         */ 
       
        private void bluetooth_DataReceived(Bluetooth sender, string data)
        {
            switch (data.ToUpper())
            {
                case "A":
                    break;
                default:
                    break;
            }
        }
    }
}
