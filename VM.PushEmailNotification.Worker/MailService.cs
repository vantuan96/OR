using System;
using System.Configuration;
using System.ServiceProcess;
using OREmailNoti.WindowService.Shared;

namespace OREmailNoti.WindowService
{
    partial class MailService : ServiceBase
    {
        private System.Timers.Timer _ServicesTimer;
        public MailService()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }
        public void onDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            this._ServicesTimer = new System.Timers.Timer();
            this._ServicesTimer.Elapsed += new System.Timers.ElapsedEventHandler(_ServicesTimer_Elapsed);
            this._ServicesTimer.Interval = Int32.Parse(ConfigurationManager.AppSettings["TimeInterval"]);
            this._ServicesTimer.Enabled = true;
            this._ServicesTimer.Start();
        }
        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
        #region _ServicesTimer_Elapsed
        void _ServicesTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ORMailWorker _sysWorker = new ORMailWorker();
            _sysWorker.onComplected += MailService_onComplected;
            _sysWorker.Run();
            //new LogFile().Write("Timer ticked.", "_ServicesTimer_Elapsed");
        }
        void MailService_onComplected(object sender, EventHandleWebservice e)
        {
            this._ServicesTimer.Enabled = true;
            this._ServicesTimer.Start();
        }
        #endregion 
    }
}
