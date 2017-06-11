// Copyright © 2016 Dimasin. All rights reserved.

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
        public int BusWidth;
        public List<OneParam> GPUParams;
        GPUType type;
        //Specific parametr for Nvidia GPU
        private readonly NvPhysicalGpuHandle handle;
        private readonly NvDisplayHandle displayHandle;
        //Specific parametr for AMD GPU
        public ADLAdapterInfo adapterInfo;
        private ADLTemperature adlt;
        private ADLFanSpeedInfo afsi;
        private ADLFanSpeedValue adlf;
        private ADLPMActivity adlp;
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
            //BusWidth
            if (NVAPI._NvAPI_GPU_GetCurrentPCIEDownstreamWidth != null)
            {
                uint busWidth = 0;
                if (NVAPI._NvAPI_GPU_GetCurrentPCIEDownstreamWidth(handle, out busWidth) == NvStatus.OK)
                    BusWidth = (int)busWidth;
                else
                    BusWidth = -1;
            }
            //Param GPU
            GPUParams = new List<OneParam>();
            for (int i = 0; i != num; i++)
                GPUParams.Insert(i, new OneParam());
        }
        public GPUParam(ADLAdapterInfo ai, int num)
        {
            adapterInfo = ai;
            adlt = new ADLTemperature();
            afsi = new ADLFanSpeedInfo();
            adlf = new ADLFanSpeedValue();
            adlp = new ADLPMActivity();
            type = GPUType.amd;
            //GPU name
            GPUName = adapterInfo.AdapterName.Trim();
            //Slot
            Slot = adapterInfo.BusNumber;
            //Subsys
            Subsys = adapterInfo.UDID.Substring(adapterInfo.UDID.IndexOf("SUBSYS") + 7, 8);
            //BusWidth
            if (ADL.ADL_Overdrive5_CurrentActivity_Get(adapterInfo.AdapterIndex, ref adlp) == ADL.ADL_OK)
                BusWidth = adlp.CurrentBusLanes;
            else
                BusWidth = -1;
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
                    UpdateNvi();
                    break;
                case GPUType.amd:
                    UpdateAMD();
                    break;
            }
            if (GPUParams[0].ParCollect.Count >= (TickCountMax - 1))
                for (int i = 0; i != GPUParams.Count; i++)
                    GPUParams[i].ParCollect.RemoveAt(0);
        }
        private void UpdateNvi()
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
            }
            GPUParams[4].ParCollect.Add(GPUTemp);
            //Fan speed %, RPM
            NvGPUCoolerSettings coolerSettings = GetCoolerSettings();
            int FANprecentage = 0; int FANrpm = 0;
            if (coolerSettings.Count > 0)
                FANprecentage = coolerSettings.Cooler[0].CurrentLevel;
            GPUParams[5].ParCollect.Add(FANprecentage);
            GPUParams[6].ParCollect.Add(FANrpm);
        }
        private void UpdateAMD()
        {
            //Clock GPU, MEM
            int GPUClock = 0; int MemClock = 0; int GPULoad = 0; int MemLoad = 0;
            if (ADL.ADL_Overdrive5_CurrentActivity_Get(adapterInfo.AdapterIndex, ref adlp) == ADL.ADL_OK)
            {
                if (adlp.EngineClock > 0)
                    GPUClock = adlp.EngineClock / 100;
                if (adlp.MemoryClock > 0)
                    MemClock = adlp.MemoryClock / 100;
                //Load GPU, MEM
                GPULoad = adlp.ActivityPercent;
                MemLoad = 0;
            }
            GPUParams[0].ParCollect.Add(GPUClock);
            GPUParams[1].ParCollect.Add(MemClock);
            GPUParams[2].ParCollect.Add(GPULoad);
            GPUParams[3].ParCollect.Add(MemLoad);
            //Temperature GPU
            int GPUTemp = 0;
            if (ADL.ADL_Overdrive5_Temperature_Get(adapterInfo.AdapterIndex, 0, ref adlt) == ADL.ADL_OK)
                GPUTemp = adlt.Temperature / 1000;
            GPUParams[4].ParCollect.Add(GPUTemp);
            //Fan speed %, RPM
            int FANprecentage = 0; int FANrpm = 0;
            if (ADL.ADL_Overdrive5_FanSpeedInfo_Get(adapterInfo.AdapterIndex, 0, ref afsi) == ADL.ADL_OK)
            {
                adlf.SpeedType = ADL.ADL_DL_FANCTRL_SPEED_TYPE_PERCENT;
                if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterInfo.AdapterIndex, 0, ref adlf) == ADL.ADL_OK)
                    FANprecentage = adlf.FanSpeed;
                adlf.SpeedType = ADL.ADL_DL_FANCTRL_SPEED_TYPE_RPM;
                if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterInfo.AdapterIndex, 0, ref adlf) == ADL.ADL_OK)
                    FANrpm = adlf.FanSpeed;
            }
            GPUParams[5].ParCollect.Add(FANprecentage);
            GPUParams[6].ParCollect.Add(FANrpm);
        }
        private NvGPUThermalSettings GetThermalSettings()
        {
            NvGPUThermalSettings settings = new NvGPUThermalSettings();
            settings.Version = NVAPI.GPU_THERMAL_SETTINGS_VER;
            settings.Count = NVAPI.MAX_THERMAL_SENSORS_PER_GPU;
            settings.Sensor = new NvSensor[NVAPI.MAX_THERMAL_SENSORS_PER_GPU];
            if (!(NVAPI.NvAPI_GPU_GetThermalSettings != null && NVAPI.NvAPI_GPU_GetThermalSettings(handle, (int)NvThermalTarget.ALL, ref settings) == NvStatus.OK))
                settings.Count = 0;
            return settings;
        }
        private NvGPUCoolerSettings GetCoolerSettings()
        {
            NvGPUCoolerSettings settings = new NvGPUCoolerSettings();
            settings.Version = NVAPI.GPU_COOLER_SETTINGS_VER;
            settings.Cooler = new NvCooler[NVAPI.MAX_COOLER_PER_GPU];
            if (!(NVAPI.NvAPI_GPU_GetCoolerSettings != null && NVAPI.NvAPI_GPU_GetCoolerSettings(handle, 0, ref settings) == NvStatus.OK))
                settings.Count = 0;
            return settings;
        }
        private uint[] GetClocks()
        {
            NvClocks allClocks = new NvClocks();
            allClocks.Version = NVAPI.GPU_CLOCKS_VER;
            allClocks.Clock = new uint[NVAPI.MAX_CLOCKS_PER_GPU];
            if (NVAPI.NvAPI_GPU_GetAllClocks != null && NVAPI.NvAPI_GPU_GetAllClocks(handle, ref allClocks) == NvStatus.OK)
                return allClocks.Clock;
            return null;
        }
    }
}
