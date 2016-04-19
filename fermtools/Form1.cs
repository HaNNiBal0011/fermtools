using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        bool fMessage;              //Устанавливается, если выведена сообщение перезагрузки
        byte WDtimer;               //Интервал в минутах для записи в WatchDog Timer
        Thread pipeServerTh;        //Поток для работы именованного канала
        ManualResetEvent signal;    //Сигнал для асинхронного чтения из pipe или завершения серверного процесса
        WDT wdt;                    //WatchDog Timer
        ToolTip pbTT;               //Инфа для отображения состояния WDT
        int CardCount;              //Количество найденных видеокарт

        private List<GPUParam> gpupar = new List<GPUParam>();               //Коллекция Параметров GPU
        private List<System.Windows.Forms.TextBox> par = new List<System.Windows.Forms.TextBox>();          //Коллекция текст боксов для отображения параметров видеокарт
        private List<System.Windows.Forms.CheckBox> check = new List<System.Windows.Forms.CheckBox>();      //Коллекция чек боксов для отметки отслеживания параметров
        private List<System.Windows.Forms.Label> label = new List<System.Windows.Forms.Label>();            //Коллекция меток для вывода названий параметров

        public Form1(string[] args)
        {
            InitializeComponent();
            RestoreSetting();
            fExitCancel = true; //Запрещаем выход из программы
            fReset = false; //Перезагрузка не инициализирована
            fMessage = false; //Сообщение не выведено
            nvigr = new NvidiaGroup(ref gpupar, NumPar); //Добавляем в группу видеокарты NVIDIA
            atigr = new ATIGroup(ref gpupar, NumPar); //Группа видеокарт ATI
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
            //Помещаем метки (названия параметров) в коллекцию для возможности доступа по индексу
            this.label.Add(this.label1);
            this.label.Add(this.label2);
            this.label.Add(this.label3);
            this.label.Add(this.label4);
            this.label.Add(this.label5);
            this.label.Add(this.label6);
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
            string repmon = "";
            //Цикл тика 1 секунда, нстраивается в графическом конструкторе свойств
            for (int i = 0; i != CardCount; i++)
            {
                gpupar[i].Update(TickCountMax);
                for (int j = 0; j != NumPar; j++)
                    this.par[j+i*NumPar].Text = gpupar[i].GPUParams[j].ParCollect.Last().ToString();
            }
            //Мониторим, если не инициирована перезагрузка, если что то не так, перезагружаем комп
            if (!fReset && !fMessage)
            {
                if (Monitoring(ref repmon))
                {
                    fMessage = true;
                    Thread msgsh = new Thread(MessageShowTh);
                    msgsh.Start(repmon);
                }
            }
        }
        private void MessageShowTh(object rep)
        {
            DialogResult dlg = AutoClosingMessageBox.Show("The monitoring system has identified the wrong setting:\n" + rep + "the computer prepares to reset\n" +
                "If a reset is not required, disable the monitoring", "Fermtool reset", 20000);
            //Если нажали ОК (или само нажалось) то запускаем процесс перезагрузки, иначе нажали Отмена и ждем 10 сек для реакции на сработку.
            if (dlg == System.Windows.Forms.DialogResult.OK)
            {
                object sender = null; EventArgs e = null;
                resetToolStripMenuItem_Click(sender, e);
            }
            else
            {
                Thread.Sleep(10000);
                fMessage = false;
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
                        //Проверяем уменьшилось ли среднее значение ниже MAX/RATE
                        if (gpupar[j].GPUParams[i].ParCollect.Average() < gpupar[j].GPUParams[i].ParCollect.Max()/gpupar[j].GPUParams[i].Rate)
                        {
                            //Если уменьшилось сообщаем в репорт и устанавливаем флаг
                            report.AppendLine(gpupar[j].GPUName + ", subsys: " + gpupar[j].Subsys + ", slot: " + gpupar[j].Slot.ToString());
                            report.AppendLine(label[i].Text + " - average: " + ((int)gpupar[j].GPUParams[i].ParCollect.Average()).ToString() + ", maximum: " +
                                gpupar[j].GPUParams[i].ParCollect.Max().ToString());
                            report.AppendLine();
                            res = true;
                        }
                    }
                }
            }
            //Если что то где то упало, сообщаем в EVENLOG и шлем e-mail
            if (res)
            {
                rep = report.ToString();
                WriteEventLog(rep, EventLogEntryType.Error);
                sendMail(rep);
            }
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
                toolStripProgressBar1.Value = 0;
                pbTT.SetToolTip(this.statusStrip1, "WDT reset computer after 1 min.");
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
                MessageBox.Show("An error occurred while sending mail message\nFor details, see the eventlog", "Test mail", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void SaveSetting(object sender, EventArgs e)
        {
            //Send mail setting
            if (!String.IsNullOrEmpty(this.tbPassword.Text))
                Properties.Settings.Default.Password = Encrypt(this.tbPassword.Text, "pass");
            else
                Properties.Settings.Default.Password = "";
            Properties.Settings.Default.SMTPServer = this.tbSmtpServer.Text;
            Properties.Settings.Default.MailFrom = this.tbMailFrom.Text;
            Properties.Settings.Default.MailTo = this.tbMailTo.Text;
            Properties.Settings.Default.Subject = this.tbSubject.Text;
            Properties.Settings.Default.EnableSSL = this.cbEnableSSL.Checked;
            //Monitoring setting
            Properties.Settings.Default.mGPUClock = this.checkCoreClock.Checked;
            Properties.Settings.Default.mMemClock = this.checkMemoryClock.Checked;
            Properties.Settings.Default.mGPULoad = this.checkGPULoad.Checked;
            Properties.Settings.Default.mMemLoad = this.checkMemCtrlLoad.Checked;
            Properties.Settings.Default.mGPUTemp = this.checkGPUTemp.Checked;
            Properties.Settings.Default.mFanLoad = this.checkFanLoad.Checked;
            Properties.Settings.Default.mFanRPM = this.checkFanRPM.Checked;
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings save successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void RestoreSetting()
        {
            //Send mail setting
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Password))
                this.tbPassword.Text = Decrypt(Properties.Settings.Default.Password, "pass");
            else
                this.tbPassword.Text = "";
            this.tbSmtpServer.Text = Properties.Settings.Default.SMTPServer;
            this.tbMailFrom.Text = Properties.Settings.Default.MailFrom;
            this.tbMailTo.Text = Properties.Settings.Default.MailTo;
            this.tbSubject.Text = Properties.Settings.Default.Subject;
            this.cbEnableSSL.Checked = Properties.Settings.Default.EnableSSL;
            //Monitoring setting
            this.checkCoreClock.Checked = Properties.Settings.Default.mGPUClock;
            this.checkMemoryClock.Checked = Properties.Settings.Default.mMemClock;
            this.checkGPULoad.Checked = Properties.Settings.Default.mGPULoad;
            this.checkMemCtrlLoad.Checked = Properties.Settings.Default.mMemLoad;
            this.checkGPUTemp.Checked = Properties.Settings.Default.mGPUTemp;
            this.checkFanLoad.Checked = Properties.Settings.Default.mFanLoad;
            this.checkFanRPM.Checked = Properties.Settings.Default.mFanRPM;
        }
        public static string Encrypt(string data, string password)
        {
            if(String.IsNullOrEmpty(data))
                return null;
            if(String.IsNullOrEmpty(password))
                return null;
            // setup the encryption algorithm
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, 8);
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
                    return Encoding.Unicode.GetString(encrypted);
                }
            }
            catch { return null; }
        }
        public static string Decrypt(string data, string password)
        {
            if(String.IsNullOrEmpty(data))
                return null;
            if(String.IsNullOrEmpty(password))
                return null;
            byte[] rawData = Encoding.Unicode.GetBytes(data);
            if(rawData.Length < 8)
                throw new ArgumentException("Invalid input data");
            // setup the decryption algorithm
            byte[] salt = new byte[8];
            for(int i = 0; i < salt.Length; i++)
                salt[i] = rawData[i];
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt);
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
            catch { return null; }
        }
    }
}
