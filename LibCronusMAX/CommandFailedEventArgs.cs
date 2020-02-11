using System;

namespace LibCronusMAX
{
    public class CommandFailedEventArgs : EventArgs
    {
        /// <summary>
        ///     Command types
        /// </summary>
        public enum Commands
        {
            /// <summary>
            ///     Request I/O Status
            /// </summary>
            RequestIoStatus,
            /// <summary>
            ///     Load GPC Script
            /// </summary>
            LoadScript,
            /// <summary>
            ///     API Mode Buffer (CmCommand/CmCommandEx)
            /// </summary>
            ApiModeBuffer,
            /// <summary>
            ///     Request Device Settings
            /// </summary>
            RequestDeviceSettings,
            /// <summary>
            ///     Save Device Settings
            /// </summary>
            SaveDeviceSettings,
            /// <summary>
            ///     Enter API Mode (CronusMAX Plus only accepts CmCommand/CmCommandEx while in this mode)
            /// </summary>
            EnterApiMode,
            /// <summary>
            ///     Exit API Mode
            /// </summary>
            ExitApiMode,
            /// <summary>
            ///     Unload currently loaded GPC Script
            /// </summary>
            UnloadGpc,
            /// <summary>
            ///     Change device slot to +1 from current (or first if on the last)
            /// </summary>
            ChangeSlot,
            /// <summary>
            ///     Turn off the connected controller
            /// </summary>
            TurnOffController,
            /// <summary>
            ///     Get device information (Firmware version etc.)
            /// </summary>
            GetDeviceInfo
        }

        /// <summary>
        ///     Reasons as to why the failure occured
        /// </summary>
        public enum FailureReasons
        {
            /// <summary>
            ///     The device is no longer connected
            /// </summary>
            DeviceNotConnected,
            /// <summary>
            ///     There was an error while attempting to send the command
            /// </summary>
            FailedToSendCommand,
            /// <summary>
            ///     There was an error while attempting to read the result of the command
            /// </summary>
            FailedToReadResult,
            /// <summary>
            ///     This command requires API Mode to be active in order to function
            /// </summary>
            NeedApiMode
        }

        /// <summary>
        ///     Command that failed
        /// </summary>
        public readonly Commands Command;

        /// <summary>
        ///     Reason why the command failed
        /// </summary>
        public readonly FailureReasons Reason;

        internal CommandFailedEventArgs(FailureReasons reason, SingleDevice.CommandObject.Commands cmd)
        {
            Reason = reason;
            switch (cmd)
            {
                case SingleDevice.CommandObject.Commands.RequestIoStatus:
                    Command = Commands.RequestIoStatus;
                    break;
                case SingleDevice.CommandObject.Commands.RunScript:
                    Command = Commands.LoadScript;
                    break;
                case SingleDevice.CommandObject.Commands.ApiModeB:
                case SingleDevice.CommandObject.Commands.ApiMode:
                    Command = Commands.ApiModeBuffer;
                    break;
                case SingleDevice.CommandObject.Commands.GetDeviceSettings:
                    Command = Commands.RequestDeviceSettings;
                    break;
                case SingleDevice.CommandObject.Commands.SetDeviceSettings:
                    Command = Commands.SaveDeviceSettings;
                    break;
                case SingleDevice.CommandObject.Commands.EnterApiMode:
                    Command = Commands.EnterApiMode;
                    break;
                case SingleDevice.CommandObject.Commands.ExitApiMode:
                    Command = Commands.ExitApiMode;
                    break;
                case SingleDevice.CommandObject.Commands.UnloadGpc:
                    Command = Commands.UnloadGpc;
                    break;
                case SingleDevice.CommandObject.Commands.ChangeSlotA:
                case SingleDevice.CommandObject.Commands.ChangeSlotB:
                    Command = Commands.ChangeSlot;
                    break;
                case SingleDevice.CommandObject.Commands.TurnOffController:
                    Command = Commands.TurnOffController;
                    break;
                case SingleDevice.CommandObject.Commands.GetFw:
                case SingleDevice.CommandObject.Commands.GetSerial:
                    Command = Commands.GetDeviceInfo;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("cmd", cmd, null);
            }
        }
    }
}
