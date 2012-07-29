using System.Threading;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace ZombieGatlinGun
{
    public class EasyStepperDriver
    {
        private OutputPort _SleepPin;
        private OutputPort _DirectionPin;
        private OutputPort _StepPin;
        private OutputPort _StepModePinOne;
        private OutputPort _StepModePinTwo;

        private Direction _StepDirection;
        public Direction StepDirection
        {
            get
            {
                return _StepDirection;
            }
            set
            {
                _StepDirection = value;
                ChangeDirection(value);
            }
        }
        private Mode _StepMode;
        public Mode StepMode
        {
            get
            {
                return _StepMode;
            }
            set
            {
                _StepMode = value;
                ChangeStepMode(value);
            }
        }
        private int _Delay = 1;// dont change this to 0 unless using 1/8 and quarter step.

        /// <summary>
        /// Creates an instance of the driver, that only lets you move and choose direction.
        /// </summary>
        /// <param name="DirectionPin">a digital pin that is used for direction</param>
        /// <param name="StepPin">a digital pin that is used for steps</param>
        public EasyStepperDriver(FEZ_Pin.Digital DirectionPin, FEZ_Pin.Digital StepPin)
        {
            _DirectionPin = new OutputPort((Cpu.Pin)DirectionPin, true);
            _StepPin = new OutputPort((Cpu.Pin)StepPin, false);
        }

        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction and put the controller to sleep
        /// </summary>
        /// <param name="DirectionPin">a digital pint that is used for direction</param>
        /// <param name="StepPin">a digital pint that is used for steps</param>
        /// <param name="SleepPin">a digital pint that is used for sleep function</param>
        public EasyStepperDriver(FEZ_Pin.Digital DirectionPin, FEZ_Pin.Digital StepPin, FEZ_Pin.Digital SleepPin)
        {
            _DirectionPin = new OutputPort((Cpu.Pin)DirectionPin, false);
            _StepPin = new OutputPort((Cpu.Pin)StepPin, false);
            _SleepPin = new OutputPort((Cpu.Pin)SleepPin, false);
        }

        /// <summary>
        /// Creates an instance of the driver, that only lets you move, choose direction, sleep, and select step mode
        /// </summary>
        /// <param name="DirectionPin">a digital pint that is used for direction</param>
        /// <param name="StepPin">a digital pint that is used for steps</param>
        /// <param name="SleepPin">a digital pint that is used for sleep function</param>
        /// <param name="StepModePinOne">pin one used to change step mode</param>
        /// <param name="StepModePinTwo">pin two used to change step mode</param>
        public EasyStepperDriver(FEZ_Pin.Digital DirectionPin, FEZ_Pin.Digital StepPin, FEZ_Pin.Digital SleepPin, FEZ_Pin.Digital StepModePinOne, FEZ_Pin.Digital StepModePinTwo)
        {
            _DirectionPin = new OutputPort((Cpu.Pin)DirectionPin, false);
            _StepPin = new OutputPort((Cpu.Pin)StepPin, false);
            _SleepPin = new OutputPort((Cpu.Pin)SleepPin, false);
            _StepModePinOne = new OutputPort((Cpu.Pin)StepModePinOne, true);
            _StepModePinTwo = new OutputPort((Cpu.Pin)StepModePinTwo, true);
        }

        /// <summary>
        /// Moves the stepper motor
        /// </summary>
        /// <param name="Steps">indicate the amount of steps that need to be moved</param>
        public void Step(int Steps)
        {
            for (int i = 0; i < Steps; i++)
            {
                //Still need to add a proper timing function.
                _StepPin.Write(true);
                Thread.Sleep(_Delay);
                _StepPin.Write(false);
            }
        }

        /// <summary>
        /// Put the stepper driver to sleep
        /// </summary>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public bool Sleep(bool sleep)
        {
            if (_SleepPin != null)
            {
                _SleepPin.Write(!sleep);
                return true;
            }
            else
                return false;
        }

        private void ChangeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Forward:
                    if (_DirectionPin != null)
                        _DirectionPin.Write(true);
                    break;
                case Direction.Backward:
                    if (_DirectionPin != null)
                        _DirectionPin.Write(false);
                    break;
            }
        }

        private void ChangeStepMode(Mode mode)
        {
            if (_StepModePinOne != null & _StepModePinTwo != null)
            {
                switch (mode)
                {
                    case Mode.Full:
                        _StepModePinOne.Write(false);
                        _StepModePinTwo.Write(false);
                        break;
                    case Mode.Half:
                        _StepModePinOne.Write(true);
                        _StepModePinTwo.Write(false);
                        break;
                    case Mode.Quarter:
                        _StepModePinOne.Write(false);
                        _StepModePinTwo.Write(true);
                        break;
                    case Mode.OneEighth:
                        _StepModePinOne.Write(true);
                        _StepModePinTwo.Write(true);
                        break;
                }
            }
        }

        public enum Direction
        {
            Forward,
            Backward
        }

        public enum Mode
        {
            Full,
            Half,
            Quarter,
            OneEighth
        }
    }
}