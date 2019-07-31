using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscudoHotSpot
{
    class Hotspot
    {
        Process ps = new Process();
        private string message;
        int idEvent = 1;
        public Hotspot()
        {
            Init();
        }

        private void Init()
        {            
            ps.StartInfo.UseShellExecute = false;            
            ps.StartInfo.CreateNoWindow = true;
            ps.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        }

        public void Start()
        {
            ps.StartInfo.Arguments = "netsh wlan start hostednetwork";
            ps.StartInfo.FileName = "netsh";
            try
            {
                using (Process execute = Process.Start("netsh.exe", "netsh wlan start hostednetwork"))
                {
                    execute.WaitForExit();
                    message = execute.StandardOutput.ToString();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            WriteEventToLog("Start", "ok");
        }

        public void Create(string ssid, string password)
        {
            ps.StartInfo.FileName = "netsh";
            ps.StartInfo.Arguments = $"netsh wlan set hostednetwork mode=allow ssid={ssid} key={password}";
            try
            {
                using (Process execute = Process.Start("netsh.exe", $"netsh wlan set hostednetwork mode=allow ssid={ssid} key={password}"))
                {
                    execute.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            WriteEventToLog("Create", "ok");
        }

        public void Stop()
        {
            ps.StartInfo.Arguments = "netsh wlan stop hostednetwork";
            ps.StartInfo.FileName = "netsh";
            try
            {
                using (Process execute = Process.Start("netsh.exe", "netsh wlan stop hostednetwork"))
                {
                    execute.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }            
            WriteEventToLog("Stop", "ok");
        }

        public void Execute(ProcessStartInfo ps)
        {
            bool isExecuted;
            try
            {
                using (Process p = Process.Start(ps))
                {
                    message = p.StandardOutput.ReadToEnd() + "\n";
                    p.WaitForExit();
                    isExecuted = true;
                }
            }
            catch (Exception ex)
            {
                isExecuted = false;
                message = ex.Message;
                WriteEventToLog("Error", ex.Message);                
            }
        }

        public void WriteEventToLog(string myEvent, string myMessage)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                switch (myEvent)
                {
                    case "Create":
                        eventLog.WriteEntry("O serviço virtual foi criado com sucesso! ;)", EventLogEntryType.Information, idEvent, 1);
                        break;
                    case "Start":
                        eventLog.WriteEntry("O serviço virtual foi iniciado com sucesso! ;)", EventLogEntryType.Information, idEvent, 1);
                        break;
                    case "Stop":
                        eventLog.WriteEntry("O serviço virtual foi encerrado com sucesso! ;)", EventLogEntryType.Information, idEvent, 1);
                        break;
                    case "Error":
                        eventLog.WriteEntry(myMessage, EventLogEntryType.Error);
                        break;
                }
                idEvent ++;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }

    }
}
