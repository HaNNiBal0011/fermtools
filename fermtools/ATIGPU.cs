/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2014 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

using System;
using System.Globalization;

namespace fermtools
{
    internal struct AtiGPUInfo
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
        public int Slot;
    }

  internal sealed class ATIGPU {

    private readonly int adapterIndex;
    private readonly int busNumber;
    private readonly int deviceNumber;
    ADLTemperature adlt;
    ADLFanSpeedInfo afsi;
    ADLFanSpeedValue adlf;
    ADLPMActivity adlp;
    public AtiGPUInfo gpuinfo;

    public ATIGPU(ADLAdapterInfo adapterInfo) 
    {
        this.adapterIndex = adapterInfo.AdapterIndex;
        this.busNumber = adapterInfo.BusNumber;
        this.deviceNumber = adapterInfo.DeviceNumber;

      adlt = new ADLTemperature();
      afsi = new ADLFanSpeedInfo();
      adlf = new ADLFanSpeedValue();
      adlp = new ADLPMActivity();
      gpuinfo = new AtiGPUInfo();
      gpuinfo.GPUName = adapterInfo.AdapterName.Trim();
      gpuinfo.Slot = adapterInfo.BusNumber;
      gpuinfo.Subsys = adapterInfo.UDID.Substring(adapterInfo.UDID.IndexOf("SUBSYS") + 7, 8);
        
      Update();                   
    }

    public int BusNumber { get { return busNumber; } }

    public int DeviceNumber { get { return deviceNumber; } }


    public void Update() 
    {
      gpuinfo.CoreClock = "-"; gpuinfo.MemoryClock = "-"; gpuinfo.GPULoad = "-"; gpuinfo.MemCtrlLoad = "-";
      gpuinfo.GPUTemp = "-"; gpuinfo.FanLoad = "-"; gpuinfo.FanRPM = "-";

      if (ADL.ADL_Overdrive5_Temperature_Get(adapterIndex, 0, ref adlt) == ADL.ADL_OK) 
      {
          gpuinfo.GPUTemp = (adlt.Temperature/1000).ToString();
      }

      if (ADL.ADL_Overdrive5_FanSpeedInfo_Get(adapterIndex, 0, ref afsi) == ADL.ADL_OK)
      {
          adlf.SpeedType = ADL.ADL_DL_FANCTRL_SPEED_TYPE_PERCENT;
          if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref adlf) == ADL.ADL_OK)
          {
              gpuinfo.FanLoad = adlf.FanSpeed.ToString();
          }
          adlf.SpeedType = ADL.ADL_DL_FANCTRL_SPEED_TYPE_RPM;
          if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref adlf) == ADL.ADL_OK)
          {
              gpuinfo.FanRPM = adlf.FanSpeed.ToString();
          }
      }

      if (ADL.ADL_Overdrive5_CurrentActivity_Get(adapterIndex, ref adlp) == ADL.ADL_OK) 
      {
        if (adlp.EngineClock > 0) 
        {
            gpuinfo.CoreClock = (adlp.EngineClock/100).ToString();
        }
        if (adlp.MemoryClock > 0) 
        {
            gpuinfo.MemoryClock = (adlp.MemoryClock/100).ToString();
        }

        if (adlp.Vddc > 0) 
        {
        }
        gpuinfo.GPULoad = adlp.ActivityPercent.ToString();
        //gpuinfo.MemCtrlLoad = adlp.CurrentPerformanceLevel.ToString();
      }
    }
  }
}
