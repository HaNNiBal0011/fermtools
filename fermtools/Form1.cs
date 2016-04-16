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
        const int TickCountMax=60;  //Интервал времени для расчета отслеживаемых параметров, зависит от цикла таймера, в котором используется
        NvidiaGroup nvigr;          //Группа Nvidia видеокарт
        ATIGroup atigr;             //Группа AMD видеокарт
        bool fExitCancel;           //Флаг используется для сворачивания окна формы в трей при нажатии на крестик и для завершения программы при нажатии Exit в контектстном меню 
        bool fReset;                //Устанавливается, если уже запущен процесс перезагрузки компьютера
        byte WDtimer;               //Интервал в минутах для записи в WatchDog Timer
        Thread pipeServerTh;        //Поток для работы именованного канала
        ManualResetEvent signal;    //Сигнал для асинхронного чтения из pipe или завершения серверного процесса
        WDT wdt;                    //WatchDog Timer
        ToolTip pbTT;               //Инфа для отображения состояния WDT
        int CardCount;              //Количество найденных видеокарт

        private List<GPUParam> gpupar = new List<GPUParam>();               //Параметры GPU
        private List<System.Windows.Forms.TextBox> par = new List<System.Windows.Forms.TextBox>();    //Коллекция текст боксов для отображения параметров видеокарт
        private List<System.Windows.Forms.CheckBox> check = new List<System.Windows.Forms.CheckBox>();    //Коллекция чек боксов для отметки отслеживания параметров

        public Form1(string[] args)
        {
            InitializeComponent();
            fExitCancel = true; //Запрещаем выход из программы
            fReset = false; //Перезагрузка не инициализирована
            nvigr = new NvidiaGroup(ref gpupar, NumPar); //Добавляем в группу видеокарты NVIDIA
            atigr = new ATIGroup(); //Группа видеокарт ATI
            CardCount = gpupar.Count; //Сколько всего видеокарт
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

            report.AppendLine(nvigr.GetReport());
            report.AppendLine(atigr.GetReport());

            return report.ToString();
        }
        private void InitVideoCards()
        {
            //Вычисление размеров и положения текст боксов на вкладке будет работать, если ренее не было задано
            //свойство формы AutoScaleMode, нужно чтобы в свойствах формы было AutoScaleMode = Inherit
            //Добавляем на форму текст боксы для вывода параметров видеокарт
            ToolTip tt = new ToolTip();
            int txtBoxWith = (this.tabPage1.Width - 200) / CardCount;
            for (int i = 0; i < CardCount; i++)
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
                    if (j==0)
                        tt.SetToolTip(this.par[m], gpupar[i].GPUName + "\nSubsys " + gpupar[i].Subsys + "\nSlot " + gpupar[i].Slot.ToString());
                }
                //Коэффициенты для срабатывания порога отслеживания
                gpupar[i].GPUParams[0].Rate = gpupar[i].GPUParams[1].Rate = gpupar[i].GPUParams[2].Rate = gpupar[i].GPUParams[3].Rate = 2;
                gpupar[i].GPUParams[4].Rate = gpupar[i].GPUParams[5].Rate = gpupar[i].GPUParams[6].Rate = 1.5;
            }
            //Применяем масштабирование формы, чтобы вновь добавленные элементы оказались в том же масштабе
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            //Помещаем чекбоксы в коллекцию для возможности доступа по индексу
            this.check.Add(this.checkCoreClock);
            this.check.Add(this.checkMemoryClock);
            this.check.Add(this.checkGPULoad);
            this.check.Add(this.checkMemCtrlLoad);
            this.check.Add(this.checkGPUTemp);
            this.check.Add(this.checkFanLoad);
            this.check.Add(this.checkFanRPM);
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
                        if (CardCount > 0)
                        {
                            sw.WriteLine(CardCount.ToString());
                            for (int i = 0; i != CardCount; i++)
                            {
                                sw.WriteLine(gpupar[i].GPUName);
                                sw.WriteLine(gpupar[i].Subsys);
                                sw.WriteLine(gpupar[i].Slot.ToString());
                                for (int j = 0; j != NumPar; j++)
                                    sw.WriteLine(gpupar[i].GPUParams[j].ParCollect.Last().ToString());
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
            for (int i = 0; i != CardCount; i++)
            {
                gpupar[i].Update(TickCountMax);
                for (int j = 0; j != NumPar; j++)
                    this.par[j].Text = gpupar[i].GPUParams[j].ParCollect.Last().ToString();
            }
        }
        private bool Monitoring()
        {
            bool res = false;
            StringBuilder report = new StringBuilder();
            //Бежим по параметрам
            for (int i = 0; i != NumPar;i++)
            {
                //Проверяем установленный флаг слежения
                if (check[i].Checked)
                {
                    //Бежим по картам
                    for (int j=0; j!=CardCount; j++)
                    {
                        //Проверяем уменьшилось ли среднее значение ниже MAX/RATE
                        if (gpupar[j].GPUParams[i].ParCollect.Average() < gpupar[j].GPUParams[i].ParCollect.Max()/gpupar[j].GPUParams[i].Rate)
                        {
                            //Если уменьшилось сообщаем в репорт и устанавливаем флаг
                            report.AppendLine(gpupar[j].GPUName + ", subsys:" + gpupar[j].Subsys + ", slot:" + gpupar[j].Slot.ToString());
                            report.AppendLine(check[i].Text + " average:" + gpupar[j].GPUParams[i].ParCollect.Average().ToString());
                            report.AppendLine();
                            res = true;
                        }
                    }
                }
            }
            //Если что то где то упало, сообщаем в EVENLOG
            if (res) 
                WriteEventLog(report.ToString(), EventLogEntryType.Error);
            return res;
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
            //Если флаг установлен, то перезагрузка уже инициирована
            if (fReset) return;
            fReset = true;
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
