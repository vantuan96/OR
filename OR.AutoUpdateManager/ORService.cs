using Business.ScheduleJobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using VG.Common;

namespace OR.AutoUpdateManager
{
    public partial class ORService : ServiceBase
    {
        public ORService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            try
            {
                //Console.Title = "OR | Auto Update Data Manager";
                //Console.Title = AppUtils.AppName;
                //Using Quartz to create job
                JobScheduler.Start();
                CustomLog.intervaljoblog.Info("AutoUpdateManager was started");

            }
            catch (Exception ex)
            {
                CustomLog.errorlog.Info(string.Format("AutoUpdateManager start Error: {0}", ex));
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            try
            {
                CustomLog.intervaljoblog.Info("AutoUpdateManager was stoped");
            }
            catch (Exception ex)
            {
                CustomLog.errorlog.Info(string.Format("AutoUpdateManager stop Error: {0}", ex));
            }
        }
    }
}
