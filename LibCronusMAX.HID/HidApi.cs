using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace LibCronusMAX.HID
{
    internal static class HidApi
    {
        public class HidApiDeviceInfo
        {
            public readonly string DevicePath;

            public readonly int InterfaceNumber = -1;

            public readonly ushort ProductId;

            public readonly ushort ReleaseNumber;

            public readonly ushort Usage;

            public readonly ushort UsagePage;

            public readonly ushort VendorId;

            internal HidApiDeviceInfo(string devicePath, ushort vendorId, ushort productId, ushort releaseNumber)
            {
                DevicePath = devicePath;
                VendorId = vendorId;
                ProductId = productId;
                ReleaseNumber = releaseNumber;
            }

            internal HidApiDeviceInfo(HidApiDeviceInfo nfo, ushort usage, ushort usagePage)
                : this(nfo.DevicePath, nfo.VendorId, nfo.ProductId, nfo.ReleaseNumber)
            {
                Usage = usage;
                UsagePage = usagePage;
            }

            internal HidApiDeviceInfo(HidApiDeviceInfo nfo, int interfaceNumber)
                : this(nfo, nfo.Usage, nfo.UsagePage)
            {
                InterfaceNumber = interfaceNumber;
            }
        }

        public class HidApiDevice
        {
            internal bool Blocking;

            internal SafeFileHandle DeviceHandle;

            internal uint InputReportLength;

            internal int LastErrorNum;

            internal string LastErrorStr;

            internal NativeOverlapped Ol;

            internal byte[] ReadBuf;

            internal bool ReadPending;

            public HidApiDevice()
            {
                Ol = new NativeOverlapped
                {
                    EventHandle = IntPtr.Zero,
                    InternalHigh = IntPtr.Zero,
                    InternalLow = IntPtr.Zero,
                    OffsetHigh = 0,
                    OffsetLow = 0
                };
                Ol.EventHandle = CreateEvent(IntPtr.Zero, bManualReset: false, bInitialState: false, null);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SpDeviceInterfaceDetailData
        {
            public int cbSize;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public readonly string DevicePath;
        }

        private struct HiddAttributes
        {
            public uint Size;

            public readonly ushort VendorID;

            public readonly ushort ProductID;

            public readonly ushort VersionNumber;
        }

        private struct SpDevinfoData
        {
            public uint cbSize;

            public readonly Guid classGuid;

            public readonly uint devInst;

            public readonly IntPtr reserved;
        }

        private struct SpDeviceInterfaceData
        {
            public int cbSize;

            public readonly Guid interfaceClassGuid;

            public readonly int flags;

            public readonly UIntPtr reserved;
        }

        private struct HidpCaps
        {
            public ushort Usage;

            public ushort UsagePage;

            public ushort InputReportByteLength;

            public ushort OutputReportByteLength;

            public ushort FeatureReportByteLength;

            public ushort _reserved0;

            public ushort _reserved1;

            public ushort _reserved2;

            public ushort _reserved3;

            public ushort _reserved4;

            public ushort _reserved5;

            public ushort _reserved6;

            public ushort _reserved7;

            public ushort _reserved8;

            public ushort _reserved9;

            public ushort _reserved10;

            public ushort _reserved11;

            public ushort _reserved12;

            public ushort _reserved13;

            public ushort _reserved14;

            public ushort _reserved15;

            public ushort _reserved16;

            public ushort _fieldsNotUsedByHidapi0;

            public ushort _fieldsNotUsedByHidapi1;

            public ushort _fieldsNotUsedByHidapi2;

            public ushort _fieldsNotUsedByHidapi3;

            public ushort _fieldsNotUsedByHidapi4;

            public ushort _fieldsNotUsedByHidapi5;

            public ushort _fieldsNotUsedByHidapi6;

            public ushort _fieldsNotUsedByHidapi7;

            public ushort _fieldsNotUsedByHidapi8;

            public ushort _fieldsNotUsedByHidapi9;
        }

        private static Guid _interfaceClassGuid = new Guid(1293833650u, 61807, 4559, 136, 203, 0, 17, 17, 0, 0, 48);

        private static readonly List<HidApiDeviceInfo> LastDeviceInfo = new List<HidApiDeviceInfo>();

        private static SafeFileHandle OpenDevice(string path)
        {
            SafeFileHandle handle = CreateFile(path, 3221225472u, 0u, IntPtr.Zero, 3u, 1073741824u, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                handle = CreateFile(path, 3221225472u, 3u, IntPtr.Zero, 3u, 1073741824u, IntPtr.Zero);
            }
            return handle;
        }

        public static HidApiDeviceInfo[] Enumerate(ushort[] vids, ushort[] pids, string[] devpath)
        {
            HidApiDeviceInfo[] prevdevs = LastDeviceInfo.ToArray();
            LastDeviceInfo.Clear();
            SpDevinfoData devinfoData = default(SpDevinfoData);
            SpDeviceInterfaceData deviceInterfaceData = default(SpDeviceInterfaceData);
            SpDeviceInterfaceDetailData deviceInterfaceDetailData = default(SpDeviceInterfaceDetailData);
            int deviceIndex = 0;
            devinfoData.cbSize = (uint)Marshal.SizeOf((object)devinfoData);
            deviceInterfaceData.cbSize = Marshal.SizeOf((object)deviceInterfaceData);
            if (IntPtr.Size == 8)
            {
                deviceInterfaceDetailData.cbSize = 8;
            }
            else
            {
                deviceInterfaceDetailData.cbSize = 4 + Marshal.SystemDefaultCharSize;
            }
            IntPtr deviceInfoSet;
            for (deviceInfoSet = SetupDiGetClassDevs(ref _interfaceClassGuid, null, IntPtr.Zero, 18u); SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref _interfaceClassGuid, (uint)deviceIndex, ref deviceInterfaceData); deviceIndex++)
            {
                uint requiredSize = 0u;
                SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0u, ref requiredSize, IntPtr.Zero);
                if (SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, ref deviceInterfaceDetailData, requiredSize, IntPtr.Zero, IntPtr.Zero) && devpath != null && devpath.Contains(deviceInterfaceDetailData.DevicePath))
                {
                    HidApiDeviceInfo[] array = prevdevs;
                    foreach (HidApiDeviceInfo devinfo in array)
                    {
                        if (!(devinfo.DevicePath != deviceInterfaceDetailData.DevicePath))
                        {
                            LastDeviceInfo.Add(devinfo);
                            break;
                        }
                    }
                }
                SafeFileHandle writeHandle = OpenDevice(deviceInterfaceDetailData.DevicePath);
                if (writeHandle.IsInvalid)
                {
                    continue;
                }
                HiddAttributes attrib = default(HiddAttributes);
                attrib.Size = (uint)Marshal.SizeOf((object)attrib);
                HidD_GetAttributes(writeHandle, ref attrib);
                if ((vids.Length == 0 && pids.Length == 0) || (vids.Contains(attrib.VendorID) && pids.Contains(attrib.ProductID)))
                {
                    HidApiDeviceInfo devinfo2 = new HidApiDeviceInfo(deviceInterfaceDetailData.DevicePath, attrib.VendorID, attrib.ProductID, attrib.VersionNumber);
                    IntPtr ppData = default(IntPtr);
                    if (HidD_GetPreparsedData(writeHandle, ref ppData))
                    {
                        HidpCaps caps = default(HidpCaps);
                        if (HidP_GetCaps(ppData, ref caps) == 1114112)
                        {
                            devinfo2 = new HidApiDeviceInfo(devinfo2, caps.Usage, caps.UsagePage);
                        }
                        HidD_FreePreparsedData(ppData);
                    }
                    int ind = devinfo2.DevicePath.IndexOf("&mi_", StringComparison.Ordinal);
                    if (ind > 0 && int.TryParse(devinfo2.DevicePath.Substring(ind + 4, 4), NumberStyles.HexNumber, null, out int id))
                    {
                        devinfo2 = new HidApiDeviceInfo(devinfo2, id);
                    }
                    if (LastDeviceInfo.All((HidApiDeviceInfo d) => d.DevicePath != devinfo2.DevicePath))
                    {
                        LastDeviceInfo.Add(devinfo2);
                    }
                }
                writeHandle.Close();
            }
            SetupDiDestroyDeviceInfoList(deviceInfoSet);
            return LastDeviceInfo.ToArray();
        }

        public static HidApiDevice Open(string path)
        {
            HidApiDevice ret = new HidApiDevice
            {
                DeviceHandle = OpenDevice(path)
            };
            if (ret.DeviceHandle.IsInvalid)
            {
                ret.LastErrorNum = Marshal.GetLastWin32Error();
                ret.LastErrorStr = new Win32Exception(ret.LastErrorNum).Message;
            }
            else
            {
                IntPtr ppData = default(IntPtr);
                if (HidD_GetPreparsedData(ret.DeviceHandle, ref ppData))
                {
                    HidpCaps caps = default(HidpCaps);
                    if (HidP_GetCaps(ppData, ref caps) == 1114112)
                    {
                        ret.InputReportLength = caps.InputReportByteLength;
                        ret.ReadBuf = new byte[ret.InputReportLength];
                    }
                    else
                    {
                        ret.LastErrorNum = Marshal.GetLastWin32Error();
                        ret.LastErrorStr = new Win32Exception(ret.LastErrorNum).Message;
                    }
                    HidD_FreePreparsedData(ppData);
                }
                else
                {
                    ret.LastErrorNum = Marshal.GetLastWin32Error();
                    ret.LastErrorStr = new Win32Exception(ret.LastErrorNum).Message;
                }
            }
            return ret;
        }

        public static int Write(HidApiDevice dev, byte[] data)
        {
            NativeOverlapped ol = default(NativeOverlapped);
            if (!WriteFile(dev.DeviceHandle, data, (uint)data.Length, out uint bytesWritten, ref ol))
            {
                int err = Marshal.GetLastWin32Error();
                if (err != 997)
                {
                    dev.LastErrorNum = err;
                    dev.LastErrorStr = new Win32Exception(err).Message;
                    return -1;
                }
                if (!GetOverlappedResult(dev.DeviceHandle, ref ol, out bytesWritten, bWait: true))
                {
                    dev.LastErrorNum = err;
                    dev.LastErrorStr = new Win32Exception(err).Message;
                    return -1;
                }
            }
            return (int)bytesWritten;
        }

        public static int Read(HidApiDevice dev, out byte[] data, int milliseconds)
        {
            data = null;
            uint bytesRead;
            if (!dev.ReadPending)
            {
                dev.ReadPending = true;
                ResetEvent(dev.Ol.EventHandle);
                if (!ReadFile(dev.DeviceHandle, dev.ReadBuf, dev.InputReportLength, out bytesRead, ref dev.Ol))
                {
                    int err = Marshal.GetLastWin32Error();
                    if (err != 997)
                    {
                        dev.LastErrorNum = err;
                        dev.LastErrorStr = new Win32Exception(err).Message;
                        CancelIo(dev.DeviceHandle);
                        dev.ReadPending = false;
                        return -1;
                    }
                }
            }
            if (milliseconds >= 0 && WaitForSingleObject(dev.Ol.EventHandle, milliseconds) != 0)
            {
                return 0;
            }
            if (GetOverlappedResult(dev.DeviceHandle, ref dev.Ol, out bytesRead, bWait: true) && bytesRead != 0)
            {
                if (dev.ReadBuf[0] == 0)
                {
                    bytesRead--;
                    data = new byte[dev.InputReportLength];
                    Array.Copy(dev.ReadBuf, 1L, data, 0L, bytesRead);
                }
                else
                {
                    data = dev.ReadBuf;
                }
            }
            dev.ReadPending = false;
            return (int)bytesRead;
        }

        public static int Read(HidApiDevice dev, out byte[] data)
        {
            return Read(dev, out data, dev.Blocking ? (-1) : 0);
        }

        public static int SetNonblocking(HidApiDevice dev, bool nonblock)
        {
            dev.Blocking = !nonblock;
            return 0;
        }

        public static string GetError(HidApiDevice dev)
        {
            return dev.LastErrorStr;
        }

        public static int GetInputLength(HidApiDevice dev)
        {
            return (int)dev.InputReportLength;
        }

        public static bool Flush(HidApiDevice dev)
        {
            return HidD_FlushQueue(dev.DeviceHandle);
        }

        public static void Close(HidApiDevice dev)
        {
            CancelIo(dev.DeviceHandle);
            CloseHandle(dev.Ol.EventHandle);
            dev.DeviceHandle.Close();
        }

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool HidD_GetAttributes(SafeFileHandle deviceObject, ref HiddAttributes attributes);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool HidD_FreePreparsedData(IntPtr preparsedData);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool HidD_GetPreparsedData(SafeFileHandle handle, ref IntPtr preparsedData);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint HidP_GetCaps(IntPtr preparsedData, ref HidpCaps caps);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool HidD_FlushQueue(SafeFileHandle handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CancelIo(SafeFileHandle hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, [In] ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ReadFile(SafeFileHandle hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [In] ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetOverlappedResult(SafeFileHandle hFile, [In] ref NativeOverlapped lpOverlapped, out uint lpNumberOfBytesTransferred, bool bWait);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ResetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int WaitForSingleObject(IntPtr handle, int wait);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, [MarshalAs(UnmanagedType.LPTStr)] string enumerator, IntPtr hwndParent, uint flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SpDeviceInterfaceData deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SpDeviceInterfaceData deviceInterfaceData, ref SpDeviceInterfaceDetailData deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, IntPtr requiredSize, IntPtr deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SpDeviceInterfaceData deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, ref uint requiredSize, IntPtr deviceInfoData);
    }
}
