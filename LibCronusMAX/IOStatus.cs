using System;
using System.Collections.Generic;

namespace LibCronusMAX
{
    /// <summary>
    ///     Class used to represent the I/O Status response
    /// </summary>
    public class IOStatus : EventArgs
    {
        /// <summary>
        ///     Currently connected console values
        /// </summary>
        public enum ConsoleType : byte
        {
            /// <summary>
            ///     No console connected
            /// </summary>
            None = 0,
            /// <summary>
            ///     A PS3/Playstation 3 is connected
            /// </summary>
            Ps3 = 1,
            /// <summary>
            ///     A Xbox 360 is connected
            /// </summary>
            Xb360 = 2,
            /// <summary>
            ///     A PS4/Playstation 4 is connected
            /// </summary>
            Ps4 = 3,
            /// <summary>
            ///     A Xbox One is connected
            /// </summary>
            Xb1 = 4,
            /// <summary>
            ///     Wheel Edition flag (we're outputting as a wheel)
            /// </summary>
            Wheel = 8,
            /// <summary>
            ///     Compound flag for PS3 + Wheel Edition
            /// </summary>
            Ps3Wheel = 9,
            /// <summary>
            ///     Compound flag for XB360 + Wheel Edition
            /// </summary>
            Xb360Wheel = 10,
            /// <summary>
            ///     Compound flag for PS4 + Wheel Edition
            /// </summary>
            Ps4Wheel = 11,
            /// <summary>
            ///     Compound flag for XB1 + Wheel Edition
            /// </summary>
            Xb1Wheel = 12
        }

        /// <summary>
        ///     Connected controller values
        /// </summary>
        public enum ControllerType : byte
        {
            /// <summary>
            ///     No controller is connected
            /// </summary>
            None = 0,
            /// <summary>
            ///     DS3 / Dualshock 3 or Sixaxis controller is connected
            /// </summary>
            Ps3 = 0x10,
            /// <summary>
            ///     Xbox 360 controller is connected
            /// </summary>
            Xb360 = 0x20,
            /// <summary>
            ///     WiiMote is connected
            /// </summary>
            Wii = 48,
            /// <summary>
            ///     WiiMote with Nunchuck is connected
            /// </summary>
            WiiN = 49,
            /// <summary>
            ///     Wii Classic Pro controller is connected
            /// </summary>
            WiiPro = 50,
            /// <summary>
            ///     DS4 / Dualshock 4 is controller connected
            /// </summary>
            Ps4 = 0x40,
            /// <summary>
            ///     Xbox One Controller connected
            /// </summary>
            Xb1 = 80,
            /// <summary>
            ///     A Logitech G25 Wheel is connected
            /// </summary>
            G25 = 105,
            /// <summary>
            ///     A Logitech G27 Wheel is connected
            /// </summary>
            G27 = 107
        }

        /// <summary>
        ///     Led state values
        /// </summary>
        public enum LedState : byte
        {
            /// <summary>
            ///     LED is Off
            /// </summary>
            Off,
            /// <summary>
            ///     LED is On
            /// </summary>
            On,
            /// <summary>
            ///     LED is blinking
            /// </summary>
            Blink,
            /// <summary>
            ///     LED is blinking slowly
            /// </summary>
            BlinkSlow
        }

        /// <summary>
        ///     Input Status (Controller Inputs)
        /// </summary>
        public readonly InputBuffer InputStatus;

        /// <summary>
        ///     Output Status (Console Outputs)
        /// </summary>
        public readonly OutputBuffer OutputStatus;

        private int _battery;

        private float _lastCpuLoad;

        private int _rumbleA;

        private int _rumbleB;

        private int _rumbleLt;

        private int _rumbleRt;

        /// <summary>
        ///     Gets the currently connected console as a string
        /// </summary>
        public string Console
        {
            get
            {
                switch (ConnectedConsole)
                {
                    case ConsoleType.Ps3:
                    case ConsoleType.Ps3Wheel:
                        return "PS3";
                    case ConsoleType.Xb360:
                    case ConsoleType.Xb360Wheel:
                        return "XB360";
                    case ConsoleType.Ps4:
                    case ConsoleType.Ps4Wheel:
                        return "PS4";
                    case ConsoleType.Xb1:
                    case ConsoleType.Xb1Wheel:
                        return "XB1";
                    default:
                        return "None";
                }
            }
        }

        /// <summary>
        ///     Gets the currently connected controller as a string
        /// </summary>
        public string Controller
        {
            get
            {
                switch (ConnectedController)
                {
                    case ControllerType.Ps3:
                        return "PS3";
                    case ControllerType.Xb360:
                        return "XB360";
                    case ControllerType.Wii:
                    case ControllerType.WiiN:
                    case ControllerType.WiiPro:
                        return "WII";
                    case ControllerType.Ps4:
                        return "PS4";
                    case ControllerType.Xb1:
                        return "XB1";
                    case ControllerType.G25:
                        return "G25";
                    case ControllerType.G27:
                        return "G27";
                    case ControllerType.None:
                        return "None";
                    default:
                        return ConnectedController.ToString();
                }
            }
        }

        /// <summary>
        ///     Gets the timestamp of when this status was received
        /// </summary>
        public DateTime Timestamp
        {
            get;
        } = DateTime.Now;


        /// <summary>
        ///     Gets the currently connected controller
        /// </summary>
        public ControllerType ConnectedController
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the currently connected console
        /// </summary>
        public ConsoleType ConnectedConsole
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the currently active slot as a string
        /// </summary>
        public string Slot => SlotValue.ToString();

        /// <summary>
        ///     Gets the currently active slot
        /// </summary>
        public int SlotValue
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the current battery level/state as a string
        /// </summary>
        public string Battery
        {
            get
            {
                if (_battery > 10)
                {
                    return "Charging";
                }
                return BatteryValue + "%";
            }
        }

        /// <summary>
        ///     Gets the current battery level/state
        /// </summary>
        /// <remarks>Anything above 100 means it's charging</remarks>
        public int BatteryValue
        {
            get
            {
                return _battery * 10;
            }
            private set
            {
                _battery = value;
            }
        }

        /// <summary>
        ///     Gets the CronusMAX Plus CPULoad value as a string
        /// </summary>
        public string Cpuload => CpuloadValue + "%";

        /// <summary>
        ///     Gets the CronusMAX Plus CPULoad value
        /// </summary>
        public int CpuloadValue
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the state of LED1 (Player 1) as a string
        /// </summary>
        public string Led1 => GetLedString(LedState1);

        /// <summary>
        ///     Gets the state of LED1 (Player 1)
        /// </summary>
        public LedState LedState1
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the state of LED2 (Player 2) as a string
        /// </summary>
        public string Led2 => GetLedString(LedState2);

        /// <summary>
        ///     Gets the state of LED2 (Player 2)
        /// </summary>
        public LedState LedState2
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the state of LED3 (Player 3) as a string
        /// </summary>
        public string Led3 => GetLedString(LedState3);

        /// <summary>
        ///     Gets the state of LED3 (Player 3)
        /// </summary>
        public LedState LedState3
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the state of LED4 (Player 4) as a string
        /// </summary>
        public string Led4 => GetLedString(LedState4);

        /// <summary>
        ///     Gets the state of LED4 (Player 4)
        /// </summary>
        public LedState LedState4
        {
            get;
            private set;
        }

        /// <summary>
        ///     Gets the current value of RUMBLE_A as a string
        /// </summary>
        public string RumbleA => RumbleValueA + "%";

        /// <summary>
        ///     Gets the current value of RUMBLE_B as a string
        /// </summary>
        public string RumbleB => RumbleValueB + "%";

        /// <summary>
        ///     Gets the current value of RUMBLE_LT as a string
        /// </summary>
        public string RumbleLt => RumbleValueLt + "%";

        /// <summary>
        ///     Gets the current value of RUMBLE_RT as a string
        /// </summary>
        public string RumbleRt => RumbleValueRt + "%";

        /// <summary>
        ///     Gets the current value of RUMBLE_A
        /// </summary>
        public int RumbleValueA => (int)((double)_rumbleA / 2.55);

        /// <summary>
        ///     Gets the current value of RUMBLE_B
        /// </summary>
        public int RumbleValueB => (int)((double)_rumbleB / 2.55);

        /// <summary>
        ///     Gets the current value of RUMBLE_LT
        /// </summary>
        public int RumbleValueLt => (int)((double)_rumbleLt / 2.55);

        /// <summary>
        ///     Gets the current value of RUMBLE_RT
        /// </summary>
        public int RumbleValueRt => (int)((double)_rumbleRt / 2.55);

        private IOStatus(InputBuffer inputs, OutputBuffer outputs)
        {
            InputStatus = inputs;
            OutputStatus = outputs;
        }

        internal static IOStatus GetIOStatusFromInput(byte[] data, IOStatus previous)
        {
            List<int> lst = new List<int>();
            for (int i = 0; i < 29; i++)
            {
                lst.Add((sbyte)data[18 + i]);
            }
            InputBuffer inputs = new InputBuffer(lst);
            IOStatus ret = (previous != null) ? new IOStatus(inputs, previous.OutputStatus) : new IOStatus(inputs, new OutputBuffer());
            short freq = BitConverter.ToInt16(data, 48);
            if (freq < 1)
            {
                freq = 1;
            }
            if (previous != null)
            {
                ret._lastCpuLoad = previous._lastCpuLoad;
            }
            ret._lastCpuLoad = (float)((double)ret._lastCpuLoad * 0.9 + 0.1 * (double)(short)(BitConverter.ToUInt16(data, 4) * 100 / freq));
            ret.CpuloadValue = (ushort)ret._lastCpuLoad;
            ret.SlotValue = data[6];
            ret.ConnectedController = (ControllerType)data[7];
            ret.ConnectedConsole = (ConsoleType)data[8];
            ret.LedState1 = (LedState)data[9];
            ret.LedState2 = (LedState)data[10];
            ret.LedState3 = (LedState)data[11];
            ret.LedState4 = (LedState)data[12];
            ret._rumbleA = data[13];
            ret._rumbleB = data[14];
            ret._rumbleRt = data[15];
            ret._rumbleLt = data[16];
            ret.BatteryValue = data[17];
            return ret;
        }

        internal static IOStatus GetIOStatusFromOutput(byte[] data, IOStatus previous)
        {
            List<int> lst = new List<int>();
            for (int i = 0; i < 35; i++)
            {
                lst.Add((sbyte)data[4 + i]);
            }
            OutputBuffer outputs = new OutputBuffer(lst);
            IOStatus ret = (previous != null) ? new IOStatus(previous.InputStatus, outputs) : new IOStatus(new InputBuffer(), outputs);
            if (previous == null)
            {
                return ret;
            }
            ret._lastCpuLoad = previous._lastCpuLoad;
            ret.CpuloadValue = previous.CpuloadValue;
            ret.SlotValue = previous.SlotValue;
            ret.ConnectedController = previous.ConnectedController;
            ret.ConnectedConsole = previous.ConnectedConsole;
            ret.LedState1 = previous.LedState1;
            ret.LedState2 = previous.LedState2;
            ret.LedState3 = previous.LedState3;
            ret.LedState4 = previous.LedState4;
            ret._rumbleA = previous._rumbleA;
            ret._rumbleB = previous._rumbleB;
            ret._rumbleLt = previous._rumbleLt;
            ret._rumbleRt = previous._rumbleRt;
            ret.BatteryValue = previous._battery;
            return ret;
        }

        private static string GetLedString(LedState state)
        {
            switch (state)
            {
                case LedState.Off:
                    return "Off";
                case LedState.On:
                    return "On";
                case LedState.Blink:
                    return "Blink";
                case LedState.BlinkSlow:
                    return "Slow Blink";
                default:
                    return "";
            }
        }
    }
}
