using System;
using System.IO.Ports;
using Microsoft.SPOT;

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

            _client.EnterPairingMode();

            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");
        }

        private void SerialPortOnDataReceived(Serial sender, SerialData data)
        {
            throw new NotImplementedException();
        }

        private void bluetooth_DataReceived(Bluetooth sender, string data)
        {
            switch (data.ToUpper())
            {
                case "A":
                    break;
                case "B":
                    break;
                case "S":
                    break;
                case "T":
                    break;
                case "R":
                    break;
                case "L":
                    break;
                default:
                    break;
            }
        }
    }
}
