using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fermtools
{
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
        public string GPUName;
        public string Subsys;
        public int Slot;
        public List<OneParam> GPUParams;

        public GPUParam(int num)
        {
            GPUParams = new List<OneParam>();
            for (int i = 0; i != num; i++)
                GPUParams.Insert(i, new OneParam());
        }
    }
}
