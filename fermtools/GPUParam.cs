using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fermtools
{
    public enum GPUType { nvi = 1, amd = 2 };
    class OneParam
    {
        public List<int> ParCollect;
        public double Rate;
        public OneParam()
        {
            ParCollect = new List<int>();
        }
    }
    class GPUParam
    {
        //Universal parametr
        public string GPUName;
        public string Subsys;
        public int Slot;
        public List<OneParam> GPUParams;
        GPUType type;
        //Specific parametr GPU
        private readonly NvPhysicalGpuHandle handle;
        private readonly NvDisplayHandle displayHandle;
        //Init Nvidia GPU param
        public GPUParam(NvPhysicalGpuHandle hdl, NvDisplayHandle displ, int num)
        {
            handle = hdl;
            displayHandle = displ;

            type = GPUType.nvi;
            //GPU name
            string tmp;
            if (NVAPI.NvAPI_GPU_GetFullName(handle, out tmp) == NvStatus.OK)
                GPUName = "NVIDIA " + tmp.Trim();
            else
                GPUName = "NVIDIA GPU name not found";
            //Slot
            if (NVAPI.NvAPI_GPU_GetBusId != null)
            {
                uint busId = 0;
                if (NVAPI.NvAPI_GPU_GetBusId(handle, out busId) == NvStatus.OK)
                    Slot = (int)busId;
                else
                    Slot = -1;
            }
            //Subsys
            if (NVAPI.NvAPI_GPU_GetPCIIdentifiers != null)
            {
                uint deviceId, subSystemId, revisionId, extDeviceId;
                NvStatus status = NVAPI.NvAPI_GPU_GetPCIIdentifiers(handle, out deviceId, out subSystemId, out revisionId, out extDeviceId);
                if (status == NvStatus.OK)
                {
                    Subsys = subSystemId.ToString("X", CultureInfo.InvariantCulture);
                    if (Subsys.Length < 8) 
                        Subsys = "0" + Subsys;
                }
            }
            //Param GPU
            GPUParams = new List<OneParam>();
            for (int i = 0; i != num; i++)
                GPUParams.Insert(i, new OneParam());
        }
        public void Update(int TickCountMax)
        {
            switch (type)
            {
                case GPUType.nvi:
                    UpdateNvi(TickCountMax);
                    break;
                case GPUType.amd:
                    break;
            }
        }
        private void UpdateNvi(int TickCountMax)
        {
            //Clock GPU, MEM
            uint[] values = GetClocks();
            int GPUClock = 0; int MemClock = 0;
            if (values != null)
            {
                if (values[30] != 0)
                {
                    GPUClock = (int)values[30]/2000;
                    MemClock = (int)values[8]/2000;
                }
                else
                {
                    GPUClock = (int)values[0]/1000;
                    MemClock = (int)values[8]/1000;
                }
            }
            GPUParams[0].ParCollect.Add(GPUClock);
            GPUParams[1].ParCollect.Add(MemClock);
            //Load GPU, MEM
            NvPStates states = new NvPStates();
            states.Version = NVAPI.GPU_PSTATES_VER;
            states.PStates = new NvPState[NVAPI.MAX_PSTATES_PER_GPU];
            int GPULoad = 0; int MemLoad = 0;
            if (NVAPI.NvAPI_GPU_GetPStates != null && NVAPI.NvAPI_GPU_GetPStates(handle, ref states) == NvStatus.OK)
            {
                if (states.PStates[0].Present)
                {
                    GPULoad = states.PStates[0].Percentage;
                    MemLoad = states.PStates[1].Percentage;
                }
            }
            GPUParams[2].ParCollect.Add(GPULoad);
            GPUParams[3].ParCollect.Add(MemLoad);
            //Temperature GPU
            NvGPUThermalSettings settings = GetThermalSettings();
            int GPUTemp = 0;
            for (int i = 0; i < settings.Count; i++)
            {
                if (settings.Sensor[i].CurrentTemp > 0)
                    GPUTemp = (int)settings.Sensor[i].CurrentTemp;
                else
                    GPUTemp = 0;
            }
            GPUParams[4].ParCollect.Add(GPUTemp);
            //Fan speed %, RPM
            NvGPUCoolerSettings coolerSettings = GetCoolerSettings();
            int FANprecentage = 0; int FANrpm = 0;
            if (coolerSettings.Count > 0)
                FANprecentage = coolerSettings.Cooler[0].CurrentLevel;
            GPUParams[5].ParCollect.Add(FANprecentage);
            GPUParams[6].ParCollect.Add(FANrpm);
            if (GPUParams[0].ParCollect.Count >= (TickCountMax-1))
                for (int i = 0; i != GPUParams.Count; i++)
                    GPUParams[i].ParCollect.RemoveAt(0);
        }
        private NvGPUThermalSettings GetThermalSettings()
        {
            NvGPUThermalSettings settings = new NvGPUThermalSettings();
            settings.Version = NVAPI.GPU_THERMAL_SETTINGS_VER;
            settings.Count = NVAPI.MAX_THERMAL_SENSORS_PER_GPU;
            settings.Sensor = new NvSensor[NVAPI.MAX_THERMAL_SENSORS_PER_GPU];
            if (!(NVAPI.NvAPI_GPU_GetThermalSettings != null && NVAPI.NvAPI_GPU_GetThermalSettings(handle, (int)NvThermalTarget.ALL, ref settings) == NvStatus.OK))
            {
                settings.Count = 0;
            }
            return settings;
        }
        private NvGPUCoolerSettings GetCoolerSettings()
        {
            NvGPUCoolerSettings settings = new NvGPUCoolerSettings();
            settings.Version = NVAPI.GPU_COOLER_SETTINGS_VER;
            settings.Cooler = new NvCooler[NVAPI.MAX_COOLER_PER_GPU];
            if (!(NVAPI.NvAPI_GPU_GetCoolerSettings != null && NVAPI.NvAPI_GPU_GetCoolerSettings(handle, 0, ref settings) == NvStatus.OK))
            {
                settings.Count = 0;
            }
            return settings;
        }
        private uint[] GetClocks()
        {
            NvClocks allClocks = new NvClocks();
            allClocks.Version = NVAPI.GPU_CLOCKS_VER;
            allClocks.Clock = new uint[NVAPI.MAX_CLOCKS_PER_GPU];
            if (NVAPI.NvAPI_GPU_GetAllClocks != null && NVAPI.NvAPI_GPU_GetAllClocks(handle, ref allClocks) == NvStatus.OK)
            {
                return allClocks.Clock;
            }
            return null;
        }
    }
}
