 ﻿// Copyright © 2016 Dimasin. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fermtools
{
    class WDT
    {
        //Константы для работы с WDT
        private readonly ushort[] REGISTER_PORTS = new ushort[] { 0x2E, 0x4E };
        private readonly ushort[] VALUE_PORTS = new ushort[] { 0x2F, 0x4F };
        private const byte DEVCIE_SELECT_REGISTER = 0x07;
        private const byte CHIP_ID_REGISTER = 0x20;
        private const byte CHIP_REVISION_REGISTER = 0x21;

        public string WDTnameChip;                                      //Наименование чипа WDT
        private ushort WDTnumPort;                                      //Порядковый номер порта в массивах REGISTER_PORTS и VALUE_PORTS
        public readonly bool isWDT;                                     //Флаг наличия чипа WDT
        public readonly StringBuilder report = new StringBuilder();     //Для отчета

        public WDT()
        {
            bool isDriver;  //Флаг успешной загрузки драйвера
            //Грузим драйвер для операций IO
            Ring0.Open(AppDomain.CurrentDomain.BaseDirectory);
            if (Ring0.IsOpen)
            {
                report.AppendLine("Kernel driver is loaded.");
                isDriver = true;
            }
            else
            {
                report.AppendLine("Kernel driver not loaded, see next for detals.");
                report.AppendLine(Ring0.GetReport());
                isDriver = false;
                isWDT = false;
            }
            //Проверяем, есть ли микросхема
            if (isDriver)
            {
                WDTnumPort = GetWDTChip();
                if (WDTnumPort >= REGISTER_PORTS.Length)
                {
                    report.AppendLine("WDT chip not found. Only software reset.");
                    isWDT = false;
                }
                else
                {
                    report.AppendLine("Found chip " + WDTnameChip + "\nRegister port " + String.Format("0x{0:X}", REGISTER_PORTS[WDTnumPort]));
                    isWDT = true;
                }
            }
            if (!isWDT) 
                WDTnameChip = "Software";
        }
        private ushort GetWDTChip()
         {
             ushort i = 0xFF;
             
             for (i = 0; i < REGISTER_PORTS.Length; i++)
             {
                 byte id = 0;
                 byte revision = 0;
                 if (Ring0.WaitIsaBusMutex(100))
                 {
                     //Unlock W83627
                     Ring0.WriteIoPort(REGISTER_PORTS[i], 0x87);
                     Ring0.WriteIoPort(REGISTER_PORTS[i], 0x87);
                     //Read chip id
                     Ring0.WriteIoPort(REGISTER_PORTS[i], CHIP_ID_REGISTER);
                     id = Ring0.ReadIoPort(VALUE_PORTS[i]);
                     //Read chip version
                     Ring0.WriteIoPort(REGISTER_PORTS[i], CHIP_REVISION_REGISTER);
                     revision = Ring0.ReadIoPort(VALUE_PORTS[i]);
                     //Lock W83627
                     Ring0.WriteIoPort(REGISTER_PORTS[i], 0xAA);
                     Ring0.ReleaseIsaBusMutex();
                 }
                 else 
                 {
                     report.AppendLine("Get WDT Chip: ISA bus error.");
                     return 0xFF;
                 }
                 switch (id)
                 {
                     case 0x52:
                         switch (revision)
                         {
                             case 0x17:
                             case 0x3A:
                             case 0x41: WDTnameChip = "W83627HF"; return i;
                         } break;
                     case 0x82:
                         switch (revision & 0xF0)
                         {
                             case 0x80: WDTnameChip = "W83627THF"; return i;
                         } break;
                     case 0x85:
                         switch (revision)
                         {
                             case 0x41: WDTnameChip = "W83687THF"; return i;
                         } break;
                     case 0x88:
                         switch (revision & 0xF0)
                         {
                             case 0x50:
                             case 0x60: WDTnameChip = "W83627EHF"; return i;
                         } break;
                     case 0xA0:
                         switch (revision & 0xF0)
                         {
                             case 0x20: WDTnameChip = "W83627DHG"; return i;
                         } break;
                     case 0xA5:
                         switch (revision & 0xF0)
                         {
                             case 0x10: WDTnameChip = "W83667HG"; return i;
                         } break;
                     case 0xB0:
                         switch (revision & 0xF0)
                         {
                             case 0x70: WDTnameChip = "W83627DHGP"; return i;
                         } break;
                     case 0xB3:
                         switch (revision & 0xF0)
                         {
                             case 0x50: WDTnameChip = "W83667HGB"; return i;
                         } break;
                     case 0xC3:
                         switch (revision & 0xF0)
                         {
                             case 0x30: WDTnameChip = "NCT6776F"; return i;
                         } break;
                     case 0xC5:
                         switch (revision & 0xF0)
                         {
                             case 0x60: WDTnameChip = "NCT6779D"; return i;
                         } break;
                     /*case 0xC8:
                         switch (revision)
                         {
                             case 0x03: WDTnameChip = "NCT6791D"; return i;
                         } break;*/
                 }
             }
             return i;
         }
        public bool SetWDT(byte count)
        {
            report.Clear();
            if (Ring0.WaitIsaBusMutex(100))
            {
                //Unlock W83627
                //mov dx,02eh
                //mov al,087h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x87);
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x87);

                //Select register of watchdog timer
                //mov al,07h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x07);
                //inc dx
                //mov al,08h
                //out dx,al
                Ring0.WriteIoPort(VALUE_PORTS[WDTnumPort], 0x08);

                //Enable the function of watchdog timer
                //dec dx
                //mov al,030h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x30);
                //inc dx
                //mov al,01h
                //out dx,al
                Ring0.WriteIoPort(VALUE_PORTS[WDTnumPort], 0x01);

                //Set minute as counting unit and enable the WDTO#
                //dec dx
                //mov al,0f5h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0xF5);
                //inc dx
                //in al,dx
                byte xx = Ring0.ReadIoPort(VALUE_PORTS[WDTnumPort]);
                //or al,08h
                //Select Watchdog Timer I count mode. 0: Second Mode. 1: Minute Mode.
                xx |= 0x08;
                //Enable the rising edge of a KBC reset (P20) to issue a time-out event. 0: Disable. 1: Enable.
                //or al,04h
                //xx |= 0x04;
                //or al,02h
                //Disable / Enable the Watchdog Timer I output low pulse to the KBRST# pin (PIN28) 0: Disable. 1: Enable.
                xx |= 0x02;
                //FOR NCT6779D Pulse or Level mode select 0: Pulse mode 1: Level mode
                //xx |= 0x01;
                //out dx,al
                Ring0.WriteIoPort(VALUE_PORTS[WDTnumPort], xx);

                //Set timeout interval as COUNT minute and start
                //dec dx
                //mov al,0f6h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0xF6);
                //inc dx
                //mov al,count
                //out dx,al
                Ring0.WriteIoPort(VALUE_PORTS[WDTnumPort], count);

                //Lock W83627
                //dec dx
                //mov al,0aah
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0xAA);
                Ring0.ReleaseIsaBusMutex();
                return true;
            }
            else 
                report.AppendLine("ISA bus error. Watchdog timer not set.");
            return false;
        }
        public byte GetWDT()
        {
            byte xx = 0;
            report.Clear();
            if (Ring0.WaitIsaBusMutex(100))
            {
                //Unlock W83627
                //mov dx,02eh
                //mov al,087h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x87);
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x87);

                //Select register of watchdog timer
                //mov al,07h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x07);
                //inc dx
                //mov al,08h
                //out dx,al
                Ring0.WriteIoPort(VALUE_PORTS[WDTnumPort], 0x08);

                //Enable the function of watchdog timer
                //dec dx
                //mov al,030h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0x30);
                //inc dx
                //mov al,01h
                //out dx,al
                Ring0.WriteIoPort(VALUE_PORTS[WDTnumPort], 0x01);

                //Select register port and read
                //dec dx
                //mov al,0f5h
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0xF6);
                //inc dx
                //in al,dx
                xx = Ring0.ReadIoPort(VALUE_PORTS[WDTnumPort]);

                //Lock W83627
                //dec dx
                //mov al,0aah
                //out dx,al
                Ring0.WriteIoPort(REGISTER_PORTS[WDTnumPort], 0xAA);
                Ring0.ReleaseIsaBusMutex();
            }
            else 
                report.AppendLine("ISA bus error. Watchdog timer not get.");
            return xx;
        }
        public string GetReport()
        {
            return report.ToString();
        }
    }
}
