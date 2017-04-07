using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace fermtools
{
    class OpenWDT
    {
        public string WDTnameChip;                                      //Наименование чипа WDT
        private SerialPort sp;                                          //Порт для opendev USB WDT
        public readonly bool isWDT;                                     //Флаг наличия чипа WDT
        public readonly StringBuilder report = new StringBuilder();     //Для отчета
        public int Count;                                               //Счетчик минут

        public OpenWDT(string ComPort)
        {
            //Проверяем, есть ли OpenDev USB WDT
            isWDT = GetOpenDevUSB(ComPort);
            if (isWDT)
                report.AppendLine("Found OpenDev USB WDT on port " + ComPort);
            else
                report.AppendLine("Not fount OpenDev USB WDT to port " + ComPort);
        }
        private bool GetOpenDevUSB(string ComPort)
        {
            string answer = new string(string.Empty.ToCharArray());
            if (!String.IsNullOrEmpty(ComPort))
            {
                try
                {
                    sp = new SerialPort(ComPort);
                    sp.WriteTimeout = 500;
                    sp.ReadTimeout = 5000;
                    sp.Open();
                    sp.Write("~U".ToCharArray(),0,2);
                    answer = sp.ReadExisting();
                    sp.Close();
                }
                catch
                {
                    sp.Close();
                    report.AppendLine("Error initialise OpenDev USB WDT to port " + ComPort);
                    return false;
                }
                if (answer.Equals("~A"))
                {
                    WDTnameChip = "OpenDev";
                    return true;
                }
            }
            return false;
        }
        public bool SetWDT(byte count)
        {
            //Приводим в соответствие с диапазоном значений для opendev WDT
            if (count <= 0)
            {
                //Если 0 или меньше, то считаем 0 и отключаем таймер
                count = 0;
                try
                {
                    sp.Open();
                    sp.Write(("~P1").ToCharArray(), 0, 3);
                    sp.Close();
                    return true;
                }
                catch
                {
                    sp.Close();
                    report.AppendLine("Pause enable error OpenDev USB WDT");
                }
            }
            else
            {
                //Если 9 или больше, считаем 9 и включаем таймер и соответствующую задержку
                if (count > 9)
                    count = 9;
                try
                {
                    sp.Open();
                    sp.Write(("~P0").ToCharArray(), 0, 3);
                    sp.Write(("~W" + count.ToString()).ToCharArray(), 0, 3);
                    sp.Close();
                    return true;
                }
                catch
                {
                    sp.Close();
                    report.AppendLine("Error write timeout to OpenDev USB WDT");
                }
            }
            return false;
        }
        public string GetReport()
        {
            return report.ToString();
        }
    }
}
