using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Twilio;

namespace ZombieAlertService
{
    class ZombieDetector
    {
        private BluetoothClient _client;
        private Stream _bluetoothStream;

        public void Activate()
        {
            if (!BluetoothRadio.IsSupported)
            {
                UpdateStatus("Bluetooth not supported.");
            }
            else if (BluetoothRadio.PrimaryRadio.Mode == RadioMode.PowerOff)
            {
                UpdateStatus("Bluetooth turned off");
            }

            FindZombieDetector();

            // Wait for zombie alert.

        }

        public void SendCommand(string command)
        {
            UpdateStatus("Sending command: " + command);
            if (_bluetoothStream != null)
            {
                var buffer = System.Text.Encoding.Default.GetBytes(command);
                _bluetoothStream.Write(buffer, 0, buffer.Length);
            }
        }

        private void FindZombieDetector()
        {
            UpdateStatus("Searching for Zombie detector...");

            BackgroundWorker bwDiscoverRover = new BackgroundWorker();
            bwDiscoverRover.DoWork += new DoWorkEventHandler(detector_DoWork);
            bwDiscoverRover.RunWorkerCompleted += new RunWorkerCompletedEventHandler(detector_RunWorkerCompleted);
            bwDiscoverRover.RunWorkerAsync();
        }

        private void detector_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_bluetoothStream != null)
                    _bluetoothStream.Close();

                _client = new BluetoothClient();

                BluetoothDeviceInfo gadgeteerDevice = null;

                while (gadgeteerDevice == null)
                {
                    gadgeteerDevice = _client.DiscoverDevices().Where(d => d.DeviceName == "Gadgeteer")
                        .FirstOrDefault();
                    UpdateStatus("Still looking...");
                }

                if (gadgeteerDevice != null)
                {
                    _client.SetPin(gadgeteerDevice.DeviceAddress, "1234");
                    _client.Connect(gadgeteerDevice.DeviceAddress, BluetoothService.SerialPort);
                    _bluetoothStream = _client.GetStream();

                    e.Result = true;
                }
                else
                {
                    e.Result = false;
                }
            }
            catch (Exception)
            {
                e.Result = false;
            }
        }

        private void detector_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var deviceFound = (bool)e.Result;
            if (!deviceFound)
            {
                UpdateStatus("Zombie detector not found or connection error.");
            }
            else
            {
                UpdateStatus("Connected to Zombie detector.");
                Console.ReadLine();
                SendTxt();
            }
        }

        private void SendTxt()
        {
            var _twilioClient = new TwilioClient();
             //_twilioClient.MessageReceived += new TwilioClient.MessageReceivedEventHandler(_twilioClient_MessageReceived);

            // To send an SMS message, first set the FromNumber (or default to my number) and then call:
            var toNumber = "+19318087937";
            var messageText = "The zombies are coming!!!";
            TwilioClient.SendSms(toNumber, messageText);
            TwilioClient.SendSms("+12178406722", "The zombies are coming!!");
            TwilioClient.SendSms("+14086603819", "The zombies are coming!!");
            TwilioClient.SendSms("+16152182869", "The zombies are coming!!");
            TwilioClient.SendSms("+16158873311", "The zombies are coming!!");
        }

        // This checks for messages on a timer that defaults to every ten seconds, but can be changed by setting the TimerInterval
        void _twilioClient_MessageReceived(object sender, SMSMessage e)
        {
            Console.WriteLine(e.Body);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_bluetoothStream != null)
            {
                _bluetoothStream.Close();
                _bluetoothStream.Dispose();
            }
        }

        public void UpdateStatus(string status)
        {
            Console.WriteLine(status);      // TODO:  convert to an event.
        }
    }
}
