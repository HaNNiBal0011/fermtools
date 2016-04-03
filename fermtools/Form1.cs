using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Management;

namespace fermtools
{
    public partial class Form1 : Form
    {
        const int NumPar = 7;       //Число параметров GPU выводимых в окошки
        NvidiaGroup nvigr;          //Группа Nvidia видеокарт
        ATIGroup atigr;             //Группа AMD видеокарт
        bool fExitCancel;           //Флаг используется для сворачивания окна формы в трей при нажатии на крестик и для завершения программы при нажатии Exit в контектстном меню 
        byte WDtimer;               //Интервал в минутах для записи в WatchDog Timer
        Thread pipeServerTh;        //Поток для работы именованного канала
        ManualResetEvent signal;    //Сигнал для асинхронного чтения из pipe или завершения серверного процесса
        WDT wdt;                    //WatchDog Timer
        ToolTip pbTT;               //Инфа для отображения состояния WDT

        private List<System.Windows.Forms.TextBox> par = new List<System.Windows.Forms.TextBox>();    //Коллекция текст боксов для отображения параметров видеокарт

        public Form1(string[] args)
        {
            InitializeComponent();
            fExitCancel = true; //Запрещаем выход из программы
            nvigr = new NvidiaGroup(); //Группа видеокарт NVIDIA
            atigr = new ATIGroup(); //Группа видеокарт ATI
            WriteEventLog(GetReportVideoCard(), EventLogEntryType.Information);
            InitVideoCards(); //Добавление элементов формы для отображения параметров видеокарт
            pipeServerTh = new Thread(pipeServerThread); //Поток для работы именованного канала
            signal = new ManualResetEvent(false);
            wdt = new WDT(); //Инициализация WatchDog Timer
            if (wdt.isWDT) InitWDT(args);
            WriteEventLog(wdt.GetReport(), EventLogEntryType.Information);
            timer1.Start(); //Стартуем таймеры и потоки
            pipeServerTh.Start();
        }
        private string GetReportVideoCard()
        {
            StringBuilder report = new StringBuilder();

            for (int i=0; i<nvigr.hardware.Count;i++) report.AppendLine(nvigr.hardware[i].GetReport());
            if (atigr.hardware.Count > 0) report.AppendLine(atigr.GetReport());

            return report.ToString();
        }
        private void InitVideoCards()
        {
            //Вычисление размеров и положения текст боксов на вкладке будет работать, если ренее не было задано
            //свойство формы AutoScaleMode, нужно чтобы в свойствах формы было AutoScaleMode = Inherit
            //Добавляем на форму текст боксы для вывода параметров видеокарт NVIDIA
            int nviCardCount = nvigr.hardware.Count;
            int atiCardCount = atigr.hardware.Count;
            int txtBoxWith = (this.tabPage1.Width - 200) / (nviCardCount + atiCardCount);
            for (int i = 0; i < nviCardCount; i++)
            {
                for (int j = 0; j < NumPar; j++)
                {
                    int m = i * NumPar + j;
                    this.par.Insert(m, new System.Windows.Forms.TextBox());
                    this.tabPage1.Controls.Add(this.par[m]);
                    this.par[m].Size = new System.Drawing.Size(txtBoxWith - 6, 22);
                    this.par[m].Location = new System.Drawing.Point(180 + txtBoxWith * i, 10 + 30 * j); //X = 234 (180 + 48 + 6) Y = 40 (10 + 22 + 8)
                    this.par[m].ReadOnly = true;
                    this.par[m].TextAlign = HorizontalAlignment.Right;
                    this.par[m].BackColor = System.Drawing.SystemColors.Window;
                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(this.par[m], nvigr.hardware[i].gpuinfo.GPUName + "\nSubsys " + nvigr.hardware[i].gpuinfo.Subsys + "\nSlot " + nvigr.hardware[i].gpuinfo.Slot.ToString());
                }
            }
            //Добавляем на форму текст боксы для вывода параметров видеокарт ATI
            for (int i = 0; i < atiCardCount; i++)
            {
                for (int j = 0; j < NumPar; j++)
                {
                    int m = (i + nviCardCount) * NumPar + j;
                    this.par.Insert(m, new System.Windows.Forms.TextBox());
                    this.tabPage1.Controls.Add(this.par[m]);
                    this.par[m].Size = new System.Drawing.Size(txtBoxWith - 6, 22);
                    this.par[m].Location = new System.Drawing.Point(180 + txtBoxWith * (i + nviCardCount), 10 + 30 * j); //X = 234 (180 + 48 + 6) Y = 40 (10 + 22 + 8)
                    this.par[m].ReadOnly = true;
                    this.par[m].TextAlign = HorizontalAlignment.Right;
                    this.par[m].BackColor = System.Drawing.SystemColors.Window;
                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(this.par[m], atigr.hardware[i].gpuinfo.GPUName + "\nSubsys " + atigr.hardware[i].gpuinfo.Subsys + "\nSlot " + atigr.hardware[i].gpuinfo.Slot.ToString());
                }
            }
            //Применяем масштабирование формы, чтобы вновь добавленные элементы оказались в том же масштабе
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }
        private void InitWDT(string[] args)
        {
            //Пытаемся брать значение таймера из командной строки, если не выходит, WDT устанавливаем 10 минут
            if (CmdString(args)) WDtimer = (byte)Convert.ToInt16(args[0]);
            else WDtimer = 10;
            //Добавляем в панель статуса информацию о чипе и показываем
            this.toolStripStatusLabel1.Text = "WDT Chip " + wdt.WDTnameChip;
            this.toolStripStatusLabel1.Visible = true;
            pbTT = new ToolTip();
            if (WDtimer > 0)
            {
                this.toolStripProgressBar1.Visible = true;
                //Добавляем информацию о состоянии таймера
                pbTT.SetToolTip(this.statusStrip1, "WDT not set");
                timer2.Start();
            }
            else pbTT.SetToolTip(this.statusStrip1, "WDT disabled");
        }
        private void pipeServerThread(object data)
        {
            //Организуем асинхронный pipe
            IAsyncResult asyncResult;
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("pipefermtools", PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 0, 512);
            WriteEventLog("Pipe for fermtools started. Wait for connection.", EventLogEntryType.Information);
            //Крутимся в цикле пока программа запущена
            while (fExitCancel)
            {
                try
                {
                    //Асинхронное ожидание прерывается установкой сигнала signal.Set(), результат произошло соединение или нет в переменной asyncResult
                    asyncResult = pipeServer.BeginWaitForConnection(_ => signal.Set(), null);
                    //Блокируем процесс, по завершении ожидания, устанавливаемого функцией signal.Set(), ресетим сигнал, чтобы снова процесс впал в спячку
                    signal.WaitOne(); 
                    signal.Reset();
                    if (asyncResult.IsCompleted)
                    {
                        //Дальше все по накатаной: завершаем состояние ожидания асинхронного коннекта и пишем в буфер pipe не ожидая чтения на приемной стороне
                        pipeServer.EndWaitForConnection(asyncResult);
                        StreamWriter sw = new StreamWriter(pipeServer);
                        sw.AutoFlush = true;
                        sw.WriteLine(NumPar.ToString());
                        if (nvigr.hardware.Count > 0)
                        {
                            sw.WriteLine(nvigr.hardware.Count.ToString());
                            for (int i = 0; i < nvigr.hardware.Count; i++)
                            {
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.GPUName);
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.CoreClock);
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.MemoryClock);
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.GPUTemp);
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.GPULoad);
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.FanLoad);
                                sw.WriteLine(nvigr.hardware[i].gpuinfo.FanRPM);
                            }
                        }
                        if (atigr.hardware.Count > 0)
                        {
                            sw.WriteLine(atigr.hardware.Count.ToString());
                            for (int i = 0; i < atigr.hardware.Count; i++)
                            {
                                sw.WriteLine(atigr.hardware[i].gpuinfo.GPUName);
                                sw.WriteLine(atigr.hardware[i].gpuinfo.CoreClock);
                                sw.WriteLine(atigr.hardware[i].gpuinfo.MemoryClock);
                                sw.WriteLine(atigr.hardware[i].gpuinfo.GPUTemp);
                                sw.WriteLine(atigr.hardware[i].gpuinfo.GPULoad);
                                sw.WriteLine(atigr.hardware[i].gpuinfo.FanLoad);
                                sw.WriteLine(atigr.hardware[i].gpuinfo.FanRPM);
                            }
                        }
                        //Ждем окончания чтения и отключаем клиента
                        pipeServer.WaitForPipeDrain();
                        pipeServer.Disconnect();
                    }
                }
                catch (IOException e)
                {
                    WriteEventLog(String.Format("Pipe Server Error: {0}", e.Message), EventLogEntryType.Error);
                    pipeServer.Disconnect();
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = fExitCancel;
            this.Hide();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible) this.Hide();
            else this.Show();
        }
        private bool CmdString(string[] str)
        {
            Int64 t = 0;

            if (str.Length == 0) return false;
            if (String.IsNullOrEmpty(str[0])) return false;
            if (str[0].Length > 3) return false;
            try
            {
                t = Convert.ToInt64(str[0]);
            }
            catch (Exception ex)
            {
                string exType = ex.GetType().ToString();
                object defVal = exType.Substring(exType.LastIndexOf('.') + 1);
                return false;
            }
            if ((t > 254) || (t < 0)) return false;
            return true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Цикл тика 1 секунда, нстраивается в графическом конструкторе свойств
            for (int i = 0; i < nvigr.hardware.Count; i++)
            {
                nvigr.hardware[i].Update();
                int m = NumPar * i;
                this.par[m].Text = nvigr.hardware[i].gpuinfo.CoreClock;         // +" / " + ((uint)nvigr.hardware[i].gpustat.CoreClock).ToString();
                if (this.checkCoreClock.Checked)
                    if (nvigr.hardware[i].gpustat.LCoreClock.Max() > 2 * nvigr.hardware[i].gpustat.CoreClock) 
                        resetToolStripMenuItem_Click(sender, e);
                this.par[m + 1].Text = nvigr.hardware[i].gpuinfo.MemoryClock;   // +" / " + ((uint)nvigr.hardware[i].gpustat.MemoryClock).ToString();
                this.par[m + 2].Text = nvigr.hardware[i].gpuinfo.GPULoad;       // +" / " + ((uint)nvigr.hardware[i].gpustat.GPULoad).ToString();
                this.par[m + 3].Text = nvigr.hardware[i].gpuinfo.MemCtrlLoad;   // +" / " + ((uint)nvigr.hardware[i].gpustat.MemCtrlLoad).ToString();
                this.par[m + 4].Text = nvigr.hardware[i].gpuinfo.GPUTemp;       // +" / " + ((uint)nvigr.hardware[i].gpustat.GPUTemp).ToString();
                this.par[m + 5].Text = nvigr.hardware[i].gpuinfo.FanLoad;       // +" / " + ((uint)nvigr.hardware[i].gpustat.FanLoad).ToString();
                this.par[m + 6].Text = nvigr.hardware[i].gpuinfo.FanRPM;        // +" / " + ((uint)nvigr.hardware[i].gpustat.FanRPM).ToString();
            }
            for (int i = 0; i < atigr.hardware.Count; i++)
            {
                atigr.hardware[i].Update();
                int m = NumPar * (i + nvigr.hardware.Count);
                this.par[m].Text = atigr.hardware[i].gpuinfo.CoreClock;
                this.par[m + 1].Text = atigr.hardware[i].gpuinfo.MemoryClock;
                this.par[m + 2].Text = atigr.hardware[i].gpuinfo.GPULoad;
                this.par[m + 3].Text = atigr.hardware[i].gpuinfo.MemCtrlLoad;
                this.par[m + 4].Text = atigr.hardware[i].gpuinfo.GPUTemp;
                this.par[m + 5].Text = atigr.hardware[i].gpuinfo.FanLoad;
                this.par[m + 6].Text = atigr.hardware[i].gpuinfo.FanRPM;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            //Цикл тика 10 секунд, нстраивается в графическом конструкторе свойств
            //Логика такова. Если остаток таймера меньше или равен 1 минуте, то перезаряжаем
            //Если больше 1 минуты, выводим в тоолтип сколько осталось и устанавливаем соответствующюу величину прогресса.
            int progress = wdt.GetWDT();
            if (progress <= 1)
            {
                if (wdt.SetWDT(WDtimer))
                {
                    WriteEventLog("Watchdog timer set to " + WDtimer.ToString() + " min.", EventLogEntryType.Information);
                    //Здесь устанавливаем значение максимума, т.к., возможно, впоследствие, WDtimer будет возможно менять интерактивно
                    toolStripProgressBar1.Maximum = WDtimer;
                    toolStripProgressBar1.Value = WDtimer;
                    pbTT.SetToolTip(this.statusStrip1, "WDT set to " + WDtimer.ToString() + " min.");
                }
                else WriteEventLog(wdt.GetReport(), EventLogEntryType.Error);
            }
            else
            {
                //При этом величина прогресса не должна превышать максимальное значение (мало ли кто еще установил таймер)
                if (progress <= WDtimer)
                {
                    toolStripProgressBar1.Value = progress;
                    pbTT.SetToolTip(this.statusStrip1, String.Format("WDT will reset computer after {0:D} min.", progress));
                }
            }
        }
        private bool WriteEventLog(string msg, EventLogEntryType type)
        {
            try { eventLog1.WriteEntry(msg, type); }
            catch { return false; }
            return true;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Разрешаем завершение программы
            fExitCancel = false;
            //Прерываем поток pipe
            signal.Set();
            //Останавливаем WDT, если он есть
            if (wdt.isWDT) if (wdt.SetWDT(0)) WriteEventLog("Watchdog timer disabled.", EventLogEntryType.Information);
            Application.Exit();
        }
        private void ShowtoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wdt.isWDT)
            {
                timer2.Stop();
                wdt.SetWDT(1);
            }
            else
            {
                ManagementBaseObject mboReboot = null;
                ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem"); mcWin32.Get();
                // You can't shutdown without security privileges
                mcWin32.Scope.Options.EnablePrivileges = true;
                ManagementBaseObject mboRebootParams = mcWin32.GetMethodParameters("Win32Shutdown");
                // Flags: - 0 (0x0) Log Off - 4 (0x4) Forced Log Off (0 4) - 1 (0x1) Shutdown - 5 (0x5) Forced Shutdown (1 4) - 2 (0x2) Reboot 
                // - 6 (0x6) Forced Reboot (2 4) - 8 (0x8) Power Off - 12 (0xC) Forced Power Off (8 4)
                mboRebootParams["Flags"] = "6";
                mboRebootParams["Reserved"] = "0";
                foreach (ManagementObject manObj in mcWin32.GetInstances())
                {
                    mboReboot = manObj.InvokeMethod("Win32Shutdown", mboRebootParams, null);
                }
            }
        }
    }
}
