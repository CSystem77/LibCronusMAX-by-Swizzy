using System;

namespace LibCronusMAX
{
    public class DeviceInformation : EventArgs
    {
        /// <summary>
        ///     Operational mode flags
        /// </summary>
        public enum OperationalModes
        {
            /// <summary>
            ///     Unknown, quite self explanatory - we currently don't know what operational mode this device is in
            /// </summary>
            Unknown,
            /// <summary>
            ///     Standard edition firmware mode is currently being used
            /// </summary>
            Standard,
            /// <summary>
            ///     Tournament Edition mode is currently being used
            /// </summary>
            TournamentEdition,
            /// <summary>
            ///     PS4 Wheel Edition mode is currently being used
            /// </summary>
            WheelEdition
        }

        /// <summary>
        ///     Device states
        /// </summary>
        public enum States
        {
            /// <summary>
            ///     Device is disconnected
            /// </summary>
            Disconnected,
            /// <summary>
            ///     Device is connected and is NOT in API Mode
            /// </summary>
            Connected,
            /// <summary>
            ///     Device is connected and is in API Mode
            /// </summary>
            ApiMode,
            /// <summary>
            ///     Device is connected and currently updating the device information
            /// </summary>
            Updating
        }

        internal byte SerialFwVersion;

        /// <summary>
        ///     Current device state
        /// </summary>
        public States State
        {
            get;
        }

        private string Serial
        {
            get;
        }

        /// <summary>
        ///     Device Firmware Version
        /// </summary>
        public Version Fw
        {
            get;
        }

        /// <summary>
        ///     Device Firmware Operational Mode
        /// </summary>
        public OperationalModes OperationalMode
        {
            get;
        }

        /// <summary>
        ///     A flag that can be used to determine if the connected device is HUB Compatible
        /// </summary>
        public bool IsHubCompatible
        {
            get;
        }

        internal DeviceInformation()
        {
            OperationalMode = OperationalModes.Unknown;
            State = States.Disconnected;
        }

        internal DeviceInformation(States state, string serial, Version fw = null, bool isHubCompatible = false, OperationalModes operationalMode = OperationalModes.Unknown)
        {
            State = state;
            Serial = serial;
            Fw = fw;
            OperationalMode = operationalMode;
            IsHubCompatible = isHubCompatible;
        }

        internal DeviceInformation(States state, Version fw = null, bool isHubCompatible = false, OperationalModes operationalMode = OperationalModes.Unknown)
        {
            State = state;
            Fw = fw;
            OperationalMode = operationalMode;
            IsHubCompatible = isHubCompatible;
        }
    }
}
