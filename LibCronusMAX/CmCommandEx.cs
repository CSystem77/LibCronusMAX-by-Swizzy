using System;
using System.Collections.Generic;

namespace LibCronusMAX
{
    /// <summary>
    ///     Class used to send outputs with some additional flags to the CronusMAX Plus device (which sends the outputs to the
    ///     connected console/controller)
    /// </summary>
    public class CmCommandEx : OutputBuffer
    {
        private IOStatus.LedState _led1 = IOStatus.LedState.On;

        private IOStatus.LedState _led2;

        private IOStatus.LedState _led3;

        private IOStatus.LedState _led4;

        private bool _ledsChanged;

        private bool _rumbleChanged;

        private bool _blockRumble;

        private bool _resetRumble;

        private int _rumbleA;

        private int _rumbleB;

        private int _rumbleLt;

        private int _rumbleRt;

        /// <summary>
        ///     Gets/Sets the flag that resets the leds to their default state (based on what the console sends)
        /// </summary>
        public bool ResetLeds
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets/Sets the flag used to tell the CronusMAX Plus to turn off the connected controller
        /// </summary>
        public bool TurnOffController
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets/Sets the flag used to tell the CronusMAX Plus to reset the block rumbles flag and sets rumbles to their
        ///     default value (0)
        /// </summary>
        public bool ResetRumble
        {
            get
            {
                return _resetRumble;
            }
            set
            {
                _resetRumble = value;
                if (value)
                {
                    _rumbleA = (_rumbleB = (_rumbleLt = (_rumbleRt = 0)));
                    _rumbleChanged = true;
                }
            }
        }

        /// <summary>
        ///     Gets/Sets the rumble value for the right rumble motor (strong)
        /// </summary>
        public int RumbleA
        {
            get
            {
                return _rumbleA;
            }
            set
            {
                if (Math.Ceiling((double)_rumbleA / 2.55) != (double)value)
                {
                    _rumbleChanged = true;
                    _blockRumble = false;
                }
                _rumbleA = (int)Math.Ceiling((double)value * 2.55);
            }
        }

        /// <summary>
        ///     Gets/Sets the rumble value for the left rumble motor (weak)
        /// </summary>
        public int RumbleB
        {
            get
            {
                return _rumbleB;
            }
            set
            {
                if (Math.Ceiling((double)_rumbleB / 2.55) != (double)value)
                {
                    _rumbleChanged = true;
                    _blockRumble = false;
                }
                _rumbleB = (int)Math.Ceiling((double)value * 2.55);
            }
        }

        /// <summary>
        ///     Gets/Sets the rumble value for the left trigger rumble on Xbox One
        /// </summary>
        public int RumbleLt
        {
            get
            {
                return _rumbleLt;
            }
            set
            {
                if (Math.Ceiling((double)_rumbleLt / 2.55) != (double)value)
                {
                    _rumbleChanged = true;
                    _blockRumble = false;
                }
                _rumbleLt = (int)Math.Ceiling((double)value * 2.55);
            }
        }

        /// <summary>
        ///     Gets/Sets the rumble value for the right trigger rumble on Xbox One
        /// </summary>
        public int RumbleRt
        {
            get
            {
                return _rumbleRt;
            }
            set
            {
                if (Math.Ceiling((double)_rumbleRt / 2.55) != (double)value)
                {
                    _rumbleChanged = true;
                    _blockRumble = false;
                }
                _rumbleRt = (int)Math.Ceiling((double)value * 2.55);
            }
        }

        public bool BlockRumble
        {
            get
            {
                return _blockRumble;
            }
            set
            {
                _blockRumble = value;
                _rumbleChanged = true;
            }
        }

        /// <summary>
        ///     Gets/Sets the state of LED1 (Player 1)
        /// </summary>
        public IOStatus.LedState Led1
        {
            get
            {
                return _led1;
            }
            set
            {
                if (_led1 != value)
                {
                    _ledsChanged = true;
                }
                _led1 = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the state of LED2 (Player 2)
        /// </summary>
        public IOStatus.LedState Led2
        {
            get
            {
                return _led2;
            }
            set
            {
                if (_led2 != value)
                {
                    _ledsChanged = true;
                }
                _led2 = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the state of LED3 (Player 3)
        /// </summary>
        public IOStatus.LedState Led3
        {
            get
            {
                return _led3;
            }
            set
            {
                if (_led3 != value)
                {
                    _ledsChanged = true;
                }
                _led3 = value;
            }
        }

        /// <summary>
        ///     Gets/Sets the state of LED4 (Player 4)
        /// </summary>
        public IOStatus.LedState Led4
        {
            get
            {
                return _led4;
            }
            set
            {
                if (_led4 != value)
                {
                    _ledsChanged = true;
                }
                _led4 = value;
            }
        }

        /// <summary>
        ///     Default constructor that makes a default CmCommandEx object with the default button values (0) and default flags
        /// </summary>
        public CmCommandEx()
        {
        }

        /// <summary>
        ///     Constructor that makes a CmCommandEx object with the specified button values and the default flags
        /// </summary>
        /// <param name="outputs">Button values to use</param>
        public CmCommandEx(IList<int> outputs)
            : base(outputs)
        {
        }

        /// <summary>
        ///     Constructor that makes a CmCommand object with the specified button values and default flags
        /// </summary>
        /// <param name="output0">Value of button 0 (PS/Xbox)</param>
        /// <param name="param">Value of whatever buttons you specify</param>
        /// <remarks>param is treated as if you passed in a array of buttons</remarks>
        public CmCommandEx(int output0, params int[] param)
            : base(output0, param)
        {
        }

        /// <summary>
        /// Creates a copy of the current CmCommandEx (use this to maintain the state of the leds, rumble and block rumble flags and resetting everything else)
        /// </summary>
        /// <returns></returns>
        public CmCommandEx CreateCopy()
        {
            return new CmCommandEx(Outputs)
            {
                _led1 = Led1,
                _led2 = Led2,
                _led3 = Led3,
                _led4 = Led4,
                _rumbleA = RumbleA,
                _rumbleB = RumbleB,
                _rumbleLt = RumbleLt,
                _rumbleRt = RumbleRt,
                _blockRumble = BlockRumble
            };
        }

        internal byte[] ToByteArray()
        {
            byte[] ret = new byte[52]
            {
                255,
                1,
                (byte)(_ledsChanged ? 1 : 0),
                (byte)_led1,
                (byte)_led2,
                (byte)_led3,
                (byte)_led4,
                (byte)(ResetLeds ? 1 : 0),
                (byte)(_rumbleChanged ? 1 : 0),
                (byte)Math.Min(255, Math.Max(0, _rumbleA)),
                (byte)Math.Min(255, Math.Max(0, _rumbleB)),
                (byte)Math.Min(255, Math.Max(0, _rumbleLt)),
                (byte)Math.Min(255, Math.Max(0, _rumbleRt)),
                (byte)(ResetRumble ? 1 : 0),
                (byte)(_blockRumble ? 1 : 0),
                (byte)(TurnOffController ? 1 : 0),
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };
            for (int i = 0; i < 36; i++)
            {
                if (i < 30)
                {
                    ret[16 + i] = (byte)(sbyte)Math.Max(Math.Min(100, Outputs[i]), -100);
                }
                else
                {
                    ret[16 + i] = (byte)Outputs[i];
                }
            }
            return ret;
        }
    }
}
