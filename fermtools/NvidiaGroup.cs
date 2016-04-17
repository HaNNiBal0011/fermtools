/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2011 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace fermtools
{
  internal class NvidiaGroup 
  {
    public readonly StringBuilder report = new StringBuilder();
    public NvidiaGroup(ref List<GPUParam> gpupar, int numpar)
    {
      if (!NVAPI.IsAvailable) 
          return;
      report.AppendLine("NVAPI");
      report.AppendLine();
      string version;
      if (NVAPI.NvAPI_GetInterfaceVersionString(out version) == NvStatus.OK) 
      {
        report.Append("Version: ");
        report.AppendLine(version);
      }
      NvPhysicalGpuHandle[] handles = new NvPhysicalGpuHandle[NVAPI.MAX_PHYSICAL_GPUS];
      int count;
      if (NVAPI.NvAPI_EnumPhysicalGPUs == null) 
      {
        report.AppendLine("Error: NvAPI_EnumPhysicalGPUs not available");
        report.AppendLine();
        return;
      } 
      else 
      {        
        NvStatus status = NVAPI.NvAPI_EnumPhysicalGPUs(handles, out count);
        if (status != NvStatus.OK) 
        {
          report.AppendLine("Status: " + status);
          report.AppendLine();
          return;
        }
      }
      IDictionary<NvPhysicalGpuHandle, NvDisplayHandle> displayHandles = new Dictionary<NvPhysicalGpuHandle, NvDisplayHandle>();
      if (NVAPI.NvAPI_EnumNvidiaDisplayHandle != null && NVAPI.NvAPI_GetPhysicalGPUsFromDisplay != null) 
      {
        NvStatus status = NvStatus.OK;
        int i = 0;
        while (status == NvStatus.OK) 
        {
          NvDisplayHandle displayHandle = new NvDisplayHandle();
          status = NVAPI.NvAPI_EnumNvidiaDisplayHandle(i, ref displayHandle);
          i++;
          if (status == NvStatus.OK) 
          {
            NvPhysicalGpuHandle[] handlesFromDisplay = new NvPhysicalGpuHandle[NVAPI.MAX_PHYSICAL_GPUS];
            uint countFromDisplay;
            if (NVAPI.NvAPI_GetPhysicalGPUsFromDisplay(displayHandle, handlesFromDisplay, out countFromDisplay) == NvStatus.OK) 
            {
                for (int j = 0; j < countFromDisplay; j++)
                {
                    if (!displayHandles.ContainsKey(handlesFromDisplay[j]))
                        displayHandles.Add(handlesFromDisplay[j], displayHandle);
                }
            }
          }
        }
      }
      report.Append("Number of GPUs: ");
      report.AppendLine(count.ToString(CultureInfo.InvariantCulture));
      for (int i = 0; i < count; i++) 
      {
        NvDisplayHandle displayHandle;
        displayHandles.TryGetValue(handles[i], out displayHandle);
          //ДОбавляем карту в коллекцию
        gpupar.Add(new GPUParam(handles[i],displayHandle, numpar));
          //Добавляем информацию о карте в отчет
        report.AppendLine(GetReportCard(i, handles[i], displayHandle));
      }
      report.AppendLine();
    }

      private string GetReportCard(int adapterIndex, NvPhysicalGpuHandle handle, NvDisplayHandle displayHandle)
    {
        StringBuilder r = new StringBuilder();
        r.AppendLine("Nvidia GPU");
        r.AppendLine();
        string tmp;
        if (NVAPI.NvAPI_GPU_GetFullName(handle, out tmp) == NvStatus.OK)
            tmp = "NVIDIA " + tmp.Trim();
        else
            tmp = "NVIDIA GPU name not found";
        r.AppendFormat("Name: {0}{1}", tmp, Environment.NewLine);
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
                    r.AppendFormat(" Sensor[{0}].Controller: {1}{2}", i, settings.Sensor[i].Controller, Environment.NewLine);
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
                    r.AppendFormat(" Cooler[{0}].Type: {1}{2}", i, settings.Cooler[i].Type, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].Controller: {1}{2}", i, settings.Cooler[i].Controller, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].DefaultMin: {1}{2}", i, settings.Cooler[i].DefaultMin, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].DefaultMax: {1}{2}", i, settings.Cooler[i].DefaultMax, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].CurrentMin: {1}{2}", i, settings.Cooler[i].CurrentMin, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].CurrentMax: {1}{2}", i, settings.Cooler[i].CurrentMax, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].CurrentLevel: {1}{2}", i, settings.Cooler[i].CurrentLevel, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].DefaultPolicy: {1}{2}", i, settings.Cooler[i].DefaultPolicy, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].CurrentPolicy: {1}{2}", i, settings.Cooler[i].CurrentPolicy, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].Target: {1}{2}", i, settings.Cooler[i].Target, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].ControlType: {1}{2}", i, settings.Cooler[i].ControlType, Environment.NewLine);
                    r.AppendFormat(" Cooler[{0}].Active: {1}{2}", i, settings.Cooler[i].Active, Environment.NewLine);
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
     public string GetReport() 
     {
         return report.ToString();
     }
  }
}
