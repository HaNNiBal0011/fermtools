using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public int Count = 0;                                           //Счетчик минут
        public string PortName;                                         //Имя открытого порта

        public OpenWDT(string ComPort)
        {
            //Проверяем, есть ли OpenDev USB WDT
            isWDT = GetOpenDevUSB(ComPort);
            if (isWDT)
                report.AppendLine("Found OpenDev USB WDT on port " + ComPort);
            else
                report.AppendLine("Not found OpenDev USB WDT to port " + ComPort);
        }
        private bool GetOpenDevUSB(string ComPort)
        {
            string answer = new string(string.Empty.ToCharArray());
            if (!String.IsNullOrEmpty(ComPort))
            {
                try
                {
                    sp = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One);
                    sp.WriteTimeout = 3000;
                    sp.ReadTimeout = 3000;
                    sp.Open();
                    sp.Write("~U".ToCharArray(),0,2);
                    Thread.Sleep(500);
                    answer = sp.ReadExisting();
                    sp.Close();
                }
                catch (Exception ex)
                {
                    sp.Close();
                    report.AppendLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message + " Func: GetOpenDevUSB()" + " Par:" + ComPort);
                    return false;
                }
                if (answer.Equals("~A"))
                {
                    WDTnameChip = "OpenDev";
                    PortName = sp.PortName;
                    return true;
                }
            }
            return false;
        }
        public bool SetWDT(ref byte count)
        {
            report.Clear();
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
                catch (Exception ex)
                {
                    sp.Close();
                    report.AppendLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message + " Func: SetWdt()" + " Par:" + count.ToString());
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
                    sp.Close();
                    Thread.Sleep(500);
                    sp.Open();
                    sp.Write(("~W" + count.ToString()).ToCharArray(), 0, 3);
                    sp.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    sp.Close();
                    report.AppendLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message + " Func: SetWdt()" + " Par:" + count.ToString());
                }
            }
            return false;
        }
        public bool TimerReset()
        {
            report.Clear();
            string answer = new string(string.Empty.ToCharArray());
            try
            {
                sp.Open();
                sp.Write("~U".ToCharArray(),0,2);
                Thread.Sleep(500);
                answer = sp.ReadExisting();
                sp.Close();
            }
            catch (Exception ex)
            {
                sp.Close();
                report.AppendLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message + " Func: TimerReset()");
                return false;
            }
            if (answer.Equals("~A"))
            {
                return true;
            }
            report.AppendLine("The answer from port " + sp.PortName + " is not equal to ~A Func: TimerReset()");
            return false;
        }

        public bool ResetTest()
        {
            report.Clear();
            try
            {
                sp.Open();
                sp.Write("~T1".ToCharArray(), 0, 3);
                Thread.Sleep(500);
                sp.Close();
                return true;
            }
            catch (Exception ex)
            {
                sp.Close();
                report.AppendLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message + " Func: ResetTest()");
            }
            return false;
        }
        public string GetReport()
        {
            return report.ToString();
        }
    }
}
