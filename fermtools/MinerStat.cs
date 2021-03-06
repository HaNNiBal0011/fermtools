﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;


namespace fermtools
{
    class MinerRemote
    {
        private CommandSet statcmd;
        private CommandSet restartcmd;
        private SatisticResult statres;
        private RestartResult restartres;
        public string server;
        public StringBuilder report;
        public int port;
        public int cardcount;
        public bool fPools;
        public List<int> hr = new List<int>();
        public MinerStat stat;
        public MinerRemote()
        {
            statcmd = new CommandSet("miner_getstat1");
            restartcmd = new CommandSet("miner_restart");
            statres = new SatisticResult();
            report = new StringBuilder();
            stat = new MinerStat();
            server = "127.0.0.1";
            port = 3333;
            cardcount = 0;
            fPools = true;
        }
        public MinerRemote(int p)
        {
            statcmd = new CommandSet("miner_getstat1");
            restartcmd = new CommandSet("miner_restart");
            statres = new SatisticResult();
            report = new StringBuilder();
            stat = new MinerStat();
            server = "127.0.0.1";
            port = p;
            cardcount = 0;
            fPools = true;
        }
        public bool GetStatistic()
        {
            bool res = false;
            report.Clear();
            string json = JsonConvert.SerializeObject(statcmd, Formatting.Indented);
            if (Communicate(ref json)) //Если сеанс связи с майнером не состоялся, то хешрейт не изменятся (останутся старые значения), установим только флаг наличия интернета, вдруг он сброшен
            {
                try { statres = JsonConvert.DeserializeObject<SatisticResult>(json); }
                catch { return res; }
                if (statres != null)
                {
                    if (statres.result.Count == 9)
                    {
                        report.AppendLine("Version: Claymore " + statres.result[0]);
                        report.AppendLine("Runing, min: " + statres.result[1]);
                        report.AppendLine("ETH (hr,sh,rej): " + statres.result[2]);
                        report.AppendLine("ETH hr GPUs: " + statres.result[3]);
                        report.AppendLine("DCR (hr,sh,rej): " + statres.result[4]);
                        report.AppendLine("DCR hr GPUs: " + statres.result[5]);
                        report.AppendLine("GPUs (T, fan %): " + statres.result[6]);
                        report.AppendLine(statres.result[7]);
                        if (String.IsNullOrEmpty(statres.result[7]))
                            fPools = false;
                        else
                            fPools = true;
                        report.AppendLine("ETH(inv,sw),DCR(inv,sw): " + statres.result[8]);
                        if (hr.Capacity == 0)
                            res = InitHr(statres.result[3]);
                        else
                            res = GettHr(statres.result[3]);
                        GetMinerStat(statres.result[2]);
                    }
                    else
                        report.AppendLine("This version Fermtools is not compatible with the version miner.");
                }
            }
            else
                fPools = true;
            return res;
        }
        private bool InitHr(string sres)
        {
            bool res = false;
            string[] shr = sres.Split(';');
            if (shr.Length > 0)
            {
                for (int i = 0; i < shr.Length; i++)
                {
                    int hash = 0;
                    if (int.TryParse(shr[i], out hash))
                        hr.Add(hash);
                    else
                        hr.Add(0);
                }
                res = true;
            }
            return res;
        }
        private bool GettHr(string sres)
        {
            bool res = false;
            string[] shr = sres.Split(';');
            if (shr.Length > 0)
            {
                if (hr.Count != shr.Length)
                    return res;
                for (int i = 0; i < shr.Length; i++)
                {
                    int hash = 0;
                    if (int.TryParse(shr[i], out hash))
                        hr[i] = hash;
                    else
                        hr[i] = 0;
                }
                res = true;
            }
            return res;
        }
        private bool GetMinerStat(string s_stat)
        {
            bool res = false;
            string[] shr = s_stat.Split(';');
            if (shr.Length == 3)
            {
                int hash = 0;
                if (int.TryParse(shr[0], out hash))
                    stat.Hashrate = hash;
                if (int.TryParse(shr[1], out hash))
                    stat.Shares = hash;
                if (int.TryParse(shr[2], out hash))
                    stat.SharesRej = hash;
                res = true;
            }
            return res;
        }
        public bool RestartMiner()
        {
            string json = JsonConvert.SerializeObject(restartcmd, Formatting.Indented);
            if (Communicate(ref json))
            {
                try { restartres = JsonConvert.DeserializeObject<RestartResult>(json); }
                catch { return false; }
                if (restartres != null)
                    return true;
            }
            return false;
        }
        private bool Communicate(ref string json)
        {
            bool bRes = false;
            if (string.IsNullOrEmpty(server))
                return bRes;
            if (port <= 0)
                return bRes;
            try
            {
                TcpClient client = new TcpClient(server, port);
                client.ReceiveTimeout = 2000;
                client.SendTimeout = 2000;
                NetworkStream stream = client.GetStream();

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
                stream.Write(data, 0, data.Length);

                data = new Byte[512];
                int bytes = stream.Read(data, 0, data.Length);
                stream.Close();
                client.Close();

                if (bytes > 0)
                {
                    json = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    bRes = true;
                }
            }
            catch { }
            return bRes;
        }
       
        private class CommandSet
        {
            public int id { get; set; }
            public string jsonrpc { get; set; }
            public string method { get; set; }
            public CommandSet(string cmd)
            {
                id = 0;
                jsonrpc = "2.0";
                method = cmd;
            }
        }
        private class SatisticResult
        {
            public SatisticResult()
            {
                result = new List<string>();
            }
            public int id { get; set; }
            public object error { get; set; }
            public List<string> result { get; set; }
        }
        private class RestartResult
        {
            public int id { get; set; }
            public object error { get; set; }
            public bool result { get; set; }
        }
        public class MinerStat
        {
            public int Hashrate;
            public int Shares;
            public int SharesOld;
            public int SharesRej;
            public int SharesRejOld;
        }
    }
}
