using System;
using System.Collections.Generic;

namespace LibCronusMAX
{
    public class DeviceSettings : EventArgs
    {
        /// <summary>
        ///     Backlight settings for CronusMAX Plus (v3 hardware)
        /// </summary>
        [Flags]
        public enum BackLightValues : byte
        {
            /// <summary>
            ///     Disable the backlight
            /// </summary>
            Disabled = 0x0,
            /// <summary>
            ///     Custom Color - Blue
            /// </summary>
            Blue = 0x1,
            /// <summary>
            ///     Custom Color - Green
            /// </summary>
            Green = 0x2,
            /// <summary>
            ///     Custom Color - Cyan
            /// </summary>
            Cyan = 0x3,
            /// <summary>
            ///     Custom Color - Red
            /// </summary>
            Red = 0x4,
            /// <summary>
            ///     Custom Color - Magenta
            /// </summary>
            Magenta = 0x5,
            /// <summary>
            ///     Custom Color - Yellow
            /// </summary>
            Yellow = 0x6,
            /// <summary>
            ///     Custom Color - White
            /// </summary>
            White = 0x7,
            /// <summary>
            ///     Internal compound flag used to determine if a custom color is used
            /// </summary>
            Custom = 0x7,
            /// <summary>
            ///     Strict Controller Player Leds
            /// </summary>
            StrictControllerPlayerLeds = 0x40,
            /// <summary>
            ///     Mimic Controller Player Leds
            /// </summary>
            MimicControllerPlayerLeds = 0x80
        }

        /// <summary>
        ///     Bluetooth rumble flags
        /// </summary>
        public enum BtRumbles : byte
        {
            /// <summary>
            ///     Full Speed
            /// </summary>
            Fullspeed,
            /// <summary>
            ///     Flow Control
            /// </summary>
            FlowControl,
            /// <summary>
            ///     Flow Control+
            /// </summary>
            FlowControlPlus,
            /// <summary>
            ///     Disabled
            /// </summary>
            Disabled
        }

        /// <summary>
        ///     Output protocol values
        /// </summary>
        public enum Consoles : byte
        {
            /// <summary>
            ///     Automatically determine connected console (Default)
            /// </summary>
            Automatic,
            /// <summary>
            ///     Force PS3/Playstation 3
            /// </summary>
            Ps3,
            /// <summary>
            ///     Force Xbox 360
            /// </summary>
            Xb360,
            /// <summary>
            ///     Force PS4/Playstation 4
            /// </summary>
            Ps4,
            /// <summary>
            ///     Force Xbox One
            /// </summary>
            Xb1
        }

        /// <summary>
        ///     Controller timeout values
        /// </summary>
        public enum TimeoutValues : byte
        {
            /// <summary>
            ///     Disable this feature
            /// </summary>
            Disabled = 0,
            /// <summary>
            ///     Disable after 5 minutes of inactivity
            /// </summary>
            Minutes5 = 5,
            /// <summary>
            ///     Disable after 10 minutes of inactivity
            /// </summary>
            Minutes10 = 10,
            /// <summary>
            ///     Disable after 15 minutes of inactivity
            /// </summary>
            Minutes15 = 0xF,
            /// <summary>
            ///     Disable after 20 minutes of inactivity
            /// </summary>
            Minutes20 = 20,
            /// <summary>
            ///     Disable after 30 minutes of inactivity
            /// </summary>
            Minutes30 = 30,
            /// <summary>
            ///     Disable after 1 hour of inactivity
            /// </summary>
            Minutes60 = 60
        }

        [Flags]
        private enum SpeedUpSettings : byte
        {
            InframeOut = 0x1,
            InframeIn = 0x2,
            OneMsResponse = 0x4,
            Ds4Boost = 0x8
        }

        private SpeedUpSettings _speedUpFlags;

        /// <summary>
        /// Gets/Sets the CronusMAX Plus Backlight setting
        /// </summary>
        public BackLightValues BackLight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the bluetooth rumble settings
        /// </summary>
        public BtRumbles BtRumble
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the bluetooth searching (PS4/Wii) settings
        /// </summary>
        public bool BtSearching
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the DS3 / DualShock 3/Sixaxis Autopairing setting
        /// </summary>
        public bool Ds3AutoPair
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the DS4 / Dualshock 4 Brightness value (range: 0 - 255)
        /// </summary>
        public byte Ds4LightbarBrightness
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Idle Timeout flag
        /// </summary>
        public TimeoutValues IdleTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Output Protocol
        /// </summary>
        public Consoles OutputProtocol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Partial PS4 Crossover flag
        /// </summary>
        public bool PartialDs4CrossOver
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Remote Control Slot flag
        /// </summary>
        public bool RemoteControlSlot
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Remote Control Slot G8 Alternate flag
        /// </summary>
        public bool RemoteControlSlotG8
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Slot Recall flag
        /// </summary>
        public bool SlotRecall
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the In Frame Out Speedup setting flag
        /// </summary>
        public bool InFrameOut
        {
            get
            {
                return (_speedUpFlags & SpeedUpSettings.InframeOut) == SpeedUpSettings.InframeOut;
            }
            set
            {
                if (value)
                {
                    _speedUpFlags |= SpeedUpSettings.InframeOut;
                }
                else
                {
                    _speedUpFlags &= ~SpeedUpSettings.InframeOut;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the In Frame In Speedup setting flag
        /// </summary>
        public bool InFrameIn
        {
            get
            {
                return (_speedUpFlags & SpeedUpSettings.InframeIn) == SpeedUpSettings.InframeIn;
            }
            set
            {
                if (value)
                {
                    _speedUpFlags |= SpeedUpSettings.InframeIn;
                }
                else
                {
                    _speedUpFlags &= ~SpeedUpSettings.InframeIn;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the 1ms Speedup setting flag
        /// </summary>
        public bool OneMsResponse
        {
            get
            {
                return (_speedUpFlags & SpeedUpSettings.OneMsResponse) == SpeedUpSettings.OneMsResponse;
            }
            set
            {
                if (value)
                {
                    _speedUpFlags |= SpeedUpSettings.OneMsResponse;
                }
                else
                {
                    _speedUpFlags &= ~SpeedUpSettings.OneMsResponse;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the DS4 / Dualshock 4 Bluetooth Boost Speedup setting flag
        /// </summary>
        public bool Ds4BtBoost
        {
            get
            {
                return (_speedUpFlags & SpeedUpSettings.Ds4Boost) == SpeedUpSettings.Ds4Boost;
            }
            set
            {
                if (value)
                {
                    _speedUpFlags |= SpeedUpSettings.Ds4Boost;
                }
                else
                {
                    _speedUpFlags &= ~SpeedUpSettings.Ds4Boost;
                }
            }
        }

        public DeviceSettings()
        {
        }

        internal DeviceSettings(IList<byte> data)
        {
            OutputProtocol = (Consoles)data[0];
            BtSearching = (data[1] == 1);
            RemoteControlSlot = (data[2] == 1 || data[2] == 8);
            RemoteControlSlotG8 = (data[2] == 8);
            SlotRecall = (data[3] == 1);
            Ds3AutoPair = (data[4] == 1);
            BtRumble = (BtRumbles)data[5];
            _speedUpFlags = (SpeedUpSettings)data[6];
            BackLight = (BackLightValues)data[7];
            Ds4LightbarBrightness = data[8];
            PartialDs4CrossOver = (data[9] == 1);
            IdleTimeout = (TimeoutValues)data[10];
        }

        internal byte[] ToByteArray()
        {
            if (BackLight == BackLightValues.White)
            {
                BackLight = BackLightValues.Disabled;
            }
            return new byte[11]
            {
                (byte)OutputProtocol,
                (byte)(BtSearching ? 1 : 0),
                (byte)((RemoteControlSlot && RemoteControlSlotG8) ? 8 : (RemoteControlSlot ? 1 : 0)),
                (byte)(SlotRecall ? 1 : 0),
                (byte)(Ds3AutoPair ? 1 : 0),
                (byte)BtRumble,
                (byte)_speedUpFlags,
                (byte)BackLight,
                Ds4LightbarBrightness,
                (byte)(PartialDs4CrossOver ? 1 : 0),
                (byte)IdleTimeout
            };
        }
    }
}
