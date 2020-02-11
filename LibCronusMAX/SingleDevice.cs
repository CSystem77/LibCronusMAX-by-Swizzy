using LibCronusMAX.HID;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace LibCronusMAX
{
    /// <summary>
    ///     Class used to communicate with a CronusMAX Plus device
    /// </summary>
    public sealed class SingleDevice : IDisposable
    {
        internal class CommandObject
        {
            public enum Commands : byte
            {
                RequestIoStatus = 2,
                RunScript = 3,
                ApiModeB = 4,
                ApiMode = 132,
                GetDeviceSettings = 5,
                SetDeviceSettings = 6,
                EnterApiMode = 7,
                ExitApiMode = 8,
                UnloadGpc = 9,
                ChangeSlotA = 10,
                ChangeSlotB = 11,
                TurnOffController = 12,
                GetFw = 240,
                GetSerial = 241
            }

            internal readonly byte[] Args;

            internal readonly Commands Command;

            internal readonly bool IsContinued;

            internal readonly int ReadCount;

            public bool Retry;

            internal int Size = -1;

            public CommandObject(Commands command, int readCount = 0, byte[] args = null, bool isContinued = false)
            {
                Command = command;
                Args = args;
                IsContinued = isContinued;
                ReadCount = readCount;
            }

            public byte[] GetBuffer()
            {
                byte[] ret = new byte[65];
                ret[1] = (byte)Command;
                if (Args != null)
                {
                    if (Size <= 0)
                    {
                        Size = Args.Length;
                    }
                    ret[2] = (byte)(Size & 0xFF);
                    ret[3] = (byte)((Size & 0xFF00) >> 8);
                    Array.Copy(Args, 0, ret, 5, Math.Min(Args.Length, ret.Length - 5));
                    if (!IsContinued)
                    {
                        ret[4] = 1;
                    }
                }
                else if (!IsContinued)
                {
                    ret[4] = 1;
                }
                return ret;
            }
        }

        private readonly BlockingCollection<CommandObject> commandQueue = new BlockingCollection<CommandObject>();

        private readonly BackgroundWorker deviceWorker = new BackgroundWorker
        {
            WorkerSupportsCancellation = true
        };

        private readonly ManualResetEvent statusSuspendEvent = new ManualResetEvent(initialState: true);

        private readonly BackgroundWorker statusWorker = new BackgroundWorker
        {
            WorkerSupportsCancellation = true
        };

        private readonly ManualResetEvent workerSuspendEvent = new ManualResetEvent(initialState: true);

        /// <summary>
        ///     Delegate called when a command failed to be processed
        /// </summary>
        public EventHandler<CommandFailedEventArgs> CommandFailed;

        private Hid.DeviceType currentDevice;

        /// <summary>
        ///     Delegate called when the device information changed
        /// </summary>
        public EventHandler<DeviceInformation> DeviceInformationChanged;

        /// <summary>
        ///     Delegate called when the device settings changed
        /// </summary>
        public EventHandler<DeviceSettings> DeviceSettingsChanged;

        private bool hasStarted;

        private Hid hidLibrary;

        /// <summary>
        ///     Delegate called when the device information changed
        /// </summary>
        public EventHandler<IOStatus> IOStatusChanged;

        private double lastApiModeB;

        private DeviceInformation lastInfo = new DeviceInformation();

        private IOStatus lastIoStatus;

        private DeviceSettings lastSettings;

        private bool disposed;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public SingleDevice()
        {
            statusWorker.DoWork += StatusWorker_DoWork;
            deviceWorker.DoWork += DeviceWorker_DoWork;
        }

        /// <summary>
        ///     Function used to start the worker threads
        /// </summary>
        public void StartWorkerThreads()
        {
            if (!hasStarted)
            {
                try
                {
                    hidLibrary = new Hid();
                    currentDevice = Hid.DeviceType.None;
                    statusWorker.RunWorkerAsync();
                    deviceWorker.RunWorkerAsync();
                    hasStarted = true;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     Function used to stop the worker threads
        /// </summary>
        public void StopWorkerThreads()
        {
            if (hasStarted)
            {
                statusWorker.CancelAsync();
                deviceWorker.CancelAsync();
                currentDevice = Hid.DeviceType.None;
                hasStarted = false;
            }
        }

        /// <summary>
        ///     Gets the last received device information
        /// </summary>
        /// <returns>Last device information</returns>
        public DeviceInformation GetLastDeviceInfo()
        {
            return lastInfo;
        }

        /// <summary>
        ///     Gets the last received device settings
        /// </summary>
        /// <returns>Last device settings</returns>
        public DeviceSettings GetLastSettings()
        {
            return lastSettings;
        }

        /// <summary>
        ///     Function used to request the current device settings
        /// </summary>
        public void RequestSettings()
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation != null && deviceInformation.State == DeviceInformation.States.Connected)
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.GetDeviceSettings, 1));
            }
            else
            {
                SendFailedNotConnected(CommandObject.Commands.GetDeviceSettings);
            }
        }

        /// <summary>
        ///     Function used to save new device settings
        /// </summary>
        /// <param name="settings">Settings to save to the CronusMAX Plus device</param>
        public void SaveSettings(DeviceSettings settings)
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation != null && deviceInformation.State == DeviceInformation.States.Connected && settings != null)
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.SetDeviceSettings, 0, settings.ToByteArray()));
            }
            else
            {
                SendFailedNotConnected(CommandObject.Commands.SetDeviceSettings);
            }
        }

        /// <summary>
        ///     Function used to enter API Mode
        /// </summary>
        public void EnterApiMode()
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation != null && deviceInformation.State == DeviceInformation.States.Connected)
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.EnterApiMode));
                return;
            }
            DeviceInformation deviceInformation2 = lastInfo;
            if (deviceInformation2 == null || deviceInformation2.State != DeviceInformation.States.ApiMode)
            {
                SendFailedNotConnected(CommandObject.Commands.EnterApiMode);
            }
        }

        /// <summary>
        ///     Function used to exit API Mode
        /// </summary>
        public void ExitApiMode()
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation != null && deviceInformation.State == DeviceInformation.States.ApiMode)
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.ExitApiMode));
                return;
            }
            DeviceInformation deviceInformation2 = lastInfo;
            if (deviceInformation2 == null || deviceInformation2.State != DeviceInformation.States.Connected)
            {
                SendFailedNotConnected(CommandObject.Commands.ExitApiMode);
            }
        }

        /// <summary>
        ///     Function used to request the current I/O status
        /// </summary>
        public void RequestIoStatus()
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation == null || deviceInformation.State != DeviceInformation.States.Connected)
            {
                SendFailedNotConnected(CommandObject.Commands.RequestIoStatus);
            }
            else if (commandQueue.All((CommandObject t) => t.Command != CommandObject.Commands.RequestIoStatus))
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.RequestIoStatus, 2));
            }
        }

        /// <summary>
        ///     Function used to unload the current GPC Script/Slot
        /// </summary>
        public void UnloadGpc()
        {
            commandQueue.Add(new CommandObject(CommandObject.Commands.UnloadGpc));
        }

        /// <summary>
        ///     Function used to load a compiled GPC Script
        /// </summary>
        /// <param name="bytecode">Compiled GPC Script to load</param>
        /// <remarks>bytecode must be atleast 2 bytes and may not be bigger then 4096 bytes</remarks>
        /// <returns>True if there was no errors while preparing the packets to send</returns>
        public bool LoadGpc(byte[] bytecode)
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation == null || deviceInformation.State != DeviceInformation.States.Connected)
            {
                SendFailedNotConnected(CommandObject.Commands.RunScript);
                return false;
            }
            if (bytecode == null)
            {
                return false;
            }
            if (bytecode.Length < 2 || bytecode.Length > 4096)
            {
                return false;
            }
            for (int i = 0; i < bytecode.Length; i += 60)
            {
                byte[] tmp = new byte[Math.Min(60, bytecode.Length - i)];
                Array.Copy(bytecode, i, tmp, 0, tmp.Length);
                commandQueue.Add(new CommandObject(CommandObject.Commands.RunScript, 0, tmp, i > 0)
                {
                    Size = bytecode.Length
                });
            }
            return true;
        }

        /// <summary>
        ///     Function used to load a compiled GPC Script
        /// </summary>
        /// <param name="filename">Compiled GPC Script to load from the filesystem</param>
        /// <remarks>bytecode must be atleast 2 bytes and may not be bigger then 4096 bytes</remarks>
        /// <returns>True if there was no errors while preparing the packets to send</returns>
        public bool LoadGpc(string filename)
        {
            try
            {
                FileInfo info = new FileInfo(filename);
                if (info.Length < 2 || info.Length > 4096)
                {
                    return false;
                }
                return LoadGpc(File.ReadAllBytes(filename));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Function used to switch to the next slot in the sequence
        /// </summary>
        public void ChangeSlot()
        {
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation == null || deviceInformation.State != DeviceInformation.States.Connected)
            {
                SendFailedNotConnected(CommandObject.Commands.ChangeSlotA);
                return;
            }
            commandQueue.Add(new CommandObject(CommandObject.Commands.ChangeSlotA));
            commandQueue.Add(new CommandObject(CommandObject.Commands.ChangeSlotB));
        }

        /// <summary>
        ///     Function used to send a CmCommand buffer to the CronusMAX Plus
        /// </summary>
        /// <param name="buffer">Outputs to send</param>
        /// <returns>True if the command was successfully added to the queue</returns>
        public bool SendApiModeData(CmCommand buffer)
        {
            if (!CheckApiState(buffer))
            {
                return false;
            }
            AddApiModeBuffer(buffer.ToByteArray());
            return true;
        }

        /// <summary>
        ///     Function used to send a CmCommandEx buffer to the CronusMAX Plus
        /// </summary>
        /// <param name="buffer">Outputs and flags to send</param>
        /// <returns>True if the command was successfully added to the queue</returns>
        public bool SendApiModeData(CmCommandEx buffer)
        {
            if (!CheckApiState(buffer))
            {
                return false;
            }
            AddApiModeBuffer(buffer.ToByteArray());
            return true;
        }

        /// <summary>
        ///     Function used to tell the CronusMAX Plus to turn off the currently connected controller
        /// </summary>
        public void TurnOffController()
        {
            commandQueue.Add(new CommandObject(CommandObject.Commands.TurnOffController));
        }

        private bool CheckApiState(object buffer)
        {
            if (buffer == null)
            {
                return false;
            }
            DeviceInformation deviceInformation = lastInfo;
            if (deviceInformation == null || deviceInformation.State != DeviceInformation.States.ApiMode)
            {
                SendFailedNeedApiMode(CommandObject.Commands.ApiMode);
                return false;
            }
            return true;
        }

        private void SendFailedToSend(CommandObject.Commands cmd)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                CommandFailed?.Invoke(this, new CommandFailedEventArgs(CommandFailedEventArgs.FailureReasons.FailedToSendCommand, cmd));
            });
        }

        private void SendFailedToRead(CommandObject.Commands cmd)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                CommandFailed?.Invoke(this, new CommandFailedEventArgs(CommandFailedEventArgs.FailureReasons.FailedToReadResult, cmd));
            });
        }

        private void SendFailedNotConnected(CommandObject.Commands cmd)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                CommandFailed?.Invoke(this, new CommandFailedEventArgs(CommandFailedEventArgs.FailureReasons.DeviceNotConnected, cmd));
            });
        }

        private void SendFailedNeedApiMode(CommandObject.Commands cmd)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                CommandFailed?.Invoke(this, new CommandFailedEventArgs(CommandFailedEventArgs.FailureReasons.NeedApiMode, cmd));
            });
        }

        private void DeviceWorker_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (!doWorkEventArgs.Cancel)
            {
                try
                {
                    CommandObject cmd = commandQueue.Take();
                    if (!hidLibrary.IsConnected)
                    {
                        SendFailedNotConnected(cmd.Command);
                    }
                    else
                    {
                        workerSuspendEvent.WaitOne();
                        statusSuspendEvent.Reset();
                        if (hidLibrary.WriteToDevice(cmd.GetBuffer()))
                        {
                            if (cmd.ReadCount > 0)
                            {
                                for (int i = 0; i < cmd.ReadCount; i++)
                                {
                                    byte[] data = hidLibrary.ReadFromDevice();
                                    if (data == null)
                                    {
                                        SendFailedToRead(cmd.Command);
                                        break;
                                    }
                                    HandleCommandRead(data);
                                    if (cmd.Command != CommandObject.Commands.ApiMode && cmd.Command != CommandObject.Commands.ApiModeB && cmd.Command != CommandObject.Commands.RequestIoStatus)
                                    {
                                        int cnt = 10;
                                        while ((uint)data[0] != (uint)cmd.Command && cnt > 0)
                                        {
                                            data = hidLibrary.ReadFromDevice();
                                            if (data != null)
                                            {
                                                HandleCommandRead(data);
                                            }
                                            if (data == null)
                                            {
                                                break;
                                            }
                                            cnt--;
                                        }
                                        if (data == null || (uint)data[0] == (uint)cmd.Command || cnt != 0 || cmd.Retry)
                                        {
                                            SendFailedToRead(cmd.Command);
                                        }
                                        else
                                        {
                                            cmd.Retry = true;
                                            commandQueue.Add(cmd);
                                        }
                                    }
                                }
                            }
                            if (cmd.Command == CommandObject.Commands.ExitApiMode)
                            {
                                hidLibrary.FlushInputs();
                                Thread.Sleep(50);
                                hidLibrary.FlushInputs();
                                DeviceInformation deviceInformation = lastInfo;
                                if (deviceInformation != null && deviceInformation.State == DeviceInformation.States.ApiMode)
                                {
                                    lastInfo = new DeviceInformation(DeviceInformation.States.Connected, lastInfo.Fw, lastInfo.IsHubCompatible, lastInfo.OperationalMode);
                                }
                                ThreadPool.QueueUserWorkItem(delegate
                                {
                                    DeviceInformationChanged?.Invoke(null, lastInfo);
                                });
                            }
                            else if (cmd.Command == CommandObject.Commands.EnterApiMode)
                            {
                                lastInfo = new DeviceInformation(DeviceInformation.States.ApiMode, lastInfo.Fw, lastInfo.IsHubCompatible, lastInfo.OperationalMode);
                                ThreadPool.QueueUserWorkItem(delegate
                                {
                                    DeviceInformationChanged?.Invoke(null, lastInfo);
                                });
                            }
                        }
                        else
                        {
                            SendFailedToSend(cmd.Command);
                        }
                        statusSuspendEvent.Set();
                    }
                }
                catch
                {
                }
            }
        }

        private void HandleCommandRead(byte[] d)
        {
            if (d[0] == 240 && d[3] == 1)
            {
                DeviceInformation.OperationalModes opmode = DeviceInformation.OperationalModes.Standard;
                switch (d[7])
                {
                    case byte.MaxValue:
                        opmode = DeviceInformation.OperationalModes.TournamentEdition;
                        break;
                    case 1:
                        opmode = DeviceInformation.OperationalModes.WheelEdition;
                        break;
                }
                DeviceInformation.States state2 = (lastInfo.SerialFwVersion != 0) ? DeviceInformation.States.Connected : DeviceInformation.States.Updating;
                bool isHubCompatible2 = lastInfo.IsHubCompatible;
                if (lastInfo.SerialFwVersion != 0)
                {
                    isHubCompatible2 = (lastInfo.SerialFwVersion == (int)Math.Ceiling((double)(int)d[6] / 1.6));
                }
                lastInfo = new DeviceInformation(state2, new Version((int)Math.Ceiling((double)(int)d[6] / 1.6), (int)Math.Ceiling((double)(int)d[4] / 1.6)), isHubCompatible2, opmode);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    DeviceInformationChanged?.Invoke(null, lastInfo);
                });
            }
            else if (d[0] == 241 && d[3] == 1)
            {
                byte v = (byte)Math.Ceiling((double)(int)d[29] / 1.6);
                if (v == 0)
                {
                    v = byte.MaxValue;
                }
                bool isHubCompatible = false;
                if (lastInfo.Fw != null)
                {
                    isHubCompatible = (lastInfo.Fw.Minor == v);
                }
                DeviceInformation.States state = (lastInfo.Fw != null) ? DeviceInformation.States.Connected : DeviceInformation.States.Updating;
                lastInfo = new DeviceInformation(state, lastInfo.Fw, isHubCompatible, lastInfo.OperationalMode);
                if (lastInfo.Fw == null)
                {
                    lastInfo.SerialFwVersion = v;
                }
                ThreadPool.QueueUserWorkItem(delegate
                {
                    DeviceInformationChanged?.Invoke(null, lastInfo);
                });
            }
            else if (d[0] == 5 && d[1] == 11 && d[2] == 0 && d[3] == 1)
            {
                byte[] tmp = new byte[11];
                Array.Copy(d, 4, tmp, 0, tmp.Length);
                lastSettings = new DeviceSettings(tmp);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    DeviceSettingsChanged?.Invoke(null, lastSettings);
                });
            }
            else if (d[0] == 1 && d[1] == 46 && d[2] == 0 && d[3] == 1)
            {
                lastIoStatus = IOStatus.GetIOStatusFromInput(d, lastIoStatus);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    IOStatusChanged?.Invoke(null, lastIoStatus);
                });
            }
            else if (d[0] == 2 && d[1] == 36 && d[2] == 0 && d[3] == 1)
            {
                lastIoStatus = IOStatus.GetIOStatusFromOutput(d, lastIoStatus);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    IOStatusChanged?.Invoke(null, lastIoStatus);
                });
            }
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (!doWorkEventArgs.Cancel)
            {
                statusSuspendEvent.WaitOne();
                workerSuspendEvent.Reset();
                hidLibrary.FindDevice(Hid.DeviceType.Normal);
                workerSuspendEvent.Set();
                HandleDeviceChanged(currentDevice);
                Thread.Sleep(100);
            }
        }

        private void HandleDeviceChanged(Hid.DeviceType last)
        {
            if (hidLibrary.IsConnected)
            {
                Hid.DeviceType current = hidLibrary.ConnectedDevice;
                if (last == current)
                {
                    return;
                }
                currentDevice = current;
                if (current == Hid.DeviceType.Normal)
                {
                    commandQueue.Add(new CommandObject(CommandObject.Commands.ExitApiMode));
                    commandQueue.Add(new CommandObject(CommandObject.Commands.GetFw, 1));
                    commandQueue.Add(new CommandObject(CommandObject.Commands.GetSerial, 1));
                }
                lastInfo = new DeviceInformation(DeviceInformation.States.Updating);
            }
            else
            {
                currentDevice = Hid.DeviceType.None;
                lastInfo = new DeviceInformation();
            }
            ThreadPool.QueueUserWorkItem(delegate
            {
                DeviceInformationChanged?.Invoke(null, lastInfo);
            });
        }

        private void AddApiModeBuffer(byte[] buffer)
        {
            double ts = HelperFunctions.GetTimeStamp();
            if (lastApiModeB == 0.0 || ts - lastApiModeB >= 0.00122)
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.ApiModeB, 2, buffer));
                lastApiModeB = ts;
            }
            else
            {
                commandQueue.Add(new CommandObject(CommandObject.Commands.ApiMode, 2, buffer));
            }
        }

        public void Dispose(bool disposing)
        {
            try
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        StopWorkerThreads();
                    }
                    disposed = true;
                }
            }
            catch
            {
            }
        }

        ~SingleDevice()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
