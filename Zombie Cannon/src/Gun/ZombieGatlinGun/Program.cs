using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace ZombieGatlinGun
{
    public class Program
    {
        private static readonly OutputPort _trigger = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di13, false);
        private static readonly AnalogIn _ping = new AnalogIn(AnalogIn.Pin.Ain0);
        private static readonly OutputPort _siteLowBit = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di4, false);
        private static readonly OutputPort _siteHighBit = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di5, false);
        private static readonly EasyStepperDriver _barrelStepper = new EasyStepperDriver(FEZ_Pin.Digital.Di9, FEZ_Pin.Digital.Di8, FEZ_Pin.Digital.Di12, FEZ_Pin.Digital.Di10, FEZ_Pin.Digital.Di11);
        private static readonly OutputPort _laser = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di7, false);

        public static void Main()
        {
            MonitorPing();
        }

        private static void CycleSitePattern()
        {
            while (true)
            {
                for (byte n = 0; n < 4; n++)
                {
                    Debug.Print(n.ToString());
                    SetSitePattern(n);
                    Thread.Sleep(2000);
                }
            }
        }

        private static void MonitorPing()
        {
            _ping.SetLinearScale(0, 1000);
            while (true)
            {
                var distance = _ping.Read();
                Debug.Print(distance.ToString());

                SetLaserState(distance < 150);

                if (distance >= 180)
                {
                    SetSitePattern(0);
                }
                else if (distance >= 150 && distance < 180)
                {
                    SetSitePattern(3);
                }
                else if (distance > 30 && distance < 150)
                {
                    SetSitePattern(2);
                }
                else
                {
                    SetSitePattern(1);
                    Fire();
                    MoveToNextBarrel();
                }
                Thread.Sleep(200);
            }
        }

        private static void SetLaserState(bool state)
        {
            _laser.Write(state);    
        }

        private static void MoveToNextBarrel()
        {
            _barrelStepper.Sleep(false);
            _barrelStepper.StepMode = EasyStepperDriver.Mode.OneEighth;
            _barrelStepper.StepDirection = EasyStepperDriver.Direction.Forward;
            _barrelStepper.Step(800);
            _barrelStepper.Sleep(true);
        }


        private static void SetSitePattern(byte pattern)
        {
            _siteLowBit.Write((pattern & 1) == 1);
            _siteHighBit.Write((pattern & 2) == 2);
        }

        private static void Fire()
        {
            _trigger.Write(true);
            Debug.Print("Fire!");
            Thread.Sleep(1000);
            _trigger.Write(false);
        }
    }
}
