using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace fermtools
{
    class SetupDi
    {    
        internal struct SetupGpuInfo
        {
            string GPUName;
            string HardwareID;
            string InstanceID;

        }
        internal enum DiGetClassFlags
        {
            DIGCF_DEFAULT = 0x00000001,
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010
        }
        internal enum RegPropertyType
        {
            SPDRP_DEVICEDESC = 0x00000000, // DeviceDesc (R/W)
            SPDRP_HARDWAREID = 0x00000001, // HardwareID (R/W)
            SPDRP_COMPATIBLEIDS = 0x00000002, // CompatibleIDs (R/W)
            SPDRP_UNUSED0 = 0x00000003, // unused
            SPDRP_SERVICE = 0x00000004, // Service (R/W)
            SPDRP_UNUSED1 = 0x00000005, // unused
            SPDRP_UNUSED2 = 0x00000006, // unused
            SPDRP_CLASS = 0x00000007, // Class (R--tied to ClassGUID)
            SPDRP_CLASSGUID = 0x00000008, // ClassGUID (R/W)
            SPDRP_DRIVER = 0x00000009, // Driver (R/W)
            SPDRP_CONFIGFLAGS = 0x0000000A, // ConfigFlags (R/W)
            SPDRP_MFG = 0x0000000B, // Mfg (R/W)
            SPDRP_FRIENDLYNAME = 0x0000000C, // FriendlyName (R/W)
            SPDRP_LOCATION_INFORMATION = 0x0000000D, // LocationInformation (R/W)
            SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E, // PhysicalDeviceObjectName (R)
            SPDRP_CAPABILITIES = 0x0000000F, // Capabilities (R)
            SPDRP_UI_NUMBER = 0x00000010, // UiNumber (R)
            SPDRP_UPPERFILTERS = 0x00000011, // UpperFilters (R/W)
            SPDRP_LOWERFILTERS = 0x00000012, // LowerFilters (R/W)
            SPDRP_BUSTYPEGUID = 0x00000013, // BusTypeGUID (R)
            SPDRP_LEGACYBUSTYPE = 0x00000014, // LegacyBusType (R)
            SPDRP_BUSNUMBER = 0x00000015, // BusNumber (R)
            SPDRP_ENUMERATOR_NAME = 0x00000016, // Enumerator Name (R)
            SPDRP_SECURITY = 0x00000017, // Security (R/W, binary form)
            SPDRP_SECURITY_SDS = 0x00000018, // Security (W, SDS form)
            SPDRP_DEVTYPE = 0x00000019, // Device Type (R/W)
            SPDRP_EXCLUSIVE = 0x0000001A, // Device is exclusive-access (R/W)
            SPDRP_CHARACTERISTICS = 0x0000001B, // Device Characteristics (R/W)
            SPDRP_ADDRESS = 0x0000001C, // Device Address (R)
            SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D, // UiNumberDescFormat (R/W)
            SPDRP_DEVICE_POWER_DATA = 0x0000001E, // Device Power Data (R)
            SPDRP_REMOVAL_POLICY = 0x0000001F, // Removal Policy (R)
            SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020, // Hardware Removal Policy (R)
            SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021, // Removal Policy Override (RW)
            SPDRP_INSTALL_STATE = 0x00000022, // Device Install State (R)
            SPDRP_LOCATION_PATHS = 0x00000023, // Device Location Paths (R)
            SPDRP_BASE_CONTAINERID = 0x00000024  // Base ContainerID (R)
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid lpGuid, IntPtr Enumerator, IntPtr hwndParent, DiGetClassFlags Flags);
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, RegPropertyType Property, out UInt32 PropertyRegDataType, IntPtr PropertyBuffer, int PropertyBufferSize, out UInt32 RequiredSize);
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, int i, out UInt32 PropertyRegDataType, IntPtr PropertyBuffer, int PropertyBufferSize, out UInt32 RequiredSize);
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, RegPropertyType Property, out UInt32 PropertyRegDataType, byte[] PropertyBuffer, int PropertyBufferSize, out UInt32 RequiredSize);
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out UInt32 RequiredSize);

        SetupDi()
        {
            Guid guid = new Guid("{4d36e968-e325-11ce-bfc1-08002be10318}");
            IntPtr PnPHandle = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, DiGetClassFlags.DIGCF_PRESENT);
            int BUFFER_SIZE = 1024;
            bool result = true; int DeviceIndex = 0;
            IntPtr ptrBuf = Marshal.AllocHGlobal(BUFFER_SIZE);
            UInt32 RequiredSize; UInt32 RegType;

            while (result)
            {
                SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();
                DeviceInfoData.cbSize = Marshal.SizeOf(DeviceInfoData);
                result = SetupDiEnumDeviceInfo(PnPHandle, DeviceIndex, ref DeviceInfoData);
                if (result)
                {
                    //Название видеокарты
                    if (SetupDiGetDeviceRegistryProperty(PnPHandle, ref DeviceInfoData, RegPropertyType.SPDRP_DEVICEDESC, out RegType, ptrBuf, BUFFER_SIZE, out RequiredSize))
                        Console.WriteLine(Marshal.PtrToStringAuto(ptrBuf));
                    //PCIID с дополнительными полями
                    StringBuilder sb = new StringBuilder(BUFFER_SIZE);
                    if (SetupDiGetDeviceInstanceId(PnPHandle, ref DeviceInfoData, sb, BUFFER_SIZE, out RequiredSize))
                        Console.WriteLine(sb.ToString());
                    //Расположение на шине
                    if (SetupDiGetDeviceRegistryProperty(PnPHandle, ref DeviceInfoData, RegPropertyType.SPDRP_LOCATION_INFORMATION, out RegType, ptrBuf, BUFFER_SIZE, out RequiredSize))
                        Console.WriteLine(Marshal.PtrToStringAuto(ptrBuf));
                }
                DeviceIndex++;
            }

        }
    }
}
