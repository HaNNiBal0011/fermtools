/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2014 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

using System;
using System.Globalization;
using System.Collections.Generic;

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

    internal struct AtiGPUInfoNum
    {
        public int CoreClock;
        public int MemoryClock;
        public int GPULoad;
        public int MemCtrlLoad;
        public int GPUTemp;
        public int FanLoad;
        public int FanRPM;
    }

    internal class AtiGPUStat
    {
        public double CoreClock;
        public double SCoreClock;
        public List<int> LCoreClock;
        public double MemoryClock;
        public double SMemoryClock;
        public List<int> LMemoryClock;
        public double GPULoad;
        public double SGPULoad;
        public List<int> LGPULoad;
        public double MemCtrlLoad;
        public double SMemCtrlLoad;
        public List<int> LMemCtrlLoad;
        public double GPUTemp;
        public double SGPUTemp;
        public List<int> LGPUTemp;
        public double FanLoad;
        public double SFanLoad;
        public List<int> LFanLoad;
        public double FanRPM;
        public double SFanRPM;
        public List<int> LFanRPM;
        public uint TickCount;

        public AtiGPUStat()
        {
            CoreClock = 0; SCoreClock = 0; LCoreClock = new List<int>();
            MemoryClock = 0; SMemoryClock = 0; LMemoryClock = new List<int>();
            GPULoad = 0; SGPULoad = 0; LGPULoad = new List<int>();
            MemCtrlLoad = 0; SMemCtrlLoad = 0; LMemCtrlLoad = new List<int>();
            GPUTemp = 0; SGPUTemp = 0; LGPUTemp = new List<int>();
            FanLoad = 0; SFanLoad = 0; LFanLoad = new List<int>();
            FanRPM = 0; SFanRPM = 0; LFanRPM = new List<int>();
            TickCount = 0;

        }
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
    public AtiGPUStat gpustat;
    public AtiGPUInfoNum gpucurr;
    public int statcount;

    public ATIGPU(ADLAdapterInfo adapterInfo) 
    {
        this.adapterIndex = adapterInfo.AdapterIndex;
        this.busNumber = adapterInfo.BusNumber;
        this.deviceNumber = adapterInfo.DeviceNumber;

      adlt = new ADLTemperature();
      afsi = new ADLFanSpeedInfo();
      adlf = new ADLFanSpeedValue();
      adlp = new ADLPMActivity();
      gpucurr = new AtiGPUInfoNum();
      gpustat = new AtiGPUStat();
      gpuinfo = new AtiGPUInfo();
      gpuinfo.GPUName = adapterInfo.AdapterName.Trim();
      gpuinfo.Slot = adapterInfo.BusNumber;
      gpuinfo.Subsys = adapterInfo.UDID.Substring(adapterInfo.UDID.IndexOf("SUBSYS") + 7, 8);
      statcount = 60;
  
      Update();                   
    }

    public int BusNumber { get { return busNumber; } }

    public int DeviceNumber { get { return deviceNumber; } }

    private void UpdateCurr()
    {
        if (ADL.ADL_Overdrive5_Temperature_Get(adapterIndex, 0, ref adlt) == ADL.ADL_OK)
        {
            gpucurr.GPUTemp = adlt.Temperature / 1000;
        }
        else gpucurr.GPUTemp = 0;

      if (ADL.ADL_Overdrive5_FanSpeedInfo_Get(adapterIndex, 0, ref afsi) == ADL.ADL_OK)
      {
          adlf.SpeedType = ADL.ADL_DL_FANCTRL_SPEED_TYPE_PERCENT;
          if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref adlf) == ADL.ADL_OK)
          {
              gpucurr.FanLoad = adlf.FanSpeed;
          }
          else gpucurr.FanLoad = 0;
          adlf.SpeedType = ADL.ADL_DL_FANCTRL_SPEED_TYPE_RPM;
          if (ADL.ADL_Overdrive5_FanSpeed_Get(adapterIndex, 0, ref adlf) == ADL.ADL_OK)
          {
              gpucurr.FanRPM = adlf.FanSpeed;
          }
          else gpucurr.FanRPM = 0;
      }

      if (ADL.ADL_Overdrive5_CurrentActivity_Get(adapterIndex, ref adlp) == ADL.ADL_OK)
      {
          if (adlp.EngineClock > 0)
          {
              gpucurr.CoreClock = adlp.EngineClock / 100;
          }
          else gpucurr.CoreClock = 0;
          if (adlp.MemoryClock > 0)
          {
              gpucurr.MemoryClock = adlp.MemoryClock / 100;
          }
          else gpucurr.MemoryClock = 0;
          if (adlp.Vddc > 0)
          {
          }
          gpucurr.GPULoad = adlp.ActivityPercent;
          //gpuinfo.MemCtrlLoad = adlp.CurrentPerformanceLevel.ToString();
      }
      else gpucurr.GPULoad = 0;
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

        if (gpucurr.GPUTemp > 0) gpuinfo.GPUTemp = gpucurr.GPUTemp.ToString();
        else gpuinfo.GPUTemp = "-";
        if (gpucurr.FanLoad > 0) gpuinfo.FanLoad = gpucurr.FanLoad.ToString();
        else gpuinfo.FanLoad = "-";
        if (gpucurr.FanRPM > 0) gpuinfo.FanRPM = gpucurr.FanRPM.ToString();
        else gpuinfo.FanRPM = "-";
        if (gpucurr.CoreClock > 0) gpuinfo.CoreClock = gpucurr.CoreClock.ToString();
        else gpuinfo.CoreClock = "-";
        if (gpucurr.MemoryClock > 0) gpuinfo.MemoryClock = gpucurr.MemoryClock.ToString();
        else gpuinfo.MemoryClock = "-";
        if (gpucurr.GPULoad > 0) gpuinfo.GPULoad = gpucurr.GPULoad.ToString();
        else gpuinfo.GPULoad = "-";
        gpuinfo.MemCtrlLoad = "-";
    }
  }
}
