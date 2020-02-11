using System.Collections.Generic;
using System.Linq;

namespace LibCronusMAX.HID
{
    internal class Hid
    {
        public enum DeviceType
        {
            None = -1,
            Any,
            Normal,
            Bootloader
        }

        private const ushort CmNormalVid = 8200;

        private const ushort CmNormalPid = 1;

        private const ushort CmNormalUsage = 512;

        private const ushort CmNormalUsagePage = 65451;

        private const ushort CmBlVid = 9480;

        private const ushort CmBlPid = 32769;

        private const ushort CmBlUsage = 28;

        private const ushort CmBlUsagePage = 65436;

        private readonly List<HidDevice> _devices = new List<HidDevice>();

        private HidDevice _dev;

        public bool IsConnected => ConnectedDevice != DeviceType.None;

        public DeviceType ConnectedDevice => _dev?.DeviceType ?? DeviceType.None;

        public void FindDevice(DeviceType expectedDevice = DeviceType.Any)
        {
            HidDevice[] devs2;
            if (_dev != null && _dev.DeviceType == expectedDevice)
            {
                devs2 = FindDevices(expectedDevice);
                if (devs2.Contains(_dev))
                {
                    return;
                }
            }
            devs2 = FindDevices(expectedDevice);
            _dev = ((devs2.Length != 0) ? devs2[0] : null);
        }

        public HidDevice[] FindDevices(DeviceType expectedDevices = DeviceType.Any)
        {
            List<HidDevice> ret = new List<HidDevice>();
            HidDevice[] oldDevs = _devices.ToArray();
            _devices.Clear();
            HidApi.HidApiDeviceInfo[] devnfo = HidApi.Enumerate(new ushort[2]
            {
                8200,
                9480
            }, new ushort[2]
            {
                1,
                32769
            }, oldDevs.Select((HidDevice dev) => dev.DevicePath).ToArray());
            HidDevice[] devs2 = GetNormalDevices(oldDevs, devnfo);
            _devices.AddRange(devs2);
            if (expectedDevices == DeviceType.Any || expectedDevices == DeviceType.Normal)
            {
                ret.AddRange(devs2);
            }
            devs2 = GetBootloaderDevices(oldDevs, devnfo);
            _devices.AddRange(devs2);
            if (expectedDevices == DeviceType.Any || expectedDevices == DeviceType.Bootloader)
            {
                ret.AddRange(devs2);
            }
            return ret.ToArray();
        }

        private static HidDevice[] GetBootloaderDevices(HidDevice[] oldDevs, IEnumerable<HidApi.HidApiDeviceInfo> devnfo)
        {
            return (from nfo in devnfo
                    where nfo.VendorId == 9480
                    where nfo.ProductId == 32769
                    where nfo.Usage == 28
                    where nfo.UsagePage == 65436
                    select oldDevs.FirstOrDefault((HidDevice d) => d.DevicePath == nfo.DevicePath) ?? new HidDevice(nfo.DevicePath, nfo.VendorId, nfo.ProductId, HidApi.Open(nfo.DevicePath), DeviceType.Bootloader)).ToArray();
        }

        private static HidDevice[] GetNormalDevices(HidDevice[] oldDevs, IEnumerable<HidApi.HidApiDeviceInfo> devnfo)
        {
            return (from nfo in devnfo
                    where nfo.VendorId == 8200
                    where nfo.ProductId == 1
                    where nfo.Usage == 512
                    where nfo.UsagePage == 65451
                    select oldDevs.FirstOrDefault((HidDevice d) => d.DevicePath == nfo.DevicePath) ?? new HidDevice(nfo.DevicePath, nfo.VendorId, nfo.ProductId, HidApi.Open(nfo.DevicePath), DeviceType.Normal)).ToArray();
        }

        public byte[] ReadFromDevice(int timeout = 1000)
        {
            return _dev?.Read(timeout);
        }

        public bool WriteToDevice(byte[] data)
        {
            if (_dev != null)
            {
                return _dev.Write(data);
            }
            return false;
        }

        public string GetLastError()
        {
            return _dev?.GetLastError() ?? "Success";
        }

        public bool FlushInputs()
        {
            if (_dev != null)
            {
                return _dev.FlushInputs();
            }
            return false;
        }
    }
}
