using System;

namespace LibCronusMAX.HID
{
    internal class HidDevice : IDisposable
    {
        private readonly HidApi.HidApiDevice _dev;

        public readonly string DevicePath;

        public readonly Hid.DeviceType DeviceType;

        public readonly ushort Pid;

        public readonly ushort Vid;

        private bool _disposed;

        internal HidDevice(string devicePath, ushort vid, ushort pid, HidApi.HidApiDevice device, Hid.DeviceType deviceType)
        {
            _dev = device;
            DeviceType = deviceType;
            Vid = vid;
            Pid = pid;
            DevicePath = devicePath;
        }

        public byte[] Read(int timeout = 1000)
        {
            if (HidApi.Read(_dev, out byte[] data, timeout) <= 0)
            {
                return null;
            }
            return data;
        }

        public bool Write(byte[] data)
        {
            return HidApi.Write(_dev, data) == data.Length;
        }

        public string GetLastError()
        {
            return HidApi.GetError(_dev) ?? "Success";
        }

        public bool FlushInputs()
        {
            return HidApi.Flush(_dev);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        HidApi.Close(_dev);
                    }
                    _disposed = true;
                }
            }
            catch
            {
            }
        }

        ~HidDevice()
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
