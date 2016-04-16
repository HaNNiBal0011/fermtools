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
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.MenuContext.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.MenuContext.Size = new System.Drawing.Size(90, 82);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(89, 26);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(89, 26);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.ShowtoolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 26);
            this.toolStripMenuItem1.Text = "Exit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
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
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(508, 264);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkFanRPM);
            this.tabPage1.Controls.Add(this.checkFanLoad);
            this.tabPage1.Controls.Add(this.checkGPUTemp);
            this.tabPage1.Controls.Add(this.checkMemCtrlLoad);
            this.tabPage1.Controls.Add(this.checkGPULoad);
            this.tabPage1.Controls.Add(this.checkMemoryClock);
            this.tabPage1.Controls.Add(this.checkCoreClock);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(500, 235);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GPU Info";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(500, 235);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setup";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
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
    }
}

