using Business.API;
using Contract.Core;
using Contract.OR;
using DataAccess;
using DataAccess.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Common;

namespace Business.ScheduleJobs
{
    public class UpdateORProcess
    {
        protected Entities DbContext = new Entities("ConnString.WebPortal");
    }

    public class UpdateORProcessJob : UpdateORProcess, IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            if (AppUtils.IsAutoUpdateStatusProcessing)
                return;
            AppUtils.IsAutoUpdateStatusProcessing = true;
            DateTime now = DateTime.Now;
            CustomLog.intervaljoblog.Info($"<Auto Update ORAnesthProgress> Start!");
            try
            {
                var firtdateOfMonth = DateTime.Now.ToFirstDayOfMonth();
                var enddateOfMonth = DateTime.Now.ToLastDayOfMonth();
                enddateOfMonth = enddateOfMonth.AddDays(1);
                var results = DbContext.ORAnesthProgresses.Where(x => x.dtOperation.Value>= firtdateOfMonth && x.dtOperation.Value< enddateOfMonth && !string.IsNullOrEmpty(x.ChargeDetailId) && x.IsDeleted!=true);
                CustomLog.intervaljoblog.Info(string.Format("<Auto Update ORAnesthProgress> Total item: {0}", results?.Count()));
                int countItemCancelCharge = 0;
                foreach (var item in results)
                {
                    var dataCharges = SyncGateWay.GetChargesV2(item.ChargeDetailId);
                    bool chargeStatus = true;
                    if (dataCharges?.Count > 0)
                    {
                        chargeStatus = dataCharges.Select(x => x.ChargeStatus).FirstOrDefault() == "1";
                    }
                    if (!chargeStatus)
                    {
                        //TH charge hủy. Hủy ca mổ
                        var entity = item.State = (int)OHPatientStateEnum.CancelCharge;
                        countItemCancelCharge++;
                        CustomLog.intervaljoblog.Info(string.Format("<Auto Update Cancel ORAnesthProgress> Info {0}: [PgId: {1}; PID: {2}; ChargeDetailId: {3}]", countItemCancelCharge, item.Id, item.PId,item.ChargeDetailId));
                    }
                }
                DbContext.SaveChanges();
                CustomLog.intervaljoblog.Info($"<Auto Update ORAnesthProgress> Success!");
            }
            catch (Exception ex)
            {
                CustomLog.errorlog.Info(string.Format("<Auto Update ORAnesthProgress> Error: {0}", ex));
            }
            AppUtils.IsAutoUpdateStatusProcessing = false;
        }
    }
}
