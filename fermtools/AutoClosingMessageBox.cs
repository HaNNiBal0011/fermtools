// Copyright © 2016 Dimasin. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fermtools
{
    class AutoClosingMessageBox
    {
         System.Threading.Timer _timeoutTimer;
        string _caption;
        public AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed, null, timeout, System.Threading.Timeout.Infinite);
        }

        public static DialogResult Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
            return MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                //SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                SendMessage(mbWnd, WM_COMMAND, (BN_CLICKED << 16) | IDOK, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        const int WM_COMMAND = 0x0111;
        const int BN_CLICKED = 245;
        const int IDOK = 1;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
    }
}
