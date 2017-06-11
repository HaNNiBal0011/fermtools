// Copyright © 2016 Dimasin. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fermtools
{
    public partial class Form1 : Form
    {
        const int NumPar = 7;                   //Число параметров GPU выводимых в окошки
        const int WDT_SOFT = 0;                 //Выбран софтовый WDT
        const int WDT_ONBOARD = 1;              //Выбран WDT на мат. плате
        const int WDT_USBOPEN = 2;              //Выбран OpenHadrdware USB WDT
        int CurrentWDT = 0;                     //Текущий WDT         
        int TickCountMax = 60;                  //Интервал времени для расчета отслеживаемых параметров, зависит от цикла таймера, в котором используется
        int MonDelay = 60;                      //Время задержки начала мониторинга при старте, сек
        int MsgBoxPause = 20000;                //Пауза для отображения сообщения, когда пользователь сможет нажать Отмену перезагрузки (мс)
        int MsgBoxTimeout = 10000;              //пауза после отмены перезагрузки до отображения нового сообщения об ошибке (мс)
        NvidiaGroup nvigr;                      //Группа Nvidia видеокарт
        ATIGroup atigr;                         //Группа AMD видеокарт
        bool fExitCancel;                       //Флаг используется для сворачивания окна формы в трей при нажатии на крестик и для завершения программы при нажатии Exit в контектстном меню 
        bool fReset;                            //Устанавливается, если уже запущен процесс перезагрузки компьютера
        bool fMessage;                          //Устанавливается, если выведена сообщение перезагрузки
        bool fNoUp;                             //Не реагировать на повышение параметров при мониторинге
        byte WDtimer;                           //Интервал в минутах для записи в WatchDog Timer
        Thread pipeServerTh;                    //Поток для работы именованного канала
        ManualResetEvent signal;                //Сигнал для асинхронного чтения из pipe или завершения серверного процесса
        WDT wdt;                                //WatchDog Timer
        SoftWDT wdt_s;                          //Soft WDT
        OpenWDT wdt_o;                          //Open WDT USB
        ToolTip pbTT;                           //Инфа для отображения состояния WDT
        int CardCount;                          //Количество найденных видеокарт
        private string config_path;             //Путь конфига
        const string rand = "xBW8skR2lmmMs";    //Случайная строка для эмуляции пароля
        TelegramBot bot = new TelegramBot();    //Бот Telegram
        MinerRemote miner = new MinerRemote();  //Подключение к клеймору

        private SettingsJSON config = new SettingsJSON();                   //Конфиг
        private List<Update> botUpdate = new List<Update>();                //Сообщения боту
        private List<GPUParam> gpupar = new List<GPUParam>();               //Коллекция Параметров GPU
        private List<System.Windows.Forms.TextBox> par = new List<System.Windows.Forms.TextBox>();          //Коллекция текст боксов для отображения параметров видеокарт
        private List<System.Windows.Forms.CheckBox> check = new List<System.Windows.Forms.CheckBox>();      //Коллекция чек боксов для отметки отслеживания параметров
        private List<System.Windows.Forms.Label> label = new List<System.Windows.Forms.Label>();            //Коллекция меток для вывода названий параметров

        public Form1(string[] args)
        {
            InitializeComponent();
            miner.server = "127.0.0.1";
            miner.port = 3333;
            miner.GetStatistic();
            miner.RestartMiner();

            config_path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\fermtools.json";
            string config_path0 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\fermtools0.json";
            if (!config.ReadParam(ref config_path))
            {
                if (config.ReadParam(ref config_path0)) //Читаем резервную копию
                    config.WriteParam(ref config_path);
                else
                {
                    if (config.WriteParamDefault(ref config_path))
                    {
                        if (!config.ReadParam(ref config_path))
                        {
                            MessageBox.Show("Error load setting from file", "Restore Setting", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            Application.Exit();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error load setting from file", "Restore Setting", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        Application.Exit();
                    }
                }
            }
            FindComPorts();
            RestoreSetting();
            fExitCancel = true; //Запрещаем выход из программы
            fReset = false; //Перезагрузка не инициализирована
            fMessage = false; //Сообщение не выведено
            nvigr = new NvidiaGroup(ref gpupar, NumPar); //Добавляем в группу видеокарты NVIDIA
            atigr = new ATIGroup(ref gpupar, NumPar); //Группа видеокарт ATI
            CardCount = gpupar.Count; //Сколько всего видеокарт
            if (CardCount == 0) //Если видеокарт нет, выходим
            {
                MessageBox.Show("Not found compatible videocards", "Fermtools", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            SetMonitoringSetting(); //Восстанавливаем параметры мониторинга из конфига (после инициализации видеокарт)
            //Устанавливаем длительность задержки монторинга и запускаем таймер задержки мониторинга, если он еще не запущен
            this.timer4.Interval = MonDelay;
            if (!this.timer4.Enabled)
                this.timer4.Start();
            WriteEventLog(GetReportVideoCard(), EventLogEntryType.Information);
            InitVideoCards(); //Добавление элементов формы для отображения параметров видеокарт
            InitWDT(args);  //Инициализация WDT
            timer1.Start(); //Стартуем таймер мониторинга
            if (config.conf.othset.cb_startPipe) //Стартуем поток канала, если нужен
            {
                signal = new ManualResetEvent(false);
                pipeServerTh = new Thread(pipeServerThread); //Поток для работы именованного канала
                pipeServerTh.Start();
            }
            if (this.cbTelegramOn.Checked) //Инициализируем бота, если установлен соответствующий флаг
                if (botInit() && config.conf.othset.isReset) //Если бот инициализирован и был нештатный рестарт, отправляем сообщение
                    bot.SendMessage(bot.chatID, this.textFermaName.Text + " restart after freze");
            if (this.cbOnEmail.Checked && this.cbOnSendStart.Checked && config.conf.othset.isReset)
                sendMail("Computer restart after freze"); //Если был хардресет отправляем мыло
            config.conf.othset.isReset = true;
            config.WriteParam(ref config_path);//Устанавливаем и сохраняем состояние ресета
            if (config.conf.othset.CompareGPUCountReset) //Перезагрузка, если видеокарт меньше, чем должно быть
            {
                if (config.conf.othset.GPUCount > CardCount)
                {
                    if (bot.bInit) //Если бот инициализирован отправляем сообщение
                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": does not match the number of video cards. Restarting...");
                    if (this.cbOnEmail.Checked)
                        sendMail("Does not match the number of video cards. Restarting..."); //Если нужно отправляем мыло
                    WriteEventLog("Does not match the number of video cards. Restarting...", EventLogEntryType.Information);
                    Reset_Click(null, null);
                }
            }
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
            int txtBoxWith = (this.tabGPU.Width - 200) / CardCount;
            for (int i = 0; i < CardCount; i++)
            {
                for (int j = 0; j < NumPar; j++)
                {
                    int m = i * NumPar + j;
                    this.par.Insert(m, new System.Windows.Forms.TextBox());
                    this.tabGPU.Controls.Add(this.par[m]);
                    this.par[m].Size = new System.Drawing.Size(txtBoxWith - 6, 22);
                    this.par[m].Location = new System.Drawing.Point(180 + txtBoxWith * i, 10 + 30 * j); //X = 234 (180 + 48 + 6) Y = 40 (10 + 22 + 8)
                    this.par[m].ReadOnly = true;
                    this.par[m].TextAlign = HorizontalAlignment.Right;
                    this.par[m].BackColor = System.Drawing.SystemColors.Window;
                    if (j==0)
                        tt.SetToolTip(this.par[m], gpupar[i].GPUName + "\nDevice ID - " + gpupar[i].Subsys.Substring(4) + " " + gpupar[i].Subsys.Substring(0, 4) + "\nSlot - " + gpupar[i].Slot.ToString() + "; BusWidth - x" + gpupar[i].BusWidth.ToString());
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
            //Помещаем метки (названия параметров) в коллекцию для возможности доступа по индексу
            this.label.Add(this.label1);
            this.label.Add(this.label2);
            this.label.Add(this.label3);
            this.label.Add(this.label4);
            this.label.Add(this.label5);
            this.label.Add(this.label6);
        }
        private void SelectSoftWDT()
        {
            CurrentWDT = WDT_SOFT;
            this.toolStripStatusLabel1.Text = "WDT Chip Software";
            this.radioSoftWDT.Checked = true;
            config.conf.wdtset.CurrentWDT = CurrentWDT;
            config.WriteParam(ref config_path);
        }
        private void InitWDT(string[] args)
        {
            //Пытаемся брать значение таймера из командной строки, иначе используем значение полученное при инициализации
            if (CmdString(args))
                WDtimer = (byte)Convert.ToInt16(args[0]);
                
            wdt_s = new SoftWDT();  //Инициализация софтового WDT
            //Добавляем в панель статуса информацию о чипе и показываем
            switch (CurrentWDT)
            {
                case WDT_SOFT:
                    this.toolStripStatusLabel1.Text = "WDT Chip Software";
                    break;
                case WDT_ONBOARD:
                    wdt = new WDT(); //Инициализация onboard WDT
                    WriteEventLog(wdt.GetReport(), EventLogEntryType.Information);
                    if (wdt.isWDT)  //Если инициализация onboard WDT неудачная, то включаем софт ресет
                        this.toolStripStatusLabel1.Text = "WDT Chip " + wdt.WDTnameChip;
                    else
                        SelectSoftWDT();
                    break;
                case WDT_USBOPEN: //Инициализация OpenWDT
                    wdt_o = new OpenWDT(config.conf.wdtset.wdtPort);
                    WriteEventLog(wdt_o.GetReport(), EventLogEntryType.Information);
                    if (wdt_o.isWDT)
                    {
                        if (wdt_o.SetWDT(ref WDtimer)) //Устанавливаем таймаут
                            this.toolStripStatusLabel1.Text = "WDT Chip " + wdt_o.WDTnameChip;
                        else
                            wdt_o.isWDT = false;
                    }
                    if (!wdt_o.isWDT)  //Если инициализация OpenWDT неудачная, то включаем софт ресет
                    {
                        WriteEventLog(wdt_o.GetReport(), EventLogEntryType.Error);
                        SelectSoftWDT();
                    }
                    break;
            }
            //Сохраняем значение WDtimer, если изменилось
            if (this.numericTimeout.Value != WDtimer)
            {
                this.numericTimeout.Value = WDtimer;
                config.conf.wdtset.WDtimer = (Byte)this.numericTimeout.Value;
                config.WriteParam(ref config_path);
            }
            //Показываем элементы управления
            this.toolStripStatusLabel1.Visible = true;
            pbTT = new ToolTip();
            if (WDtimer > 0)
            {
                this.toolStripProgressBar1.Visible = true;
                //Добавляем информацию о состоянии таймера
                pbTT.SetToolTip(this.statusStrip1, "WDT not set");
                timer2.Start();
            }
            else 
                pbTT.SetToolTip(this.statusStrip1, "WDT disabled");
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
            string repmon = "";
            //Цикл тика 1 секунда, нстраивается в графическом конструкторе свойств
            for (int i = 0; i != CardCount; i++)
            {
                gpupar[i].Update(TickCountMax);
                for (int j = 0; j != NumPar; j++)
                    this.par[j+i*NumPar].Text = gpupar[i].GPUParams[j].ParCollect.Last().ToString();
            }
            //Мониторим, если не инициирована перезагрузка, не отображается сообщение и не кончилось время задержки на запуск мониторинга
            if (!fReset && !fMessage && !this.timer4.Enabled)
            {
                if (Monitoring(ref repmon))
                {
                    //Если мониторинг что то нашел, устанавливаем флаг и отображаем сообщение что не так
                    fMessage = true;
                    Thread msgsh = new Thread(MessageShowTh);
                    msgsh.Start(repmon);
                }
            }
        }
        private void MessageShowTh(object rep)
        {
            AutoClosingMessageBox adlg = new AutoClosingMessageBox("Fermtool reset", MsgBoxPause);
            DialogResult dlg = adlg.Show("The monitoring system has identified the wrong setting:\n" + rep + "the computer prepares to reset\n" +
                "If a reset is not required, disable the monitoring");
            adlg._timeoutTimer.Dispose();
            //Если нажали ОК (или само нажалось) то запускаем процесс перезагрузки, иначе нажали Отмена и ждем MsgBoxTimeout для реакции на сработку.
            if (dlg == System.Windows.Forms.DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(config.conf.othset.cmd_Script)) 
                    runCmd();
                Reset_Click(null, null);
            }
            else
            {
                //For test
                //if (!String.IsNullOrEmpty(Properties.Settings.Default.cmd_Script)) runCmd();
                //Если нажата кнопка отмены ждем, чтобы сразу не вылезло следующее предупреждение
                Thread.Sleep(MsgBoxTimeout);
                fMessage = false;
            }
        }
        private void SetMonitoringSetting()
        {
            TickCountMax = (int)this.nc_Span_integration.Value;
            MsgBoxPause = (int)(this.nc_DelayFailover.Value) * 1000;
            MsgBoxTimeout = (int)(this.nc_DelayFailoverNext.Value) * 1000;
            MonDelay = (int)(this.nc_DelayMon.Value) * 1000;
            fNoUp = this.cb_NoUp.Checked;
            for (int j=0; j!=CardCount; j++)
            {
                gpupar[j].GPUParams[0].Rate = (double)this.nc_K_gpu_clock.Value;
                gpupar[j].GPUParams[1].Rate = (double)this.nc_K_mem_clock.Value;
                gpupar[j].GPUParams[2].Rate = (double)this.nc_K_gpu_load.Value;
                gpupar[j].GPUParams[3].Rate = (double)this.nc_K_mem_load.Value;
                gpupar[j].GPUParams[4].Rate = (double)this.nc_K_gpu_temp.Value;
                gpupar[j].GPUParams[5].Rate = (double)this.nc_K_fan_speed_p.Value;
                gpupar[j].GPUParams[6].Rate = (double)this.nc_K_fan_speed_r.Value;
            }
        }
        private bool Monitoring(ref string rep)
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
                        //Проверяем меньше ли среднее значение * RATE, чем MAX
                        if (((int)(gpupar[j].GPUParams[i].ParCollect.Average() * gpupar[j].GPUParams[i].Rate)) < gpupar[j].GPUParams[i].ParCollect.Max())
                        {
                            //Проверяем, установлен ли флаг контролирующий повышение параметров, если установлен и текущий параметр больше среднего
                            //то не реагируем на повышение параметра
                            if ((fNoUp) && (gpupar[j].GPUParams[i].ParCollect.Last() > ((int)gpupar[j].GPUParams[i].ParCollect.Average())))
                                return res;
                            report.AppendLine(gpupar[j].GPUName + ", subsys: " + gpupar[j].Subsys + ", slot: " + gpupar[j].Slot.ToString());
                            report.AppendLine(label[i].Text + " - average: " + ((int)gpupar[j].GPUParams[i].ParCollect.Average()).ToString() + ", maximum: " +
                                gpupar[j].GPUParams[i].ParCollect.Max().ToString());
                            report.AppendLine();
                            res = true;
                        }
                    }
                }
            }
            //Если что то где то упало, сообщаем в EVENLOG, шлем e-mail и телеграмму
            if (res)
            {
                rep = report.ToString();
                WriteEventLog(rep, EventLogEntryType.Error);
                if (this.cbOnEmail.Checked) 
                    sendMail(rep);
                if (bot.bInit)
                    bot.SendMessage(bot.chatID, this.textFermaName.Text + "\n" + rep);
            }
            return res;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            //Цикл тика 10 секунд, нстраивается в графическом конструкторе свойств
            //Логика такова. Если остаток таймера меньше или равен 1 минуте, то перезаряжаем
            //Если больше 1 минуты, выводим в тоолтип сколько осталось и устанавливаем соответствующюу величину прогресса.
            int progress = 0; bool res = false;
            switch (CurrentWDT)         //Получаем значение счетчика, сколько минут осталось до сброса
            {
                case WDT_ONBOARD:
                    progress = wdt.GetWDT();
                    break;
                case WDT_USBOPEN:
                    progress = wdt_o.Count;
                    break;
                case WDT_SOFT:
                    progress = wdt_s.Count;
                    break;
            }
            if (progress <= 1)      //Если меньше минтуы, то пытаемся сбросить WDT
            {
                switch (CurrentWDT)
                {
                    case WDT_ONBOARD:
                        res = wdt.SetWDT(WDtimer); //Если сброс не прошел, то при следующем срабатывании таймера пытаемся снова сбросить WDT
                        if (!res)
                            WriteEventLog(wdt.GetReport(), EventLogEntryType.Error);
                        break;
                    case WDT_SOFT:
                        res = true;
                        wdt_s.Count = WDtimer;
                        if (!this.timerSoft.Enabled)
                            this.timerSoft.Start();
                        break;
                    case WDT_USBOPEN:
                        res = wdt_o.TimerReset();
                        if (res)     //Если сброс не прошел, то при следующем срабатывании таймера пытаемся снова сбросить WDT
                        {
                            wdt_o.Count = WDtimer;  //Если сброс прошел, то перезаряжаем таймер и запускаем счетчик минут до следующего сброса
                            if (!this.timerSoft.Enabled)
                                this.timerSoft.Start();
                        }
                        else
                            WriteEventLog(wdt_o.GetReport(), EventLogEntryType.Error);
                        break;
                }
                if (res)
                {
                    WriteEventLog("Watchdog timer set to " + WDtimer.ToString() + " min.", EventLogEntryType.Information);
                    //Здесь устанавливаем значение максимума, т.к., возможно, впоследствие, WDtimer будет возможно менять интерактивно
                    toolStripProgressBar1.Maximum = WDtimer;
                    toolStripProgressBar1.Value = WDtimer;
                    pbTT.SetToolTip(this.statusStrip1, "WDT set to " + WDtimer.ToString() + " min.");
                }
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
        private void Exit_Click(object sender, EventArgs e)
        {
            //Разрешаем завершение программы
            fExitCancel = false;
            //Если запущен поток канала, завершаем его
            if (config.conf.othset.cb_startPipe)
                signal.Set();
            switch (CurrentWDT)
            {
                case WDT_ONBOARD:
                    if (wdt.SetWDT(0)) //Останавливаем WDT
                        WriteEventLog("Watchdog timer disabled.", EventLogEntryType.Information);
                    else
                        WriteEventLog(wdt.GetReport(), EventLogEntryType.Error);
                    break;
                case WDT_USBOPEN:
                    byte k = 0;
                    if (wdt_o.SetWDT(ref k)) //Останавливаем WDT
                        WriteEventLog("Watchdog timer disabled.", EventLogEntryType.Information);
                    else
                        WriteEventLog(wdt_o.GetReport(), EventLogEntryType.Error);
                    break;
            }
            //Останавливаем таймеры
            if (this.timer1.Enabled) 
                this.timer1.Stop();
            if (this.timer2.Enabled)
                this.timer2.Stop();
            if (this.timer3.Enabled)
                this.timer3.Stop();
            if (this.timer4.Enabled)
                this.timer4.Stop();
            //Сбрасываем состояние ресета и сохраняем в файл
            config.conf.othset.isReset = false;
            config.WriteParam(ref config_path);
            Application.Exit();
        }
        private void Show_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        private void Reset_Click(object sender, EventArgs e)
        {
            //Если флаг установлен, то перезагрузка уже инициирована
            if (fReset) 
                return;
            fReset = true;
            config.wait_write.WaitOne(10000); //Ждем пока закончится запись файла или 10 секунд
            switch (CurrentWDT)
            {
                case WDT_ONBOARD:
                    timer2.Stop();
                    wdt.SetWDT(1);
                    toolStripProgressBar1.Value = 0;
                    pbTT.SetToolTip(this.statusStrip1, "WDT reset computer after 1 min.");
                    break;
                case WDT_SOFT:
                    try
                    {
                        ExitWindows.Reboot(true);
                    }
                    catch (Exception ex)
                    {
                        WriteEventLog(String.Format("Exception caught in resetToolStripMenuItem_Click(): {0}", ex.ToString()), EventLogEntryType.Error);
                    }
                    break;
                case WDT_USBOPEN:
                    Thread.Sleep(30000); //Ресет этого типа происходит практически моментально, поэтому, на всякий случай ждем еще 30 секунд, чтобы сбросились буферы
                    if (!wdt_o.ResetTest())
                        WriteEventLog(wdt_o.GetReport(), EventLogEntryType.Error);
                    break;
            }
        }
        private bool sendMail(string msg)
        {
            try
            {
                MailAddress from = new MailAddress(this.tbMailFrom.Text);
                MailAddress to = new MailAddress(this.tbMailTo.Text);
                MailMessage message = new MailMessage(from, to);
                message.Subject = this.tbSubject.Text;
                message.Body = msg;
                //В поле должно быть указано Server, port
                string[] elements = this.tbSmtpServer.Text.Split(',');
                string server = elements[0];
                //Если попытка преобразовать строку в число не удалась, считаем порт по умолчаию 25
                int port;
                if (elements.Length > 1)
                {
                    if (!int.TryParse(elements[1], out port))
                        port = 25;
                }
                else
                    port = 25;
                SmtpClient client = new SmtpClient(server, port);
                //Если в поле пароль по умолчанию, то берем его из настроек, если нет (поменяли) то берем из поля формы
                if (this.tbPassword.Text == rand)
                {
                    //Если в настройках не пусто и не пароль по умолчанию, то авторизуемся, иначе нет смысла авторизоваться
                    if (!String.IsNullOrEmpty(Decrypt(config.conf.mailset.tbPassword)) && Decrypt(config.conf.mailset.tbPassword) != rand)
                        client.Credentials = new NetworkCredential(this.tbMailFrom.Text, Decrypt(config.conf.mailset.tbPassword));
                }
                else
                    client.Credentials = new NetworkCredential(this.tbMailFrom.Text, this.tbPassword.Text);
                client.EnableSsl = this.cbEnableSSL.Checked;
                //Фиксит ошибку при установке флага SSL: System.Security.Authentication.AuthenticationException: Удаленный сертификат недействителен согласно результатам проверки подлинности.
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                client.Send(message);
            }
            catch (Exception ex)
            {
                WriteEventLog(String.Format("Exception caught in sendMail(): {0}", ex.ToString()), EventLogEntryType.Error);
                return false;
            }
            return true;
        }
        private void Send_TestMail(object sender, EventArgs e)
        {
            if (sendMail("Test"))
                MessageBox.Show("Sending a test mail message successfully", "Test mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show("An error occurred while sending mail message\nFor details, see the eventlog", "Test mail", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        private void SaveMailSetting(object sender, EventArgs e)
        {
            //Send mail setting
            if ((!String.IsNullOrEmpty(this.tbPassword.Text)) && (this.tbPassword.Text != rand))
            {
                config.conf.mailset.tbPassword = Encrypt(this.tbPassword.Text);
                this.tbPassword.Text = rand;
            }
            config.conf.mailset.tbSmtpServer = this.tbSmtpServer.Text;
            config.conf.mailset.tbMailFrom = this.tbMailFrom.Text;
            config.conf.mailset.tbMailTo = this.tbMailTo.Text;
            config.conf.mailset.tbSubject = this.tbSubject.Text;
            config.conf.mailset.cbEnableSSL = this.cbEnableSSL.Checked;
            config.conf.mailset.cbOnEmail = this.cbOnEmail.Checked;
            config.conf.mailset.cbOnSendStart = this.cbOnSendStart.Checked;
            config.WriteParam(ref config_path);
            MessageBox.Show("Mail settings save successfully", "Save mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void SaveBotSetting(object sender, EventArgs e)
        {
            //Bot setting
            config.conf.botset.textBotToken = this.textBotToken.Text;
            config.conf.botset.textBotName = this.textBotName.Text;
            config.conf.botset.textBotSendTo = this.textBotSendTo.Text;
            config.conf.botset.textFermaName = this.textFermaName.Text;
            config.conf.botset.cbTelegramOn = this.cbTelegramOn.Checked;
            config.conf.botset.cbResponceCmd = this.cbResponceCmd.Checked;
            config.WriteParam(ref config_path);
            MessageBox.Show("Bot settings save successfully", "Save bot", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void RestoreSetting()
        {
            if (!String.IsNullOrEmpty(Decrypt(config.conf.mailset.tbPassword)))
                this.tbPassword.Text = rand;
            this.tbSmtpServer.Text = config.conf.mailset.tbSmtpServer;
            this.tbMailFrom.Text = config.conf.mailset.tbMailFrom;
            this.tbMailTo.Text = config.conf.mailset.tbMailTo;
            this.tbSubject.Text = config.conf.mailset.tbSubject;
            this.cbEnableSSL.Checked = config.conf.mailset.cbEnableSSL;
            this.cbOnEmail.Checked = config.conf.mailset.cbOnEmail;
            this.cbOnSendStart.Checked = config.conf.mailset.cbOnSendStart;
            //Monitoring setting
            this.checkCoreClock.Checked = config.conf.monset.checkCoreClock;
            this.checkMemoryClock.Checked = config.conf.monset.checkMemoryClock;
            this.checkGPULoad.Checked = config.conf.monset.checkGPULoad;
            this.checkMemCtrlLoad.Checked = config.conf.monset.checkMemCtrlLoad;
            this.checkGPUTemp.Checked = config.conf.monset.checkGPUTemp;
            this.checkFanLoad.Checked = config.conf.monset.checkFanLoad;
            this.checkFanRPM.Checked = config.conf.monset.checkFanRPM;
            this.nc_K_gpu_clock.Value = config.conf.monset.nc_K_gpu_clock;
            this.nc_K_mem_clock.Value = config.conf.monset.nc_K_mem_clock;
            this.nc_K_gpu_load.Value = config.conf.monset.nc_K_gpu_load;
            this.nc_K_mem_load.Value = config.conf.monset.nc_K_mem_load;
            this.nc_K_gpu_temp.Value = config.conf.monset.nc_K_gpu_temp;
            this.nc_K_fan_speed_p.Value = config.conf.monset.nc_K_fan_speed_p;
            this.nc_K_fan_speed_r.Value = config.conf.monset.nc_K_fan_speed_r;
            this.nc_Span_integration.Value = config.conf.monset.nc_Span_integration;
            this.nc_DelayFailover.Value = config.conf.monset.nc_DelayFailover;
            this.nc_DelayFailoverNext.Value = config.conf.monset.nc_DelayFailoverNext;
            this.nc_DelayMon.Value = config.conf.monset.nc_DelayMon;
            this.cb_NoUp.Checked = config.conf.monset.cb_NoUp;
            //Bot setting
            this.textBotToken.Text = config.conf.botset.textBotToken;
            this.textBotName.Text = config.conf.botset.textBotName;
            this.textBotSendTo.Text = config.conf.botset.textBotSendTo;
            this.textFermaName.Text = config.conf.botset.textFermaName;
            this.cbTelegramOn.Checked = config.conf.botset.cbTelegramOn;
            this.cbResponceCmd.Checked = config.conf.botset.cbResponceCmd;
            //WDT setting, восстанавливаем значения переменных и элементов управления формы
            WDtimer = config.conf.wdtset.WDtimer;
            this.numericTimeout.Value = WDtimer;
            CurrentWDT = config.conf.wdtset.CurrentWDT;
            switch (CurrentWDT)
            {
                case WDT_SOFT:
                    radioSoftWDT.Checked = true;
                    break;
                case WDT_ONBOARD:
                    radioOnboardWDT.Checked = true;
                    break;
                case WDT_USBOPEN:
                    radioOpendevUSBWDT.Checked = true;
                    break;
                default:
                    radioSoftWDT.Checked = true;
                    break;
            }
        }
        public static string Encrypt(string data)
        {
            if(String.IsNullOrEmpty(data))
                return null;
            // setup the encryption algorithm
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(rand, 8);
            Rijndael aes = Rijndael.Create();
            aes.IV = keyGenerator.GetBytes(aes.BlockSize / 8);
            aes.Key = keyGenerator.GetBytes(aes.KeySize / 8);
            // encrypt the data
            byte[] rawData = Encoding.Unicode.GetBytes(data);
            using(MemoryStream memoryStream = new MemoryStream())
            try
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    memoryStream.Write(keyGenerator.Salt, 0, keyGenerator.Salt.Length);
                    cryptoStream.Write(rawData, 0, rawData.Length);
                    cryptoStream.Close();
                    byte[] encrypted = memoryStream.ToArray();
                    char [] outArr = new char[encrypted.Length * 2];
                    int chlen = Convert.ToBase64CharArray(encrypted, 0, encrypted.Length, outArr, 0);
                    return new string(outArr, 0, chlen);
                }
            }
            catch { return null; }
        }
        public static string Decrypt(string data)
        {
            if(String.IsNullOrEmpty(data))
                return null;
            byte[] rawData = new byte[data.Length];
            try
            {
                rawData = Convert.FromBase64CharArray(data.ToCharArray(), 0, data.Length);
            }
            catch { return null; }
            if(rawData.Length < 8)
                throw new ArgumentException("Invalid input data");
            // setup the decryption algorithm
            byte[] salt = new byte[8];
            for(int i = 0; i < salt.Length; i++)
                salt[i] = rawData[i];
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(rand, salt);
            Rijndael aes = Rijndael.Create();
            aes.IV = keyGenerator.GetBytes(aes.BlockSize / 8);
            aes.Key = keyGenerator.GetBytes(aes.KeySize / 8);
            // decrypt the data
            using(MemoryStream memoryStream = new MemoryStream())
            try
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(rawData, 8, rawData.Length - 8);
                    cryptoStream.Close();
                    byte[] decrypted = memoryStream.ToArray();
                    return Encoding.Unicode.GetString(decrypted);
                }
            }
            catch 
            { return null; }
        }
        private bool botInit()
        {
            if ((!String.IsNullOrEmpty(this.textBotToken.Text)) && (!String.IsNullOrEmpty(this.textBotName.Text)))
            {
                //Инициализируем основные элементы бота
                bot.token = this.textBotToken.Text;
                if (!String.IsNullOrEmpty(config.conf.botset.botChatID))
                    bot.chatID = config.conf.botset.botChatID;
                //Получаем имя бота, если соответствует ожидаемому, считаем, что все работает и запускаем цикл сообщений бота, если нет, пишем в лог
                User us = bot.GetMe();
                if ((us != null) && (us.Username == this.textBotName.Text))
                {
                    bot.bInit = true;
                    this.timer3.Start();
                    return true;
                }
                else
                    WriteEventLog("You Telegram bot " + this.textBotName.Text + " not init and not work.\nCheck the bot settings.", EventLogEntryType.Error);
            }
            return false;
        }
        private string getParam(int par)
        {
            StringBuilder report = new StringBuilder();
            for (int i = 0; i != CardCount; i++)
                report.AppendLine("Slot " + gpupar[i].Slot.ToString() + ": " + gpupar[i].GPUParams[par].ParCollect.Last().ToString());
            return report.ToString();
        }
        private void timer3Tick(object sender, EventArgs e)
        {
            //Останавливаем таймер, чтобы исключить повторный запуск цикла сообщений бота
            timer3.Stop();
            //Обработка сообщений для бота
            botMessageCycle();
            //Вновь запускаем таймер
            timer3.Start();
        }
        private void Send_TestBot(object sender, EventArgs e)
        {
            if (botInit())
                MessageBox.Show("Telegram Bot started successfully\nPlease send command to Bot", "Test Telegram Bot", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show("An error occurred while starting Bot\nFor details, see the eventlog", "Test Telegram Bot", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        private void botMessageCycle()
        {
            bool flagrestart = false;
            botUpdate = bot.GetUpdates((bot.lastUpd).ToString());
            if (botUpdate != null)
            {
                foreach (var upd in botUpdate)
                {
                    //Если сообщение уже обработано, идем дальше
                    if (bot.lastUpd < upd.UpdateId)
                    {
                        //Берем сообщения только конкретного пользователя
                        if (upd.Message.Chat.Username == this.textBotSendTo.Text)
                        {
                            //Сохраняем чатИД
                            bot.chatID = upd.Message.Chat.Id.ToString();
                            //Обрабатываем Цикл команд, если установлен соотвествующий флаг, если флаг сброшен, то единственная польза цикла получить чат ИД для уведомлений.
                            if (this.cbResponceCmd.Checked)
                            {
                                switch (upd.Message.Text)
                                {
                                    case "/fgpu":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label1.Text + "\n" + getParam(0), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/fmem":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label2.Text + "\n" + getParam(1), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/lgpu":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label3.Text + "\n" + getParam(2), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/lmem":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label4.Text + "\n" + getParam(3), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/tgpu":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label5.Text + "\n" + getParam(4), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/fanp":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label6.Text + "\n" + getParam(5), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/fanr":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": " + this.label7.Text + "\n" + getParam(6), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/all":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ":\n" +
                                            this.label1.Text + "\n" + getParam(0) + "\n" +
                                            this.label2.Text + "\n" + getParam(1) + "\n" +
                                            this.label3.Text + "\n" + getParam(2) + "\n" +
                                            this.label4.Text + "\n" + getParam(3) + "\n" +
                                            this.label5.Text + "\n" + getParam(4) + "\n" +
                                            this.label6.Text + "\n" + getParam(5) + "\n" +
                                            this.label7.Text + "\n" + getParam(6), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/resetget":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": flag reset is " + (!fReset).ToString(), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/reseton":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": flag reset is " + (!fReset).ToString(), "", upd.Message.MessageId.ToString());
                                        fReset = false;
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": flag reset set to " + (!fReset).ToString(), "", upd.Message.MessageId.ToString());
                                        break;
                                    case "/resetoff":
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": flag reset is " + (!fReset).ToString(), "", upd.Message.MessageId.ToString());
                                        fReset = true;
                                        bot.SendMessage(bot.chatID, this.textFermaName.Text + ": flag reset set to " + (!fReset).ToString(), "", upd.Message.MessageId.ToString());
                                        break;
                                    default:
                                        //Проверяем, не рестарт ли
                                        flagrestart = upd.Message.Text.Equals("/reset " + textFermaName.Text);
                                        if (flagrestart)
                                        {
                                            //чтобы избежать ошибки при увеличении UpdateId, подстрахуемся, при этом upd.UpdateId сохранит старое значение, а очередь очистится значением Int32.MaxValue
                                            int id = upd.UpdateId;
                                            try { id++; }
                                            catch { id = Int32.MaxValue; }
                                            //Чтобы избежать последующего ресета при перезагрузке фермы, очищаем очередь на сервере
                                            bot.GetUpdates(id.ToString());
                                            //Посылаем реквест
                                            bot.SendMessage(bot.chatID, this.textFermaName.Text + " restarts ...", "", upd.Message.MessageId.ToString());
                                        }
                                        break;
                                }
                            }
                            //Сохраняем ИД сообщения для очистки очереди
                            bot.lastUpd = upd.UpdateId;
                        }
                    }
                    //Сохраняем последний чатИД, чтобы бот мог ответить
                    if (!String.IsNullOrEmpty(bot.chatID))
                    {
                        if (!config.conf.botset.botChatID.Equals(bot.chatID))
                        {
                            config.conf.botset.botChatID = bot.chatID;
                            config.WriteParam(ref config_path);
                        }
                    }
                }
                //Защита от спама: если запросы были не мои, чтобы не копились
                if (botUpdate.Count > 10)
                    bot.lastUpd = botUpdate[botUpdate.Count - 1].UpdateId;
            }
            if (flagrestart)
                Reset_Click(null, null);
        }
        private void SaveMonitoringSetting(object sender, EventArgs e)
        {
            //Monitoring setting
            config.conf.monset.checkCoreClock = this.checkCoreClock.Checked;
            config.conf.monset.checkMemoryClock = this.checkMemoryClock.Checked;
            config.conf.monset.checkGPULoad = this.checkGPULoad.Checked;
            config.conf.monset.checkMemCtrlLoad = this.checkMemCtrlLoad.Checked;
            config.conf.monset.checkGPUTemp = this.checkGPUTemp.Checked;
            config.conf.monset.checkFanLoad = this.checkFanLoad.Checked;
            config.conf.monset.checkFanRPM = this.checkFanRPM.Checked;
            config.conf.monset.nc_K_gpu_clock = this.nc_K_gpu_clock.Value;
            config.conf.monset.nc_K_mem_clock = this.nc_K_mem_clock.Value;
            config.conf.monset.nc_K_gpu_load = this.nc_K_gpu_load.Value;
            config.conf.monset.nc_K_mem_load = this.nc_K_mem_load.Value;
            config.conf.monset.nc_K_gpu_temp = this.nc_K_gpu_temp.Value;
            config.conf.monset.nc_K_fan_speed_p = this.nc_K_fan_speed_p.Value;
            config.conf.monset.nc_K_fan_speed_r = this.nc_K_fan_speed_r.Value;
            config.conf.monset.nc_Span_integration = this.nc_Span_integration.Value;
            config.conf.monset.nc_DelayFailover = this.nc_DelayFailover.Value;
            config.conf.monset.nc_DelayFailoverNext = this.nc_DelayFailoverNext.Value;
            config.conf.monset.nc_DelayMon = this.nc_DelayMon.Value;
            config.conf.monset.cb_NoUp = this.cb_NoUp.Checked;
            config.WriteParam(ref config_path);
            SetMonitoringSetting(); //Устанавливаем текущие параметры мониторинга из котролов в переменные
            MessageBox.Show("Monitoring settings save successfully", "Save monitoring", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void PauseAfterStart(object sender, EventArgs e)
        {
            //Задержка на время timer4 после старта программы
            this.timer4.Stop();
        }
        private void EstimateDuration(object sender, EventArgs e)
        {
            //Оценка времени превышения порога наблюдения за параметрами при известном интервале наблюдения, K, Min и Max значенииях
            // K*((Min*tau) + (Max * (T - tau)))/T = Max => T*Max/K = Min*tau + Max*T - Max*tau => T*Max/K - T*Max = (Min - Max)*tau => tau = T*Max(1 - 1/K)/(Max-Min)
            int K_est, Min_est, Max_est;
            bool fK, fMin, fMax;
            fK = int.TryParse(this.tb_K_est.Text, out K_est);
            fMin = int.TryParse(this.tb_Min_est.Text, out Min_est);
            fMax = int.TryParse(this.tb_Max_est.Text, out Max_est);
            if (fK && fMin && fMax && (Max_est > 0) && (Min_est >= 0) && (K_est > 0) && (Max_est > Min_est))
            {
                int tau = (int)(this.nc_Span_integration.Value);
                tau = tau * Max_est;
                tau = tau - tau / K_est;
                tau = tau / (Max_est - Min_est);
                MessageBox.Show("Approximate time operation the monitoring\nafter fails is " + tau.ToString() + " sec.", "Estimate the duration", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
                MessageBox.Show("Approximate time operation the monitoring\nafter fails can not be calculated\nCheck variables values", "Estimate the duration", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void ResetDefaultParam(object sender, EventArgs e)
        {
            this.checkCoreClock.Checked = false;
            this.checkMemoryClock.Checked = false;
            this.checkGPULoad.Checked = false;
            this.checkMemCtrlLoad.Checked = false;
            this.checkGPUTemp.Checked = false;
            this.checkFanLoad.Checked = false;
            this.checkFanRPM.Checked = false;
            this.nc_K_gpu_clock.Value = 2.0M;
            this.nc_K_mem_clock.Value = 2.0M;
            this.nc_K_gpu_load.Value = 2.0M;
            this.nc_K_mem_load.Value = 2.0M;
            this.nc_K_gpu_temp.Value = 1.5M;
            this.nc_K_fan_speed_p.Value = 1.5M;
            this.nc_K_fan_speed_r.Value = 1.5M;
            this.nc_Span_integration.Value = 60M;
            this.nc_DelayFailover.Value = 20M;
            this.nc_DelayFailoverNext.Value = 10M;
            this.nc_DelayMon.Value = 60M;
            this.cb_NoUp.Checked = true;
            config.conf.othset.cmd_Script = "";
            config.WriteParam(ref config_path);
        }
        private void SoftReset(object sender, EventArgs e)
        {
            //Счетчик минут для OpenDev и Soft таймеров
            switch (CurrentWDT)
            {
                case WDT_SOFT: //перезагружает софтовый таймер при окончании счета
                    if (wdt_s.Count == 0) 
                        Reset_Click(sender, e);
                    wdt_s.Count--;
                    break;
                case WDT_USBOPEN:
                    wdt_o.Count--;
                    break;
            }
        }
        private void runCmd()
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe") 
                { UseShellExecute = false, RedirectStandardInput = false, Arguments = "/c " + config.conf.othset.cmd_Script };
            Process proc = new Process() { StartInfo = psi };
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                WriteEventLog("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message, EventLogEntryType.Information);
            }
        }
        private void SaveWDTSetting(object sender, EventArgs e)
        {
            config.conf.wdtset.WDtimer = (Byte) this.numericTimeout.Value;
            config.conf.wdtset.wdtPort = cbCOMPort.Text;
            if (radioSoftWDT.Checked)
                config.conf.wdtset.CurrentWDT = WDT_SOFT;
            if (radioOnboardWDT.Checked)
                config.conf.wdtset.CurrentWDT = WDT_ONBOARD;
            if (radioOpendevUSBWDT.Checked)
                config.conf.wdtset.CurrentWDT = WDT_USBOPEN;
            config.WriteParam(ref config_path);
            MessageBox.Show("WDT settings save successfully\nNew values will be valid after the restart of the program", "Save WDT setting", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void FindComPorts()
        {
            try
            {
                //Перечисляем все СОМ порты в системе
                string[] ports = SerialPort.GetPortNames();
                //Заполняем комбо бокс найденными значениями
                if (ports.Length > 0)
                {
                    for (int i = 0; i < ports.Length; i++)
                    {
                        cbCOMPort.Items.Add(ports.GetValue(i));
                        //Устанавливаем нужный СОМ порт текущим
                        if (ports.GetValue(i).Equals(config.conf.wdtset.wdtPort))
                            cbCOMPort.SelectedIndex = i;
                    }
                }
            }
            catch
            {
                WriteEventLog("Error serial port: GetPortNames()", EventLogEntryType.Information);
            }
        }
        private void TestPortWDT(object sender, EventArgs e)
        {
            if (CurrentWDT == WDT_USBOPEN)
            {
                if (cbCOMPort.Text.Equals(wdt_o.PortName))
                {
                    MessageBox.Show("Рort " + cbCOMPort.Text + " is already open", "Test WDT port", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            OpenWDT w = new OpenWDT(cbCOMPort.Text);
            if (w.isWDT)
            {
                this.radioOpendevUSBWDT.Enabled = true;
                MessageBox.Show("Open WDT found on " + cbCOMPort.Text, "Test WDT port", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                WriteEventLog(w.GetReport(), EventLogEntryType.Warning);
                MessageBox.Show("Open WDT not found on port " + cbCOMPort.Text + "\nFor details, see the eventlog", "Test WDT port", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
