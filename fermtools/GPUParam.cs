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
        public double ParCurrent;
        public double ParSum;
        public List<int> ParCollect;

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

        public void Update()
        {

        }
    }
}
