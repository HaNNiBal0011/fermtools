using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace fermtools
{
    class SettingsJSON
    {
        public SetingRoot conf;
        public Mutex wait_write;  //Ждать пока не закончтся запись
        public SettingsJSON()
        {
            conf = new SetingRoot();
            wait_write = new Mutex();
        }
        public bool ReadParam(ref string config_path)
        {
            try
            {
                StreamReader sr = new StreamReader(config_path);
                string json = sr.ReadToEnd();
                sr.Close();
                conf = JsonConvert.DeserializeObject<SetingRoot>(json);
            }
            catch { return false; }
            if (conf == null)
            {
                conf = new SetingRoot();
                return false;
            }
            return true;
        }
        public bool WriteParam(ref string config_path)
        {
            wait_write.WaitOne();
            bool res = false;
            try
            {
                StreamWriter sw = new StreamWriter(config_path, false);
                try
                {
                    string json = JsonConvert.SerializeObject(conf, Formatting.Indented);
                    sw.Write(json);
                    res = true;
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
            wait_write.ReleaseMutex();
            return res;
        }
        public bool WriteParamDefault(ref string config_path)
        {
            conf.monset.checkCoreClock = false;
            conf.monset.checkMemoryClock = false;
            conf.monset.checkGPULoad = false;
            conf.monset.checkMemCtrlLoad = false;
            conf.monset.checkGPUTemp = false;
            conf.monset.checkFanLoad = false;
            conf.monset.checkFanRPM = false;
            conf.monset.nc_K_gpu_clock = 2.0M;
            conf.monset.nc_K_mem_clock = 2.0M;
            conf.monset.nc_K_gpu_load = 2.0M;
            conf.monset.nc_K_mem_load = 2.0M;
            conf.monset.nc_K_gpu_temp = 1.5M;
            conf.monset.nc_K_fan_speed_p = 1.5M;
            conf.monset.nc_K_fan_speed_r = 1.5M;
            conf.monset.nc_Span_integration = 60M;
            conf.monset.nc_DelayFailover = 60M;
            conf.monset.nc_DelayFailoverNext = 10M;
            conf.monset.nc_DelayMon = 60M;
            conf.monset.cb_NoUp = true;

            conf.wdtset.CurrentWDT = 0;
            conf.wdtset.WDtimer = 10;
            conf.wdtset.wdtPort = "";

            conf.miner.bClaymoreStat = false;
            conf.miner.bClaymoreMon = false;
            conf.miner.ClaymorePort = 3333;
            conf.miner.bPoolConnect = false;

            conf.mailset.tbSmtpServer = "";
            conf.mailset.tbMailFrom = "";
            conf.mailset.tbMailTo = "";
            conf.mailset.tbSubject = "";
            conf.mailset.tbPassword = "";
            conf.mailset.cbEnableSSL = false;
            conf.mailset.cbOnEmail = false;
            conf.mailset.cbOnSendStart = false;

            conf.botset.textBotToken = "";
            conf.botset.textBotName = "";
            conf.botset.textBotSendTo = "";
            conf.botset.textFermaName = "";
            conf.botset.botChatID = "";
            conf.botset.cbTelegramOn = false;
            conf.botset.cbResponceCmd = false;

            conf.othset.cb_startPipe = false;
            conf.othset.isReset = true;
            conf.othset.cmd_Script = "";
            conf.othset.GPUCount = 0;
            conf.othset.CompareGPUCountReset = false;
            conf.othset.SendMinerStat = false;

            return WriteParam(ref config_path); ;
        }
    }
    public class SetingRoot
    {
        public SetingRoot()
        {
            monset = new MonitoringSettings();
            wdtset = new WDTSettings();
            miner = new MinerSettings();
            mailset = new MailSettings();
            botset = new BotSettings();
            othset = new OtherSettings();
        }
        public MonitoringSettings monset { get; set; }
        public WDTSettings wdtset { get; set; }
        public MinerSettings miner { get; set; }
        public MailSettings mailset { get; set; }
        public BotSettings botset { get; set; }
        public OtherSettings othset { get; set; }
    }
    public class MonitoringSettings
    {
        //Monitoring setting
        public bool checkCoreClock { get; set; }
        public bool checkMemoryClock { get; set; }
        public bool checkGPULoad { get; set; }
        public bool checkMemCtrlLoad { get; set; }
        public bool checkGPUTemp { get; set; }
        public bool checkFanLoad { get; set; }
        public bool checkFanRPM { get; set; }
        public decimal nc_K_gpu_clock { get; set; }
        public decimal nc_K_mem_clock { get; set; }
        public decimal nc_K_gpu_load { get; set; }
        public decimal nc_K_mem_load { get; set; }
        public decimal nc_K_gpu_temp { get; set; }
        public decimal nc_K_fan_speed_p { get; set; }
        public decimal nc_K_fan_speed_r { get; set; }
        public decimal nc_Span_integration { get; set; }
        public decimal nc_DelayFailover { get; set; }
        public decimal nc_DelayFailoverNext { get; set; }
        public decimal nc_DelayMon { get; set; }
        public bool cb_NoUp { get; set; }
    }
    public class WDTSettings
    {
        //WDT setting
        public byte WDtimer { get; set; }
        public int CurrentWDT { get; set; }
        public string wdtPort { get; set; }
    }

    public class MinerSettings
    {
        //WDT setting
        public bool bClaymoreStat { get; set; }
        public bool bClaymoreMon { get; set; }
        public int ClaymorePort { get; set; }
        public bool bPoolConnect { get; set; }
    }
    public class MailSettings
    {
        //Send mail setting
        public string tbSmtpServer { get; set; }
        public string tbMailFrom { get; set; }
        public string tbMailTo { get; set; }
        public string tbSubject { get; set; }
        public string tbPassword { get; set; }
        public bool cbEnableSSL { get; set; }
        public bool cbOnEmail { get; set; }
        public bool cbOnSendStart { get; set; }
    }
    public class BotSettings
    {
        //Bot setting
        public string textBotToken { get; set; }
        public string textBotName { get; set; }
        public string textBotSendTo { get; set; }
        public string textFermaName { get; set; }
        public bool cbTelegramOn { get; set; }
        public bool cbResponceCmd { get; set; }
        public string botChatID { get; set; }
    }
    public class OtherSettings
    {
        //Other setting
        public bool cb_startPipe { get; set; }
        public string cmd_Script { get; set; }
        public bool isReset { get; set; }
        public int GPUCount { get; set; }
        public bool CompareGPUCountReset { get; set; }
        public bool SendMinerStat { get; set; }
    }

}
