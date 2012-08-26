using System;
using System.Linq;
using System.Diagnostics;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace ZombieAlertService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ZombieAlertService
    {
        private static ZombieDetector _detector = new ZombieDetector();

        static void Main(string[] args)
        {
            _detector.Activate();

            while(true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
