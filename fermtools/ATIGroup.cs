/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2012 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace fermtools
{
  internal class ATIGroup 
  {
    public readonly StringBuilder report = new StringBuilder();
    public ATIGroup(ref List<GPUParam> gpupar, int numpar)
    {
      try 
      {
        int status = ADL.ADL_Main_Control_Create(1);
        report.AppendLine("AMD Display Library");
        report.AppendLine();
        report.Append("Status: ");
        report.AppendLine(status == ADL.ADL_OK ? "OK" : status.ToString(CultureInfo.InvariantCulture));
        report.AppendLine();
        if (status == ADL.ADL_OK) 
        {
          int numberOfAdapters = 0;
          ADL.ADL_Adapter_NumberOfAdapters_Get(ref numberOfAdapters);
          report.Append("Number of adapters: "); 
          report.AppendLine(numberOfAdapters.ToString(CultureInfo.InvariantCulture));
          report.AppendLine();
          if (numberOfAdapters > 0) 
          {
              ADLAdapterInfo[] adapterInfo = new ADLAdapterInfo[numberOfAdapters];
              if (ADL.ADL_Adapter_AdapterInfo_Get(adapterInfo) == ADL.ADL_OK)
              for (int i = 0; i < numberOfAdapters; i++) 
              {
                int isActive;
                ADL.ADL_Adapter_Active_Get(adapterInfo[i].AdapterIndex,out isActive);
                int adapterID;
                ADL.ADL_Adapter_ID_Get(adapterInfo[i].AdapterIndex,out adapterID);
                report.Append("AdapterIndex: "); 
                report.AppendLine(i.ToString(CultureInfo.InvariantCulture));
                report.Append("isActive: "); 
                report.AppendLine(isActive.ToString(CultureInfo.InvariantCulture));
                report.Append("AdapterName: "); 
                report.AppendLine(adapterInfo[i].AdapterName);     
                report.Append("UDID: ");
                report.AppendLine(adapterInfo[i].UDID);
                report.Append("Present: ");
                report.AppendLine(adapterInfo[i].Present.ToString(CultureInfo.InvariantCulture));
                report.Append("VendorID: 0x");
                report.AppendLine(adapterInfo[i].VendorID.ToString("X",CultureInfo.InvariantCulture));
                report.Append("BusNumber: ");
                report.AppendLine(adapterInfo[i].BusNumber.ToString(CultureInfo.InvariantCulture));
                report.Append("DeviceNumber: ");
                report.AppendLine(adapterInfo[i].DeviceNumber.ToString(CultureInfo.InvariantCulture));
                report.Append("FunctionNumber: ");
                report.AppendLine(adapterInfo[i].FunctionNumber.ToString(CultureInfo.InvariantCulture));
                report.Append("AdapterID: 0x");
                report.AppendLine(adapterID.ToString("X", CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(adapterInfo[i].UDID) && adapterInfo[i].VendorID == ADL.ATI_VENDOR_ID) 
                      gpupar.Add(new GPUParam(adapterInfo[i], numpar));
                report.AppendLine();
              }
          }
        }
      } 
      catch (DllNotFoundException) 
      { 

      }
      catch (EntryPointNotFoundException e) 
      {
          report.AppendLine();
          report.AppendLine(e.ToString());
          report.AppendLine();        
      }
    }

    public string GetReport() 
    {
      return report.ToString();
    }
  }
}
