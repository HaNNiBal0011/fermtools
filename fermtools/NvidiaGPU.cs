/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2014 Michael Möller <mmoeller@openhardwaremonitor.org>
	Copyright (C) 2011 Christian Vallières
 
*/

using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace fermtools
{
    internal struct NviGPUInfo
    {
        public string GPUName;
        public string CoreClock;
        public string MemoryClock;
        public string GPULoad;
        public string MemCtrlLoad;
        public string GPUTemp;
        public string FanLoad;
        public string FanRPM;
        public string Subsys;
        public uint Slot;
    }

    internal struct NviGPUInfoNum
    {
        public uint CoreClock;
        public uint MemoryClock;
        public uint GPULoad;
        public uint MemCtrlLoad;
        public uint GPUTemp;
        public uint FanLoad;
        public uint FanRPM;
    }

    internal class NviGPUStat
    {
        public double CoreClock;
        public double SCoreClock;
        public List<uint> LCoreClock;
        public double MemoryClock;
        public double SMemoryClock;
        public List<uint> LMemoryClock;
        public double GPULoad;
        public double SGPULoad;
        public List<uint> LGPULoad;
        public double MemCtrlLoad;
        public double SMemCtrlLoad;
        public List<uint> LMemCtrlLoad;
        public double GPUTemp;
        public double SGPUTemp;
        public List<uint> LGPUTemp;
        public double FanLoad;
        public double SFanLoad;
        public List<uint> LFanLoad;
        public double FanRPM;
        public double SFanRPM;
        public List<uint> LFanRPM;
        public uint TickCount;

        public NviGPUStat()
        {
            CoreClock = 0; SCoreClock = 0; LCoreClock = new List<uint>();
            MemoryClock = 0; SMemoryClock = 0; LMemoryClock = new List<uint>();
            GPULoad = 0; SGPULoad = 0; LGPULoad = new List<uint>();
            MemCtrlLoad = 0; SMemCtrlLoad = 0; LMemCtrlLoad = new List<uint>();
            GPUTemp = 0; SGPUTemp = 0; LGPUTemp = new List<uint>();
            FanLoad = 0; SFanLoad = 0; LFanLoad = new List<uint>();
            FanRPM = 0; SFanRPM = 0; LFanRPM = new List<uint>();
            TickCount = 0;
            
        }
    }

  internal class NvidiaGPU 
  {

    private readonly int adapterIndex;
    private readonly NvPhysicalGpuHandle handle;
    private readonly NvDisplayHandle displayHandle;
    public NviGPUInfo gpuinfo;
    public NviGPUStat gpustat;
    public NviGPUInfoNum gpucurr;
    public int statcount;


    public NvidiaGPU(int adapterIndex, NvPhysicalGpuHandle handl, NvDisplayHandle displayHandle)
    {
      this.adapterIndex = adapterIndex;
      this.handle = handl;
      this.displayHandle = displayHandle;

      gpustat = new NviGPUStat();
      gpucurr = new NviGPUInfoNum();
      gpuinfo = new NviGPUInfo();
      gpuinfo.GPUName = GetName();

      if (NVAPI.NvAPI_GPU_GetBusId != null)
      {
        uint busId = 0;
        NvStatus status = NVAPI.NvAPI_GPU_GetBusId(handle, out busId);
        if (status == NvStatus.OK)
        {
            gpuinfo.Slot = busId;
        }
      }
      if (NVAPI.NvAPI_GPU_GetPCIIdentifiers != null) 
      {
        uint deviceId, subSystemId, revisionId, extDeviceId;
        NvStatus status = NVAPI.NvAPI_GPU_GetPCIIdentifiers(handle, out deviceId, out subSystemId, out revisionId, out extDeviceId);
        if (status == NvStatus.OK)
        {
            gpuinfo.Subsys = subSystemId.ToString("X", CultureInfo.InvariantCulture);
            if (gpuinfo.Subsys.Length < 8) gpuinfo.Subsys = "0" + gpuinfo.Subsys;
        }
      }
      //Переменная задает время сбора статистики, при цикле 1 сек статистика будет формироваться за последние 5 минут.
      statcount = 60;
      Update();
    }

    private string GetName() 
    {
      string gpuName;
      if (NVAPI.NvAPI_GPU_GetFullName(handle, out gpuName) == NvStatus.OK) return "NVIDIA " + gpuName.Trim();
      else return "NVIDIA GPU name not found";
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

    private void UpdateCurr()
    {
        NvGPUThermalSettings settings = GetThermalSettings();
        for (int i = 0; i < settings.Count; i++)
        {
            if (settings.Sensor[i].CurrentTemp > 0) gpucurr.GPUTemp = settings.Sensor[i].CurrentTemp;
            else gpucurr.GPUTemp = 0;
        }

        uint[] values = GetClocks();
        if (values != null)
        {
            gpucurr.CoreClock = (values[0] / 1000);
            gpucurr.MemoryClock = (values[8] / 1000);
            if (values[30] != 0)
            {
                //GPU Clock values[30]/2 GPU Memory Clock values[8]/2
                gpucurr.CoreClock = (values[30] / 2000);
                gpucurr.MemoryClock = (values[8] / 2000);
            }
        }
        else
        {
            gpucurr.CoreClock = 0; 
            gpucurr.MemoryClock = 0;
        }

        NvPStates states = new NvPStates();
        states.Version = NVAPI.GPU_PSTATES_VER;
        states.PStates = new NvPState[NVAPI.MAX_PSTATES_PER_GPU];
        if (NVAPI.NvAPI_GPU_GetPStates != null && NVAPI.NvAPI_GPU_GetPStates(handle, ref states) == NvStatus.OK)
        {
            if (states.PStates[0].Present)
            {
                //states.PStates[0] GPU Load states.PStates[1] Memory controller load
                gpucurr.GPULoad = (uint)states.PStates[0].Percentage;
                gpucurr.MemCtrlLoad = (uint)states.PStates[1].Percentage;
            }
            else
            {
                gpucurr.GPULoad = 0; 
                gpucurr.MemCtrlLoad = 0;
            }
        }
        else
        {
            NvUsages usages = new NvUsages();
            usages.Version = NVAPI.GPU_USAGES_VER;
            usages.Usage = new uint[NVAPI.MAX_USAGES_PER_GPU];
            if (NVAPI.NvAPI_GPU_GetUsages != null && NVAPI.NvAPI_GPU_GetUsages(handle, ref usages) == NvStatus.OK)
            {
            }
        }

        NvGPUCoolerSettings coolerSettings = GetCoolerSettings();
        if (coolerSettings.Count > 0)
        {
            gpucurr.FanLoad = (uint)coolerSettings.Cooler[0].CurrentLevel;
        }
        else
        {
            gpucurr.FanLoad = 0; 
        }

        gpucurr.FanRPM = 0;

        NvMemoryInfo memoryInfo = new NvMemoryInfo();
        memoryInfo.Version = NVAPI.GPU_MEMORY_INFO_VER;
        memoryInfo.Values = new uint[NVAPI.MAX_MEMORY_VALUES_PER_GPU];
        if (NVAPI.NvAPI_GPU_GetMemoryInfo != null && NVAPI.NvAPI_GPU_GetMemoryInfo(displayHandle, ref memoryInfo) == NvStatus.OK)
        {
            uint totalMemory = memoryInfo.Values[0];
            uint freeMemory = memoryInfo.Values[4];
            float usedMemory = Math.Max(totalMemory - freeMemory, 0);
        }

    }

    private void UpdateStat()
    {
        //Усредняем значения за время statcount (начало нужно чтобы были корректные данные если времени прошло меньше, чем statcount)
        gpustat.LCoreClock.Add(gpucurr.CoreClock);
        gpustat.LMemoryClock.Add(gpucurr.MemoryClock);
        gpustat.LGPULoad.Add(gpucurr.GPULoad);
        gpustat.LMemCtrlLoad.Add(gpucurr.MemCtrlLoad);
        gpustat.LGPUTemp.Add(gpucurr.GPUTemp);
        gpustat.LFanLoad.Add(gpucurr.FanLoad);
        gpustat.LFanRPM.Add(gpucurr.FanRPM);
        if (gpustat.LCoreClock.Count != statcount)
        {
            gpustat.SCoreClock += gpucurr.CoreClock;
            gpustat.SMemoryClock += gpucurr.MemoryClock;
            gpustat.SGPULoad += gpucurr.GPULoad;
            gpustat.SMemCtrlLoad += gpucurr.MemCtrlLoad;
            gpustat.SGPUTemp += gpucurr.GPUTemp;
            gpustat.SFanLoad += gpucurr.FanLoad;
            gpustat.SFanRPM += gpucurr.FanRPM;
            gpustat.TickCount++;
        }
        else
        {
            gpustat.SCoreClock += (gpucurr.CoreClock - (double)gpustat.LCoreClock[0]);
            gpustat.LCoreClock.RemoveAt(0);
            gpustat.SMemoryClock += (gpucurr.MemoryClock - (double)gpustat.LMemoryClock[0]);
            gpustat.LMemoryClock.RemoveAt(0);
            gpustat.SGPULoad += (gpucurr.GPULoad - (double)gpustat.LGPULoad[0]);
            gpustat.LGPULoad.RemoveAt(0);
            gpustat.SMemCtrlLoad += (gpucurr.MemCtrlLoad - (double)gpustat.LMemCtrlLoad[0]);
            gpustat.LMemCtrlLoad.RemoveAt(0);
            gpustat.SGPUTemp += (gpucurr.GPUTemp - (double)gpustat.LGPUTemp[0]);
            gpustat.LGPUTemp.RemoveAt(0);
            gpustat.SFanLoad += (gpucurr.FanLoad - (double)gpustat.LFanLoad[0]);
            gpustat.LFanLoad.RemoveAt(0);
            gpustat.SFanRPM += (gpucurr.FanRPM - (double)gpustat.LFanRPM[0]);
            gpustat.LFanRPM.RemoveAt(0);
        }
        gpustat.CoreClock = gpustat.SCoreClock / (double)gpustat.TickCount;
        gpustat.MemoryClock = gpustat.SMemoryClock / (double)gpustat.TickCount;
        gpustat.GPULoad = gpustat.SGPULoad / (double)gpustat.TickCount;
        gpustat.MemCtrlLoad = gpustat.SMemCtrlLoad / (double)gpustat.TickCount;
        gpustat.GPUTemp = gpustat.SGPUTemp / (double)gpustat.TickCount;
        gpustat.FanLoad = gpustat.SFanLoad / (double)gpustat.TickCount;
        gpustat.FanRPM = gpustat.SFanRPM / (double)gpustat.TickCount;
    }

    public void Update()
    {
        UpdateCurr();
        UpdateStat();
        if (gpucurr.CoreClock > 0) gpuinfo.CoreClock = gpucurr.CoreClock.ToString();
        else gpuinfo.CoreClock = "-";
        if (gpucurr.MemoryClock > 0) gpuinfo.MemoryClock = gpucurr.MemoryClock.ToString();
        else gpuinfo.MemoryClock = "-";
        if (gpucurr.GPULoad > 0) gpuinfo.GPULoad = gpucurr.GPULoad.ToString();
        else gpuinfo.GPULoad = "-";
        if (gpucurr.MemCtrlLoad > 0) gpuinfo.MemCtrlLoad = gpucurr.MemCtrlLoad.ToString();
        else gpuinfo.MemCtrlLoad = "-";
        if (gpucurr.GPUTemp > 0) gpuinfo.GPUTemp = gpucurr.GPUTemp.ToString();
        else gpuinfo.GPUTemp = "-";
        if (gpucurr.FanLoad > 0) gpuinfo.FanLoad = gpucurr.FanLoad.ToString();
        else gpuinfo.FanLoad = "-";
        if (gpucurr.FanRPM > 0) gpuinfo.FanRPM = gpucurr.FanRPM.ToString();
        else gpuinfo.FanRPM = "-";
    }

    public string GetReport() 
    {
      StringBuilder r = new StringBuilder();
      r.AppendLine("Nvidia GPU");
      r.AppendLine();
      r.AppendFormat("Name: {0}{1}", gpuinfo.GPUName, Environment.NewLine);
      r.AppendFormat("Index: {0}{1}", adapterIndex, Environment.NewLine);
      if (NVAPI.NvAPI_GetDisplayDriverVersion != null) 
      {
        NvDisplayDriverVersion driverVersion = new NvDisplayDriverVersion();
        driverVersion.Version = NVAPI.DISPLAY_DRIVER_VERSION_VER;
        if (NVAPI.NvAPI_GetDisplayDriverVersion(displayHandle, ref driverVersion) == NvStatus.OK) 
        {
          r.Append("Driver Version: ");
          r.Append(driverVersion.DriverVersion / 100);
          r.Append(".");
          r.Append((driverVersion.DriverVersion % 100).ToString("00", CultureInfo.InvariantCulture));
          r.AppendLine();
          r.Append("Driver Branch: ");
          r.AppendLine(driverVersion.BuildBranch);
        }
      }
      r.AppendLine();   

      if (NVAPI.NvAPI_GPU_GetPCIIdentifiers != null) 
      {
        uint deviceId, subSystemId, revisionId, extDeviceId;
        NvStatus status = NVAPI.NvAPI_GPU_GetPCIIdentifiers(handle, out deviceId, out subSystemId, out revisionId, out extDeviceId);
        if (status == NvStatus.OK) 
        {
          r.Append("DeviceID: 0x");
          r.AppendLine(deviceId.ToString("X", CultureInfo.InvariantCulture));
          r.Append("SubSystemID: 0x");
          r.AppendLine(subSystemId.ToString("X", CultureInfo.InvariantCulture));
          r.Append("RevisionID: 0x");
          r.AppendLine(revisionId.ToString("X", CultureInfo.InvariantCulture));
          r.Append("ExtDeviceID: 0x");
          r.AppendLine(extDeviceId.ToString("X", CultureInfo.InvariantCulture));
          r.AppendLine();
        }
      }

      if (NVAPI.NvAPI_GPU_GetThermalSettings != null) 
      {
        NvGPUThermalSettings settings = new NvGPUThermalSettings();
        settings.Version = NVAPI.GPU_THERMAL_SETTINGS_VER;
        settings.Count = NVAPI.MAX_THERMAL_SENSORS_PER_GPU;
        settings.Sensor = new NvSensor[NVAPI.MAX_THERMAL_SENSORS_PER_GPU];

        NvStatus status = NVAPI.NvAPI_GPU_GetThermalSettings(handle, (int)NvThermalTarget.ALL, ref settings);

        r.AppendLine("Thermal Settings");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          for (int i = 0; i < settings.Count; i++) 
          {
            r.AppendFormat(" Sensor[{0}].Controller: {1}{2}", i,settings.Sensor[i].Controller, Environment.NewLine);
            r.AppendFormat(" Sensor[{0}].DefaultMinTemp: {1}{2}", i, settings.Sensor[i].DefaultMinTemp, Environment.NewLine);
            r.AppendFormat(" Sensor[{0}].DefaultMaxTemp: {1}{2}", i, settings.Sensor[i].DefaultMaxTemp, Environment.NewLine);
            r.AppendFormat(" Sensor[{0}].CurrentTemp: {1}{2}", i, settings.Sensor[i].CurrentTemp, Environment.NewLine);
            r.AppendFormat(" Sensor[{0}].Target: {1}{2}", i, settings.Sensor[i].Target, Environment.NewLine);
          }
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      if (NVAPI.NvAPI_GPU_GetAllClocks != null) 
      {
        NvClocks allClocks = new NvClocks();
        allClocks.Version = NVAPI.GPU_CLOCKS_VER;
        allClocks.Clock = new uint[NVAPI.MAX_CLOCKS_PER_GPU];
        NvStatus status = NVAPI.NvAPI_GPU_GetAllClocks(handle, ref allClocks);

        r.AppendLine("Clocks");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          for (int i = 0; i < allClocks.Clock.Length; i++)
            if (allClocks.Clock[i] > 0) r.AppendFormat(" Clock[{0}]: {1}{2}", i, allClocks.Clock[i], Environment.NewLine);
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      if (NVAPI.NvAPI_GPU_GetTachReading != null) 
      {
        int tachValue;
        NvStatus status = NVAPI.NvAPI_GPU_GetTachReading(handle, out tachValue);

        r.AppendLine("Tachometer");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          r.AppendFormat(" Value: {0}{1}", tachValue, Environment.NewLine);
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      if (NVAPI.NvAPI_GPU_GetPStates != null)
      {
        NvPStates states = new NvPStates();
        states.Version = NVAPI.GPU_PSTATES_VER;
        states.PStates = new NvPState[NVAPI.MAX_PSTATES_PER_GPU];
        NvStatus status = NVAPI.NvAPI_GPU_GetPStates(handle, ref states);

        r.AppendLine("P-States");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          for (int i = 0; i < states.PStates.Length; i++)
            if (states.PStates[i].Present) r.AppendFormat(" Percentage[{0}]: {1}{2}", i, states.PStates[i].Percentage, Environment.NewLine);
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      if (NVAPI.NvAPI_GPU_GetUsages != null) 
      {
        NvUsages usages = new NvUsages();
        usages.Version = NVAPI.GPU_USAGES_VER;
        usages.Usage = new uint[NVAPI.MAX_USAGES_PER_GPU];
        NvStatus status = NVAPI.NvAPI_GPU_GetUsages(handle, ref usages);

        r.AppendLine("Usages");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          for (int i = 0; i < usages.Usage.Length; i++)
            if (usages.Usage[i] > 0) r.AppendFormat(" Usage[{0}]: {1}{2}", i, usages.Usage[i], Environment.NewLine);
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      if (NVAPI.NvAPI_GPU_GetCoolerSettings != null) 
      {
        NvGPUCoolerSettings settings = new NvGPUCoolerSettings();
        settings.Version = NVAPI.GPU_COOLER_SETTINGS_VER;
        settings.Cooler = new NvCooler[NVAPI.MAX_COOLER_PER_GPU];
        NvStatus status = NVAPI.NvAPI_GPU_GetCoolerSettings(handle, 0, ref settings);

        r.AppendLine("Cooler Settings");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          for (int i = 0; i < settings.Count; i++) 
          {
            r.AppendFormat(" Cooler[{0}].Type: {1}{2}", i,settings.Cooler[i].Type, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].Controller: {1}{2}", i,settings.Cooler[i].Controller, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].DefaultMin: {1}{2}", i,settings.Cooler[i].DefaultMin, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].DefaultMax: {1}{2}", i,settings.Cooler[i].DefaultMax, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].CurrentMin: {1}{2}", i,settings.Cooler[i].CurrentMin, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].CurrentMax: {1}{2}", i,settings.Cooler[i].CurrentMax, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].CurrentLevel: {1}{2}", i,settings.Cooler[i].CurrentLevel, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].DefaultPolicy: {1}{2}", i,settings.Cooler[i].DefaultPolicy, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].CurrentPolicy: {1}{2}", i,settings.Cooler[i].CurrentPolicy, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].Target: {1}{2}", i,settings.Cooler[i].Target, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].ControlType: {1}{2}", i,settings.Cooler[i].ControlType, Environment.NewLine);
            r.AppendFormat(" Cooler[{0}].Active: {1}{2}", i,settings.Cooler[i].Active, Environment.NewLine);
          }
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      if (NVAPI.NvAPI_GPU_GetMemoryInfo != null) 
      {
        NvMemoryInfo memoryInfo = new NvMemoryInfo();
        memoryInfo.Version = NVAPI.GPU_MEMORY_INFO_VER;
        memoryInfo.Values = new uint[NVAPI.MAX_MEMORY_VALUES_PER_GPU];
        NvStatus status = NVAPI.NvAPI_GPU_GetMemoryInfo(displayHandle, ref memoryInfo);

        r.AppendLine("Memory Info");
        r.AppendLine();
        if (status == NvStatus.OK) 
        {
          for (int i = 0; i < memoryInfo.Values.Length; i++) 
              r.AppendFormat(" Value[{0}]: {1}{2}", i, memoryInfo.Values[i], Environment.NewLine);
        } 
        else 
        {
          r.Append(" Status: ");
          r.AppendLine(status.ToString());
        }
        r.AppendLine();
      }

      return r.ToString();
    }

    private void SetDefaultFanSpeed() 
    {
      NvGPUCoolerLevels coolerLevels = new NvGPUCoolerLevels();
      coolerLevels.Version = NVAPI.GPU_COOLER_LEVELS_VER;
      coolerLevels.Levels = new NvLevel[NVAPI.MAX_COOLER_PER_GPU];
      coolerLevels.Levels[0].Policy = 0x20;
      NVAPI.NvAPI_GPU_SetCoolerLevels(handle, 0, ref coolerLevels);
    }

    public void Close() 
    {
          SetDefaultFanSpeed();
    }
  }
}
