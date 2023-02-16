using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace example
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private void writeEvent(string message)
        {
            string name = "SimpleService";
            string logDestination = "Application";

            if (!EventLog.SourceExists(name))
            {
                EventLog.CreateEventSource(name, logDestination);
            }
            EventLog.WriteEntry(name, message);
        }

        protected override void OnStart(string[] args)
        {
            writeEvent("Running OnStart");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        private int t = 0;
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            writeEvent(string.Format($"Simple running about {t} seconds"));
            t += 10;
        }

        protected override void OnPause()
        {
            writeEvent("Service on Pause");
        }

        protected override void OnContinue()
        {
            writeEvent("Service on Continue");
        }

        protected override void OnStop()
        {
            writeEvent("Service on Stop");
            t = 0;
        }
    }
}
