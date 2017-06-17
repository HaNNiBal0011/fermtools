// Copyright © 2016 Dimasin. All rights reserved.

namespace fermtools
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.MenuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGPU = new System.Windows.Forms.TabPage();
            this.checkFanRPM = new System.Windows.Forms.CheckBox();
            this.checkFanLoad = new System.Windows.Forms.CheckBox();
            this.checkGPUTemp = new System.Windows.Forms.CheckBox();
            this.checkMemCtrlLoad = new System.Windows.Forms.CheckBox();
            this.checkGPULoad = new System.Windows.Forms.CheckBox();
            this.checkMemoryClock = new System.Windows.Forms.CheckBox();
            this.checkCoreClock = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMonitoring = new System.Windows.Forms.TabPage();
            this.bt_ResetDefault = new System.Windows.Forms.Button();
            this.bt_Calc = new System.Windows.Forms.Button();
            this.tb_Max_est = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.tb_Min_est = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tb_K_est = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cb_NoUp = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.nc_DelayMon = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.nc_DelayFailoverNext = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.nc_DelayFailover = new System.Windows.Forms.NumericUpDown();
            this.btSaveMon = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.nc_Span_integration = new System.Windows.Forms.NumericUpDown();
            this.nc_K_fan_speed_r = new System.Windows.Forms.NumericUpDown();
            this.nc_K_fan_speed_p = new System.Windows.Forms.NumericUpDown();
            this.nc_K_gpu_temp = new System.Windows.Forms.NumericUpDown();
            this.nc_K_mem_load = new System.Windows.Forms.NumericUpDown();
            this.nc_K_gpu_load = new System.Windows.Forms.NumericUpDown();
            this.nc_K_mem_clock = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.nc_K_gpu_clock = new System.Windows.Forms.NumericUpDown();
            this.tabWDT = new System.Windows.Forms.TabPage();
            this.btMiner = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.tbClaymorPort = new System.Windows.Forms.TextBox();
            this.chClaymoreMon = new System.Windows.Forms.CheckBox();
            this.chClaymoreStat = new System.Windows.Forms.CheckBox();
            this.btTestPort = new System.Windows.Forms.Button();
            this.label32 = new System.Windows.Forms.Label();
            this.cbCOMPort = new System.Windows.Forms.ComboBox();
            this.btSaveWDT = new System.Windows.Forms.Button();
            this.numericTimeout = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBoxWDT = new System.Windows.Forms.GroupBox();
            this.radioOpendevUSBWDT = new System.Windows.Forms.RadioButton();
            this.radioSoftWDT = new System.Windows.Forms.RadioButton();
            this.radioOnboardWDT = new System.Windows.Forms.RadioButton();
            this.tabEmail = new System.Windows.Forms.TabPage();
            this.cbOnSendStart = new System.Windows.Forms.CheckBox();
            this.cbOnEmail = new System.Windows.Forms.CheckBox();
            this.btSaveMail = new System.Windows.Forms.Button();
            this.cbEnableSSL = new System.Windows.Forms.CheckBox();
            this.btSendTest = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbMailTo = new System.Windows.Forms.TextBox();
            this.tbMailFrom = new System.Windows.Forms.TextBox();
            this.tbSmtpServer = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabTelegram = new System.Windows.Forms.TabPage();
            this.btBotSave = new System.Windows.Forms.Button();
            this.btBotTest = new System.Windows.Forms.Button();
            this.cbResponceCmd = new System.Windows.Forms.CheckBox();
            this.cbTelegramOn = new System.Windows.Forms.CheckBox();
            this.textFermaName = new System.Windows.Forms.TextBox();
            this.textBotSendTo = new System.Windows.Forms.TextBox();
            this.textBotName = new System.Windows.Forms.TextBox();
            this.textBotToken = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.timerSoft = new System.Windows.Forms.Timer(this.components);
            this.timerMiner = new System.Windows.Forms.Timer(this.components);
            this.MenuContext.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabGPU.SuspendLayout();
            this.tabMonitoring.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nc_DelayMon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_DelayFailoverNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_DelayFailover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_Span_integration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_fan_speed_r)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_fan_speed_p)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_gpu_temp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_mem_load)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_gpu_load)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_mem_clock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_gpu_clock)).BeginInit();
            this.tabWDT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).BeginInit();
            this.groupBoxWDT.SuspendLayout();
            this.tabEmail.SuspendLayout();
            this.tabTelegram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.MenuContext;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FermTools";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MenuContext
            // 
            this.MenuContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.showToolStripMenuItem,
            this.toolStripMenuItem1});
            this.MenuContext.Name = "MenuContext";
            this.MenuContext.ShowImageMargin = false;
            this.MenuContext.Size = new System.Drawing.Size(90, 76);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.Reset_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.Show_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 24);
            this.toolStripMenuItem1.Text = "Exit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.Exit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 264);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(508, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 19);
            this.toolStripProgressBar1.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGPU);
            this.tabControl1.Controls.Add(this.tabMonitoring);
            this.tabControl1.Controls.Add(this.tabWDT);
            this.tabControl1.Controls.Add(this.tabEmail);
            this.tabControl1.Controls.Add(this.tabTelegram);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(508, 264);
            this.tabControl1.TabIndex = 2;
            // 
            // tabGPU
            // 
            this.tabGPU.Controls.Add(this.checkFanRPM);
            this.tabGPU.Controls.Add(this.checkFanLoad);
            this.tabGPU.Controls.Add(this.checkGPUTemp);
            this.tabGPU.Controls.Add(this.checkMemCtrlLoad);
            this.tabGPU.Controls.Add(this.checkGPULoad);
            this.tabGPU.Controls.Add(this.checkMemoryClock);
            this.tabGPU.Controls.Add(this.checkCoreClock);
            this.tabGPU.Controls.Add(this.label7);
            this.tabGPU.Controls.Add(this.label6);
            this.tabGPU.Controls.Add(this.label5);
            this.tabGPU.Controls.Add(this.label4);
            this.tabGPU.Controls.Add(this.label3);
            this.tabGPU.Controls.Add(this.label2);
            this.tabGPU.Controls.Add(this.label1);
            this.tabGPU.Location = new System.Drawing.Point(4, 25);
            this.tabGPU.Name = "tabGPU";
            this.tabGPU.Padding = new System.Windows.Forms.Padding(3);
            this.tabGPU.Size = new System.Drawing.Size(500, 235);
            this.tabGPU.TabIndex = 0;
            this.tabGPU.Text = "GPU Info";
            this.tabGPU.UseVisualStyleBackColor = true;
            // 
            // checkFanRPM
            // 
            this.checkFanRPM.AutoSize = true;
            this.checkFanRPM.Location = new System.Drawing.Point(479, 196);
            this.checkFanRPM.Name = "checkFanRPM";
            this.checkFanRPM.Size = new System.Drawing.Size(18, 17);
            this.checkFanRPM.TabIndex = 20;
            this.checkFanRPM.UseVisualStyleBackColor = true;
            // 
            // checkFanLoad
            // 
            this.checkFanLoad.AutoSize = true;
            this.checkFanLoad.Location = new System.Drawing.Point(479, 166);
            this.checkFanLoad.Name = "checkFanLoad";
            this.checkFanLoad.Size = new System.Drawing.Size(18, 17);
            this.checkFanLoad.TabIndex = 19;
            this.checkFanLoad.UseVisualStyleBackColor = true;
            // 
            // checkGPUTemp
            // 
            this.checkGPUTemp.AutoSize = true;
            this.checkGPUTemp.Location = new System.Drawing.Point(479, 136);
            this.checkGPUTemp.Name = "checkGPUTemp";
            this.checkGPUTemp.Size = new System.Drawing.Size(18, 17);
            this.checkGPUTemp.TabIndex = 18;
            this.checkGPUTemp.UseVisualStyleBackColor = true;
            // 
            // checkMemCtrlLoad
            // 
            this.checkMemCtrlLoad.AutoSize = true;
            this.checkMemCtrlLoad.Location = new System.Drawing.Point(479, 106);
            this.checkMemCtrlLoad.Name = "checkMemCtrlLoad";
            this.checkMemCtrlLoad.Size = new System.Drawing.Size(18, 17);
            this.checkMemCtrlLoad.TabIndex = 17;
            this.checkMemCtrlLoad.UseVisualStyleBackColor = true;
            // 
            // checkGPULoad
            // 
            this.checkGPULoad.AutoSize = true;
            this.checkGPULoad.Location = new System.Drawing.Point(479, 76);
            this.checkGPULoad.Name = "checkGPULoad";
            this.checkGPULoad.Size = new System.Drawing.Size(18, 17);
            this.checkGPULoad.TabIndex = 16;
            this.checkGPULoad.UseVisualStyleBackColor = true;
            // 
            // checkMemoryClock
            // 
            this.checkMemoryClock.AutoSize = true;
            this.checkMemoryClock.Location = new System.Drawing.Point(479, 46);
            this.checkMemoryClock.Name = "checkMemoryClock";
            this.checkMemoryClock.Size = new System.Drawing.Size(18, 17);
            this.checkMemoryClock.TabIndex = 15;
            this.checkMemoryClock.UseVisualStyleBackColor = true;
            // 
            // checkCoreClock
            // 
            this.checkCoreClock.AutoSize = true;
            this.checkCoreClock.Location = new System.Drawing.Point(479, 16);
            this.checkCoreClock.Name = "checkCoreClock";
            this.checkCoreClock.Size = new System.Drawing.Size(18, 17);
            this.checkCoreClock.TabIndex = 14;
            this.checkCoreClock.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "FAN Speed, RPM";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "FAN Speed, %";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "GPU Temperature, *C";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Memory Controller Load,%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "GPU Load, %";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "GPU Memory Clock, MHz";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "GPU Core Clock, MHz";
            // 
            // tabMonitoring
            // 
            this.tabMonitoring.Controls.Add(this.bt_ResetDefault);
            this.tabMonitoring.Controls.Add(this.bt_Calc);
            this.tabMonitoring.Controls.Add(this.tb_Max_est);
            this.tabMonitoring.Controls.Add(this.label30);
            this.tabMonitoring.Controls.Add(this.tb_Min_est);
            this.tabMonitoring.Controls.Add(this.label29);
            this.tabMonitoring.Controls.Add(this.tb_K_est);
            this.tabMonitoring.Controls.Add(this.label28);
            this.tabMonitoring.Controls.Add(this.cb_NoUp);
            this.tabMonitoring.Controls.Add(this.label27);
            this.tabMonitoring.Controls.Add(this.nc_DelayMon);
            this.tabMonitoring.Controls.Add(this.label26);
            this.tabMonitoring.Controls.Add(this.nc_DelayFailoverNext);
            this.tabMonitoring.Controls.Add(this.label25);
            this.tabMonitoring.Controls.Add(this.nc_DelayFailover);
            this.tabMonitoring.Controls.Add(this.btSaveMon);
            this.tabMonitoring.Controls.Add(this.label24);
            this.tabMonitoring.Controls.Add(this.nc_Span_integration);
            this.tabMonitoring.Controls.Add(this.nc_K_fan_speed_r);
            this.tabMonitoring.Controls.Add(this.nc_K_fan_speed_p);
            this.tabMonitoring.Controls.Add(this.nc_K_gpu_temp);
            this.tabMonitoring.Controls.Add(this.nc_K_mem_load);
            this.tabMonitoring.Controls.Add(this.nc_K_gpu_load);
            this.tabMonitoring.Controls.Add(this.nc_K_mem_clock);
            this.tabMonitoring.Controls.Add(this.label17);
            this.tabMonitoring.Controls.Add(this.label18);
            this.tabMonitoring.Controls.Add(this.label19);
            this.tabMonitoring.Controls.Add(this.label20);
            this.tabMonitoring.Controls.Add(this.label21);
            this.tabMonitoring.Controls.Add(this.label22);
            this.tabMonitoring.Controls.Add(this.label23);
            this.tabMonitoring.Controls.Add(this.nc_K_gpu_clock);
            this.tabMonitoring.Location = new System.Drawing.Point(4, 25);
            this.tabMonitoring.Name = "tabMonitoring";
            this.tabMonitoring.Size = new System.Drawing.Size(500, 235);
            this.tabMonitoring.TabIndex = 3;
            this.tabMonitoring.Text = "Monitoring setting";
            this.tabMonitoring.UseVisualStyleBackColor = true;
            // 
            // bt_ResetDefault
            // 
            this.bt_ResetDefault.Location = new System.Drawing.Point(386, 194);
            this.bt_ResetDefault.Name = "bt_ResetDefault";
            this.bt_ResetDefault.Size = new System.Drawing.Size(89, 29);
            this.bt_ResetDefault.TabIndex = 44;
            this.bt_ResetDefault.Text = "Reset def";
            this.bt_ResetDefault.UseVisualStyleBackColor = true;
            this.bt_ResetDefault.Click += new System.EventHandler(this.ResetDefaultParam);
            // 
            // bt_Calc
            // 
            this.bt_Calc.Location = new System.Drawing.Point(293, 194);
            this.bt_Calc.Name = "bt_Calc";
            this.bt_Calc.Size = new System.Drawing.Size(89, 29);
            this.bt_Calc.TabIndex = 43;
            this.bt_Calc.Text = "Estimate";
            this.bt_Calc.UseVisualStyleBackColor = true;
            this.bt_Calc.Click += new System.EventHandler(this.EstimateDuration);
            // 
            // tb_Max_est
            // 
            this.tb_Max_est.Location = new System.Drawing.Point(401, 163);
            this.tb_Max_est.Name = "tb_Max_est";
            this.tb_Max_est.Size = new System.Drawing.Size(70, 22);
            this.tb_Max_est.TabIndex = 42;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(369, 164);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(37, 17);
            this.label30.TabIndex = 41;
            this.label30.Text = "Max:";
            // 
            // tb_Min_est
            // 
            this.tb_Min_est.Location = new System.Drawing.Point(293, 163);
            this.tb_Min_est.Name = "tb_Min_est";
            this.tb_Min_est.Size = new System.Drawing.Size(70, 22);
            this.tb_Min_est.TabIndex = 40;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(261, 164);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(34, 17);
            this.label29.TabIndex = 39;
            this.label29.Text = "Min:";
            // 
            // tb_K_est
            // 
            this.tb_K_est.Location = new System.Drawing.Point(203, 163);
            this.tb_K_est.Name = "tb_K_est";
            this.tb_K_est.Size = new System.Drawing.Size(52, 22);
            this.tb_K_est.TabIndex = 38;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(184, 164);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(21, 17);
            this.label28.TabIndex = 37;
            this.label28.Text = "K:";
            // 
            // cb_NoUp
            // 
            this.cb_NoUp.Checked = true;
            this.cb_NoUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_NoUp.Location = new System.Drawing.Point(184, 135);
            this.cb_NoUp.Name = "cb_NoUp";
            this.cb_NoUp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cb_NoUp.Size = new System.Drawing.Size(251, 21);
            this.cb_NoUp.TabIndex = 36;
            this.cb_NoUp.Text = "No react to up";
            this.cb_NoUp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_NoUp.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(184, 101);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(226, 34);
            this.label27.TabIndex = 35;
            this.label27.Text = "Delay enable the monitoring after start, sec:";
            // 
            // nc_DelayMon
            // 
            this.nc_DelayMon.Location = new System.Drawing.Point(416, 104);
            this.nc_DelayMon.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nc_DelayMon.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_DelayMon.Name = "nc_DelayMon";
            this.nc_DelayMon.Size = new System.Drawing.Size(55, 22);
            this.nc_DelayMon.TabIndex = 34;
            this.nc_DelayMon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_DelayMon.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(184, 67);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(226, 34);
            this.label26.TabIndex = 33;
            this.label26.Text = "Timeout display the next message of fault, sec:";
            // 
            // nc_DelayFailoverNext
            // 
            this.nc_DelayFailoverNext.Location = new System.Drawing.Point(416, 74);
            this.nc_DelayFailoverNext.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nc_DelayFailoverNext.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_DelayFailoverNext.Name = "nc_DelayFailoverNext";
            this.nc_DelayFailoverNext.Size = new System.Drawing.Size(55, 22);
            this.nc_DelayFailoverNext.TabIndex = 32;
            this.nc_DelayFailoverNext.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_DelayFailoverNext.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(184, 33);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(226, 34);
            this.label25.TabIndex = 31;
            this.label25.Text = "Timeout display the message of fault, sec:";
            // 
            // nc_DelayFailover
            // 
            this.nc_DelayFailover.Location = new System.Drawing.Point(416, 44);
            this.nc_DelayFailover.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nc_DelayFailover.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_DelayFailover.Name = "nc_DelayFailover";
            this.nc_DelayFailover.Size = new System.Drawing.Size(55, 22);
            this.nc_DelayFailover.TabIndex = 30;
            this.nc_DelayFailover.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_DelayFailover.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // btSaveMon
            // 
            this.btSaveMon.Location = new System.Drawing.Point(200, 194);
            this.btSaveMon.Name = "btSaveMon";
            this.btSaveMon.Size = new System.Drawing.Size(89, 29);
            this.btSaveMon.TabIndex = 29;
            this.btSaveMon.Text = "Save";
            this.btSaveMon.UseVisualStyleBackColor = true;
            this.btSaveMon.Click += new System.EventHandler(this.SaveMonitoringSetting);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(184, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(226, 17);
            this.label24.TabIndex = 28;
            this.label24.Text = "Span calculating the average, sec:";
            // 
            // nc_Span_integration
            // 
            this.nc_Span_integration.Location = new System.Drawing.Point(416, 14);
            this.nc_Span_integration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nc_Span_integration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_Span_integration.Name = "nc_Span_integration";
            this.nc_Span_integration.Size = new System.Drawing.Size(55, 22);
            this.nc_Span_integration.TabIndex = 27;
            this.nc_Span_integration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_Span_integration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // nc_K_fan_speed_r
            // 
            this.nc_K_fan_speed_r.DecimalPlaces = 1;
            this.nc_K_fan_speed_r.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_fan_speed_r.Location = new System.Drawing.Point(117, 194);
            this.nc_K_fan_speed_r.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_fan_speed_r.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_fan_speed_r.Name = "nc_K_fan_speed_r";
            this.nc_K_fan_speed_r.Size = new System.Drawing.Size(52, 22);
            this.nc_K_fan_speed_r.TabIndex = 26;
            this.nc_K_fan_speed_r.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_fan_speed_r.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // nc_K_fan_speed_p
            // 
            this.nc_K_fan_speed_p.DecimalPlaces = 1;
            this.nc_K_fan_speed_p.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_fan_speed_p.Location = new System.Drawing.Point(117, 164);
            this.nc_K_fan_speed_p.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_fan_speed_p.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_fan_speed_p.Name = "nc_K_fan_speed_p";
            this.nc_K_fan_speed_p.Size = new System.Drawing.Size(52, 22);
            this.nc_K_fan_speed_p.TabIndex = 25;
            this.nc_K_fan_speed_p.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_fan_speed_p.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // nc_K_gpu_temp
            // 
            this.nc_K_gpu_temp.DecimalPlaces = 1;
            this.nc_K_gpu_temp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_gpu_temp.Location = new System.Drawing.Point(117, 134);
            this.nc_K_gpu_temp.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_gpu_temp.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_gpu_temp.Name = "nc_K_gpu_temp";
            this.nc_K_gpu_temp.Size = new System.Drawing.Size(52, 22);
            this.nc_K_gpu_temp.TabIndex = 24;
            this.nc_K_gpu_temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_gpu_temp.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // nc_K_mem_load
            // 
            this.nc_K_mem_load.DecimalPlaces = 1;
            this.nc_K_mem_load.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_mem_load.Location = new System.Drawing.Point(117, 104);
            this.nc_K_mem_load.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_mem_load.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_mem_load.Name = "nc_K_mem_load";
            this.nc_K_mem_load.Size = new System.Drawing.Size(52, 22);
            this.nc_K_mem_load.TabIndex = 23;
            this.nc_K_mem_load.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_mem_load.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // nc_K_gpu_load
            // 
            this.nc_K_gpu_load.DecimalPlaces = 1;
            this.nc_K_gpu_load.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_gpu_load.Location = new System.Drawing.Point(117, 74);
            this.nc_K_gpu_load.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_gpu_load.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_gpu_load.Name = "nc_K_gpu_load";
            this.nc_K_gpu_load.Size = new System.Drawing.Size(52, 22);
            this.nc_K_gpu_load.TabIndex = 22;
            this.nc_K_gpu_load.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_gpu_load.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // nc_K_mem_clock
            // 
            this.nc_K_mem_clock.DecimalPlaces = 1;
            this.nc_K_mem_clock.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_mem_clock.Location = new System.Drawing.Point(117, 44);
            this.nc_K_mem_clock.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_mem_clock.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_mem_clock.Name = "nc_K_mem_clock";
            this.nc_K_mem_clock.Size = new System.Drawing.Size(52, 22);
            this.nc_K_mem_clock.TabIndex = 21;
            this.nc_K_mem_clock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_mem_clock.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 196);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(105, 17);
            this.label17.TabIndex = 20;
            this.label17.Text = "K fan_speed_r:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 166);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(108, 17);
            this.label18.TabIndex = 19;
            this.label18.Text = "K fan_speed_p:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 136);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 17);
            this.label19.TabIndex = 18;
            this.label19.Text = "K gpu_temp:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 106);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(90, 17);
            this.label20.TabIndex = 17;
            this.label20.Text = "K mem_load:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 76);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 17);
            this.label21.TabIndex = 16;
            this.label21.Text = "K gpu_load:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 46);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(95, 17);
            this.label22.TabIndex = 15;
            this.label22.Text = "K mem_clock:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(8, 16);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(89, 17);
            this.label23.TabIndex = 14;
            this.label23.Text = "K gpu_clock:";
            // 
            // nc_K_gpu_clock
            // 
            this.nc_K_gpu_clock.DecimalPlaces = 1;
            this.nc_K_gpu_clock.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nc_K_gpu_clock.Location = new System.Drawing.Point(117, 14);
            this.nc_K_gpu_clock.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nc_K_gpu_clock.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nc_K_gpu_clock.Name = "nc_K_gpu_clock";
            this.nc_K_gpu_clock.Size = new System.Drawing.Size(52, 22);
            this.nc_K_gpu_clock.TabIndex = 0;
            this.nc_K_gpu_clock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nc_K_gpu_clock.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // tabWDT
            // 
            this.tabWDT.Controls.Add(this.btMiner);
            this.tabWDT.Controls.Add(this.label33);
            this.tabWDT.Controls.Add(this.tbClaymorPort);
            this.tabWDT.Controls.Add(this.chClaymoreMon);
            this.tabWDT.Controls.Add(this.chClaymoreStat);
            this.tabWDT.Controls.Add(this.btTestPort);
            this.tabWDT.Controls.Add(this.label32);
            this.tabWDT.Controls.Add(this.cbCOMPort);
            this.tabWDT.Controls.Add(this.btSaveWDT);
            this.tabWDT.Controls.Add(this.numericTimeout);
            this.tabWDT.Controls.Add(this.label31);
            this.tabWDT.Controls.Add(this.groupBoxWDT);
            this.tabWDT.Location = new System.Drawing.Point(4, 25);
            this.tabWDT.Name = "tabWDT";
            this.tabWDT.Padding = new System.Windows.Forms.Padding(3);
            this.tabWDT.Size = new System.Drawing.Size(500, 235);
            this.tabWDT.TabIndex = 4;
            this.tabWDT.Text = "WDT setting";
            this.tabWDT.UseVisualStyleBackColor = true;
            // 
            // btMiner
            // 
            this.btMiner.Location = new System.Drawing.Point(42, 194);
            this.btMiner.Name = "btMiner";
            this.btMiner.Size = new System.Drawing.Size(89, 29);
            this.btMiner.TabIndex = 43;
            this.btMiner.Text = "Test miner";
            this.btMiner.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(282, 99);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(135, 17);
            this.label33.TabIndex = 42;
            this.label33.Text = "Claymore miner port";
            // 
            // tbClaymorPort
            // 
            this.tbClaymorPort.Location = new System.Drawing.Point(423, 96);
            this.tbClaymorPort.Name = "tbClaymorPort";
            this.tbClaymorPort.Size = new System.Drawing.Size(63, 22);
            this.tbClaymorPort.TabIndex = 41;
            this.tbClaymorPort.Text = "3333";
            this.tbClaymorPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chClaymoreMon
            // 
            this.chClaymoreMon.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chClaymoreMon.Location = new System.Drawing.Point(19, 127);
            this.chClaymoreMon.Name = "chClaymoreMon";
            this.chClaymoreMon.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chClaymoreMon.Size = new System.Drawing.Size(260, 25);
            this.chClaymoreMon.TabIndex = 38;
            this.chClaymoreMon.Text = "Monitoring Claymore miner statistics";
            this.chClaymoreMon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chClaymoreMon.UseVisualStyleBackColor = true;
            // 
            // chClaymoreStat
            // 
            this.chClaymoreStat.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chClaymoreStat.Location = new System.Drawing.Point(19, 96);
            this.chClaymoreStat.Name = "chClaymoreStat";
            this.chClaymoreStat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chClaymoreStat.Size = new System.Drawing.Size(230, 25);
            this.chClaymoreStat.TabIndex = 37;
            this.chClaymoreStat.Text = "Claymore miner statistics send";
            this.chClaymoreStat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chClaymoreStat.UseVisualStyleBackColor = true;
            // 
            // btTestPort
            // 
            this.btTestPort.Location = new System.Drawing.Point(135, 194);
            this.btTestPort.Name = "btTestPort";
            this.btTestPort.Size = new System.Drawing.Size(89, 29);
            this.btTestPort.TabIndex = 31;
            this.btTestPort.Text = "Test Port";
            this.btTestPort.UseVisualStyleBackColor = true;
            this.btTestPort.Click += new System.EventHandler(this.TestPortWDT);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(282, 70);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(102, 17);
            this.label32.TabIndex = 30;
            this.label32.Text = "Port USB WDT";
            // 
            // cbCOMPort
            // 
            this.cbCOMPort.FormattingEnabled = true;
            this.cbCOMPort.Location = new System.Drawing.Point(390, 66);
            this.cbCOMPort.Name = "cbCOMPort";
            this.cbCOMPort.Size = new System.Drawing.Size(97, 24);
            this.cbCOMPort.TabIndex = 3;
            // 
            // btSaveWDT
            // 
            this.btSaveWDT.Location = new System.Drawing.Point(228, 194);
            this.btSaveWDT.Name = "btSaveWDT";
            this.btSaveWDT.Size = new System.Drawing.Size(89, 29);
            this.btSaveWDT.TabIndex = 29;
            this.btSaveWDT.Text = "Save";
            this.btSaveWDT.UseVisualStyleBackColor = true;
            this.btSaveWDT.Click += new System.EventHandler(this.SaveWDTSetting);
            // 
            // numericTimeout
            // 
            this.numericTimeout.Location = new System.Drawing.Point(147, 68);
            this.numericTimeout.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericTimeout.Name = "numericTimeout";
            this.numericTimeout.Size = new System.Drawing.Size(55, 22);
            this.numericTimeout.TabIndex = 28;
            this.numericTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(16, 70);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(125, 17);
            this.label31.TabIndex = 8;
            this.label31.Text = "Timeout WDT, min";
            // 
            // groupBoxWDT
            // 
            this.groupBoxWDT.Controls.Add(this.radioOpendevUSBWDT);
            this.groupBoxWDT.Controls.Add(this.radioSoftWDT);
            this.groupBoxWDT.Controls.Add(this.radioOnboardWDT);
            this.groupBoxWDT.Location = new System.Drawing.Point(8, 6);
            this.groupBoxWDT.Name = "groupBoxWDT";
            this.groupBoxWDT.Size = new System.Drawing.Size(484, 52);
            this.groupBoxWDT.TabIndex = 0;
            this.groupBoxWDT.TabStop = false;
            this.groupBoxWDT.Text = "Select WDT";
            // 
            // radioOpendevUSBWDT
            // 
            this.radioOpendevUSBWDT.Location = new System.Drawing.Point(323, 21);
            this.radioOpendevUSBWDT.Name = "radioOpendevUSBWDT";
            this.radioOpendevUSBWDT.Size = new System.Drawing.Size(155, 24);
            this.radioOpendevUSBWDT.TabIndex = 2;
            this.radioOpendevUSBWDT.Text = "Opendev USB WDT";
            this.radioOpendevUSBWDT.UseVisualStyleBackColor = true;
            // 
            // radioSoftWDT
            // 
            this.radioSoftWDT.Checked = true;
            this.radioSoftWDT.Location = new System.Drawing.Point(8, 21);
            this.radioSoftWDT.Margin = new System.Windows.Forms.Padding(0);
            this.radioSoftWDT.Name = "radioSoftWDT";
            this.radioSoftWDT.Size = new System.Drawing.Size(125, 24);
            this.radioSoftWDT.TabIndex = 1;
            this.radioSoftWDT.TabStop = true;
            this.radioSoftWDT.Text = "Software WDT";
            this.radioSoftWDT.UseVisualStyleBackColor = true;
            // 
            // radioOnboardWDT
            // 
            this.radioOnboardWDT.Location = new System.Drawing.Point(166, 21);
            this.radioOnboardWDT.Name = "radioOnboardWDT";
            this.radioOnboardWDT.Size = new System.Drawing.Size(121, 24);
            this.radioOnboardWDT.TabIndex = 0;
            this.radioOnboardWDT.Text = "Onboard WDT";
            this.radioOnboardWDT.UseVisualStyleBackColor = true;
            // 
            // tabEmail
            // 
            this.tabEmail.Controls.Add(this.cbOnSendStart);
            this.tabEmail.Controls.Add(this.cbOnEmail);
            this.tabEmail.Controls.Add(this.btSaveMail);
            this.tabEmail.Controls.Add(this.cbEnableSSL);
            this.tabEmail.Controls.Add(this.btSendTest);
            this.tabEmail.Controls.Add(this.tbPassword);
            this.tabEmail.Controls.Add(this.label12);
            this.tabEmail.Controls.Add(this.tbSubject);
            this.tabEmail.Controls.Add(this.tbMailTo);
            this.tabEmail.Controls.Add(this.tbMailFrom);
            this.tabEmail.Controls.Add(this.tbSmtpServer);
            this.tabEmail.Controls.Add(this.label11);
            this.tabEmail.Controls.Add(this.label10);
            this.tabEmail.Controls.Add(this.label9);
            this.tabEmail.Controls.Add(this.label8);
            this.tabEmail.Location = new System.Drawing.Point(4, 25);
            this.tabEmail.Name = "tabEmail";
            this.tabEmail.Padding = new System.Windows.Forms.Padding(3);
            this.tabEmail.Size = new System.Drawing.Size(500, 235);
            this.tabEmail.TabIndex = 1;
            this.tabEmail.Text = "E-mail setting";
            this.tabEmail.UseVisualStyleBackColor = true;
            // 
            // cbOnSendStart
            // 
            this.cbOnSendStart.AutoSize = true;
            this.cbOnSendStart.Location = new System.Drawing.Point(17, 30);
            this.cbOnSendStart.Name = "cbOnSendStart";
            this.cbOnSendStart.Size = new System.Drawing.Size(174, 21);
            this.cbOnSendStart.TabIndex = 14;
            this.cbOnSendStart.Text = "Send e-mail after reset";
            this.cbOnSendStart.UseVisualStyleBackColor = true;
            // 
            // cbOnEmail
            // 
            this.cbOnEmail.AutoSize = true;
            this.cbOnEmail.Location = new System.Drawing.Point(17, 6);
            this.cbOnEmail.Name = "cbOnEmail";
            this.cbOnEmail.Size = new System.Drawing.Size(188, 21);
            this.cbOnEmail.TabIndex = 13;
            this.cbOnEmail.Text = "Enable e-mail notification";
            this.cbOnEmail.UseVisualStyleBackColor = true;
            // 
            // btSaveMail
            // 
            this.btSaveMail.Location = new System.Drawing.Point(228, 194);
            this.btSaveMail.Name = "btSaveMail";
            this.btSaveMail.Size = new System.Drawing.Size(89, 29);
            this.btSaveMail.TabIndex = 12;
            this.btSaveMail.Text = "Save";
            this.btSaveMail.UseVisualStyleBackColor = true;
            this.btSaveMail.Click += new System.EventHandler(this.SaveMailSetting);
            // 
            // cbEnableSSL
            // 
            this.cbEnableSSL.Location = new System.Drawing.Point(12, 194);
            this.cbEnableSSL.Name = "cbEnableSSL";
            this.cbEnableSSL.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbEnableSSL.Size = new System.Drawing.Size(117, 21);
            this.cbEnableSSL.TabIndex = 11;
            this.cbEnableSSL.Text = "Enable SSL";
            this.cbEnableSSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbEnableSSL.UseVisualStyleBackColor = true;
            // 
            // btSendTest
            // 
            this.btSendTest.Location = new System.Drawing.Point(135, 194);
            this.btSendTest.Name = "btSendTest";
            this.btSendTest.Size = new System.Drawing.Size(89, 29);
            this.btSendTest.TabIndex = 10;
            this.btSendTest.Text = "Send Test";
            this.btSendTest.UseVisualStyleBackColor = true;
            this.btSendTest.Click += new System.EventHandler(this.Send_TestMail);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(112, 166);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(205, 22);
            this.tbPassword.TabIndex = 9;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 17);
            this.label12.TabIndex = 8;
            this.label12.Text = "Password:";
            // 
            // tbSubject
            // 
            this.tbSubject.Location = new System.Drawing.Point(112, 138);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(205, 22);
            this.tbSubject.TabIndex = 7;
            // 
            // tbMailTo
            // 
            this.tbMailTo.Location = new System.Drawing.Point(112, 110);
            this.tbMailTo.Name = "tbMailTo";
            this.tbMailTo.Size = new System.Drawing.Size(205, 22);
            this.tbMailTo.TabIndex = 6;
            // 
            // tbMailFrom
            // 
            this.tbMailFrom.Location = new System.Drawing.Point(112, 82);
            this.tbMailFrom.Name = "tbMailFrom";
            this.tbMailFrom.Size = new System.Drawing.Size(205, 22);
            this.tbMailFrom.TabIndex = 5;
            // 
            // tbSmtpServer
            // 
            this.tbSmtpServer.Location = new System.Drawing.Point(112, 55);
            this.tbSmtpServer.Name = "tbSmtpServer";
            this.tbSmtpServer.Size = new System.Drawing.Size(205, 22);
            this.tbSmtpServer.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 141);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 17);
            this.label11.TabIndex = 3;
            this.label11.Text = "Subject:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 17);
            this.label10.TabIndex = 2;
            this.label10.Text = "Mail To:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Mail From:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "SMTP Server";
            // 
            // tabTelegram
            // 
            this.tabTelegram.Controls.Add(this.btBotSave);
            this.tabTelegram.Controls.Add(this.btBotTest);
            this.tabTelegram.Controls.Add(this.cbResponceCmd);
            this.tabTelegram.Controls.Add(this.cbTelegramOn);
            this.tabTelegram.Controls.Add(this.textFermaName);
            this.tabTelegram.Controls.Add(this.textBotSendTo);
            this.tabTelegram.Controls.Add(this.textBotName);
            this.tabTelegram.Controls.Add(this.textBotToken);
            this.tabTelegram.Controls.Add(this.label13);
            this.tabTelegram.Controls.Add(this.label14);
            this.tabTelegram.Controls.Add(this.label15);
            this.tabTelegram.Controls.Add(this.label16);
            this.tabTelegram.Location = new System.Drawing.Point(4, 25);
            this.tabTelegram.Name = "tabTelegram";
            this.tabTelegram.Padding = new System.Windows.Forms.Padding(3);
            this.tabTelegram.Size = new System.Drawing.Size(500, 235);
            this.tabTelegram.TabIndex = 2;
            this.tabTelegram.Text = "Telegram setting";
            this.tabTelegram.UseVisualStyleBackColor = true;
            // 
            // btBotSave
            // 
            this.btBotSave.Location = new System.Drawing.Point(228, 194);
            this.btBotSave.Name = "btBotSave";
            this.btBotSave.Size = new System.Drawing.Size(89, 29);
            this.btBotSave.TabIndex = 26;
            this.btBotSave.Text = "Save";
            this.btBotSave.UseVisualStyleBackColor = true;
            this.btBotSave.Click += new System.EventHandler(this.SaveBotSetting);
            // 
            // btBotTest
            // 
            this.btBotTest.Location = new System.Drawing.Point(135, 194);
            this.btBotTest.Name = "btBotTest";
            this.btBotTest.Size = new System.Drawing.Size(89, 29);
            this.btBotTest.TabIndex = 25;
            this.btBotTest.Text = "Test Bot";
            this.btBotTest.UseVisualStyleBackColor = true;
            this.btBotTest.Click += new System.EventHandler(this.Send_TestBot);
            // 
            // cbResponceCmd
            // 
            this.cbResponceCmd.AutoSize = true;
            this.cbResponceCmd.Location = new System.Drawing.Point(17, 30);
            this.cbResponceCmd.Name = "cbResponceCmd";
            this.cbResponceCmd.Size = new System.Drawing.Size(132, 21);
            this.cbResponceCmd.TabIndex = 24;
            this.cbResponceCmd.Text = "Command mode";
            this.cbResponceCmd.UseVisualStyleBackColor = true;
            // 
            // cbTelegramOn
            // 
            this.cbTelegramOn.AutoSize = true;
            this.cbTelegramOn.Location = new System.Drawing.Point(17, 6);
            this.cbTelegramOn.Name = "cbTelegramOn";
            this.cbTelegramOn.Size = new System.Drawing.Size(220, 21);
            this.cbTelegramOn.TabIndex = 23;
            this.cbTelegramOn.Text = "Enable messenger notification";
            this.cbTelegramOn.UseVisualStyleBackColor = true;
            // 
            // textFermaName
            // 
            this.textFermaName.Location = new System.Drawing.Point(112, 138);
            this.textFermaName.Name = "textFermaName";
            this.textFermaName.Size = new System.Drawing.Size(380, 22);
            this.textFermaName.TabIndex = 22;
            // 
            // textBotSendTo
            // 
            this.textBotSendTo.Location = new System.Drawing.Point(112, 110);
            this.textBotSendTo.Name = "textBotSendTo";
            this.textBotSendTo.Size = new System.Drawing.Size(380, 22);
            this.textBotSendTo.TabIndex = 21;
            // 
            // textBotName
            // 
            this.textBotName.Location = new System.Drawing.Point(112, 82);
            this.textBotName.Name = "textBotName";
            this.textBotName.Size = new System.Drawing.Size(380, 22);
            this.textBotName.TabIndex = 20;
            // 
            // textBotToken
            // 
            this.textBotToken.Location = new System.Drawing.Point(112, 55);
            this.textBotToken.Name = "textBotToken";
            this.textBotToken.Size = new System.Drawing.Size(380, 22);
            this.textBotToken.TabIndex = 19;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 141);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 17);
            this.label13.TabIndex = 18;
            this.label13.Text = "Name Ferm:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 113);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 17);
            this.label14.TabIndex = 17;
            this.label14.Text = "Send To:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 85);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 17);
            this.label15.TabIndex = 16;
            this.label15.Text = "Name Bot:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 58);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 17);
            this.label16.TabIndex = 15;
            this.label16.Text = "Token Bot:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // eventLog1
            // 
            this.eventLog1.Log = "Application";
            this.eventLog1.Source = "FermTools";
            this.eventLog1.SynchronizingObject = this;
            // 
            // timer2
            // 
            this.timer2.Interval = 10000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 5000;
            this.timer3.Tick += new System.EventHandler(this.timer3Tick);
            // 
            // timer4
            // 
            this.timer4.Interval = 60000;
            this.timer4.Tick += new System.EventHandler(this.PauseAfterStart);
            // 
            // timerSoft
            // 
            this.timerSoft.Interval = 60000;
            this.timerSoft.Tick += new System.EventHandler(this.SoftReset);
            // 
            // timerMiner
            // 
            this.timerMiner.Interval = 10000;
            this.timerMiner.Tick += new System.EventHandler(this.MinerStat);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(508, 286);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "FermTools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.MenuContext.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabGPU.ResumeLayout(false);
            this.tabGPU.PerformLayout();
            this.tabMonitoring.ResumeLayout(false);
            this.tabMonitoring.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nc_DelayMon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_DelayFailoverNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_DelayFailover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_Span_integration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_fan_speed_r)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_fan_speed_p)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_gpu_temp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_mem_load)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_gpu_load)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_mem_clock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nc_K_gpu_clock)).EndInit();
            this.tabWDT.ResumeLayout(false);
            this.tabWDT.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).EndInit();
            this.groupBoxWDT.ResumeLayout(false);
            this.tabEmail.ResumeLayout(false);
            this.tabEmail.PerformLayout();
            this.tabTelegram.ResumeLayout(false);
            this.tabTelegram.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip MenuContext;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGPU;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabEmail;
        private System.Windows.Forms.Timer timer1;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkCoreClock;
        private System.Windows.Forms.CheckBox checkFanRPM;
        private System.Windows.Forms.CheckBox checkFanLoad;
        private System.Windows.Forms.CheckBox checkGPUTemp;
        private System.Windows.Forms.CheckBox checkMemCtrlLoad;
        private System.Windows.Forms.CheckBox checkGPULoad;
        private System.Windows.Forms.CheckBox checkMemoryClock;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.Button btSendTest;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.TextBox tbMailTo;
        private System.Windows.Forms.TextBox tbMailFrom;
        private System.Windows.Forms.TextBox tbSmtpServer;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbEnableSSL;
        private System.Windows.Forms.Button btSaveMail;
        private System.Windows.Forms.CheckBox cbOnSendStart;
        private System.Windows.Forms.CheckBox cbOnEmail;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.TabPage tabTelegram;
        private System.Windows.Forms.CheckBox cbResponceCmd;
        private System.Windows.Forms.CheckBox cbTelegramOn;
        private System.Windows.Forms.TextBox textFermaName;
        private System.Windows.Forms.TextBox textBotSendTo;
        private System.Windows.Forms.TextBox textBotName;
        private System.Windows.Forms.TextBox textBotToken;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btBotSave;
        private System.Windows.Forms.Button btBotTest;
        private System.Windows.Forms.TabPage tabMonitoring;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown nc_K_gpu_clock;
        private System.Windows.Forms.NumericUpDown nc_K_fan_speed_r;
        private System.Windows.Forms.NumericUpDown nc_K_fan_speed_p;
        private System.Windows.Forms.NumericUpDown nc_K_gpu_temp;
        private System.Windows.Forms.NumericUpDown nc_K_mem_load;
        private System.Windows.Forms.NumericUpDown nc_K_gpu_load;
        private System.Windows.Forms.NumericUpDown nc_K_mem_clock;
        private System.Windows.Forms.Button btSaveMon;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.NumericUpDown nc_Span_integration;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.NumericUpDown nc_DelayFailoverNext;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.NumericUpDown nc_DelayFailover;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.NumericUpDown nc_DelayMon;
        private System.Windows.Forms.CheckBox cb_NoUp;
        private System.Windows.Forms.Button bt_Calc;
        private System.Windows.Forms.TextBox tb_Max_est;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox tb_Min_est;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tb_K_est;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button bt_ResetDefault;
        private System.Windows.Forms.Timer timerSoft;
        private System.Windows.Forms.TabPage tabWDT;
        private System.Windows.Forms.GroupBox groupBoxWDT;
        private System.Windows.Forms.RadioButton radioSoftWDT;
        private System.Windows.Forms.RadioButton radioOnboardWDT;
        private System.Windows.Forms.ComboBox cbCOMPort;
        private System.Windows.Forms.RadioButton radioOpendevUSBWDT;
        private System.Windows.Forms.NumericUpDown numericTimeout;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button btSaveWDT;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button btTestPort;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox tbClaymorPort;
        private System.Windows.Forms.CheckBox chClaymoreMon;
        private System.Windows.Forms.CheckBox chClaymoreStat;
        private System.Windows.Forms.Button btMiner;
        private System.Windows.Forms.Timer timerMiner;
    }
}

