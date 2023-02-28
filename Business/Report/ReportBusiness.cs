using Contract.Enum;
using Contract.Report;
using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VG.Common;

namespace Business.Report
{
    public interface IReportBusiness
    {
        List<CheckListDashboard> GetDashboardInMonth();
        List<SystemDashBoard> GetDashboardBySystem();
        List<CheckListInfoDashboard> GetReportCheckListSummary();
    }

    public class ReportBusiness : BaseBusiness, IReportBusiness
    {
        private readonly Lazy<IReportDataAccess> lazyReport;
        private readonly Lazy<IOperationCheckListAccess> lazyOperCheckList;
        private readonly Lazy<IMasterDataAccess> lazyMasterAccess;

        public ReportBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyReport = new Lazy<IReportDataAccess>(() => new ReportDataAccess(appid, uid));
            lazyOperCheckList = new Lazy<IOperationCheckListAccess>(() => new OperationCheckListAccess(appid, uid));
            lazyMasterAccess = new Lazy<IMasterDataAccess>(() => new MasterDataAccess(appid, uid));

        }

        public List<CheckListDashboard> GetDashboardInMonth()
        {
            var result = new List<CheckListDashboard>();
            result =EnumExtension.ToListOfValueAndDesc<CheckListTypeEnum>().Select(r => new CheckListDashboard
                      {
                          CheckListType = r.Value,
                          CheckListTypeName = r.Description,
                          TotalCheckList=0,
                          TotalFinish=0,
                          TotalInprocess=0,
                          OverDeadline=0
                      }).ToList();
            var query=lazyOperCheckList.Value.ListOperationCheckList(0, 0, 0, 0, string.Empty, 0, false, true);
            if(query!=null)
            {
                var data = query.ToList();
                DateTime dt = DateTime.Now;
                DateTime firstDayOfMonth = Extension.AddTimeToTheStartOfDay(Extension.ToFirstDayOfMonth(dt));
                DateTime lastDayOfMonth = Extension.AddTimeToTheEndOfDay(Extension.ToLastDayOfMonth(dt));
                DateTime firstDayOfWeek = Extension.AddTimeToTheStartOfDay(Extension.FirstDayOfWeek(dt));
                DateTime lastDayOfWeek = Extension.AddTimeToTheEndOfDay(Extension.LastDayOfWeek(dt));

                foreach (var item in result)
                {
                    List<CheckListOperation> listCheckList = new List<CheckListOperation>();
                    switch (item.CheckListType)
                    {
                        case (int)CheckListTypeEnum.Monthly:
                            listCheckList = data.Where(c => c.CheckListTypeId == item.CheckListType && (firstDayOfMonth <= c.CreatedDate && c.CreatedDate <= lastDayOfMonth)).ToList();
                            break;
                        case (int)CheckListTypeEnum.Weekly:
                            listCheckList = data.Where(c => c.CheckListTypeId == item.CheckListType && (firstDayOfWeek <= c.CreatedDate && c.CreatedDate <= lastDayOfWeek)).ToList();
                            break;
                        case (int)CheckListTypeEnum.Daily:
                            listCheckList = data.Where(c => c.CheckListTypeId == item.CheckListType && Extension.EqualDate(c.CreatedDate??DateTime.Now)).ToList();
                            break;
                        case (int)CheckListTypeEnum.OnlyOne:
                            listCheckList = data.Where(c => c.CheckListTypeId == item.CheckListType && Extension.EqualDate(c.CreatedDate??DateTime.Now)).ToList();
                            break;                            
                    }
                    item.TotalCheckList = listCheckList.Count();
                    item.TotalFinish = listCheckList.Count(c => c.CheckListStatusId == (int)CheckListStateEnum.ApproveOk);
                    item.TotalInprocess = listCheckList.Count(c => c.CheckListStatusId != (int)CheckListStateEnum.ApproveOk);
                    item.OverDeadline = listCheckList.Count(c => c.CheckListStatusId != (int)CheckListStateEnum.ApproveOk && c.Deadline<dt);                    
                }
            }
            return result;            
        }

        public List<CheckListInfoDashboard> GetReportCheckListSummary()
        {
            var result = new List<CheckListInfoDashboard>();
            result = EnumExtension.ToListOfValueAndDesc<ReportCheckListTypeEnum>().Select(r => new CheckListInfoDashboard
            {
                ReportCheckListTypeId = r.Value,
                ReportCheckListTypeName = r.Description,
                TotalCheckList = 0,
                TotalFinishCheckList = 0,

            }).ToList();
            var query = lazyOperCheckList.Value.ListOperationCheckList(0, 0, 0, 0, string.Empty, 0, false, true);
            if (query != null)
            {
                var data = query.ToList();
                DateTime dt = DateTime.Now;
                DateTime firstDayOfMonth = Extension.AddTimeToTheStartOfDay(Extension.ToFirstDayOfMonth(dt));
                DateTime lastDayOfMonth = Extension.AddTimeToTheEndOfDay(Extension.ToLastDayOfMonth(dt));
                DateTime firstDayOfWeek = Extension.AddTimeToTheStartOfDay(Extension.FirstDayOfWeek(dt));
                DateTime lastDayOfWeek = Extension.AddTimeToTheEndOfDay(Extension.LastDayOfWeek(dt));

              
                foreach (var item in result)
                {
                    List<CheckListOperation> listCheckList = new List<CheckListOperation>();
                    switch (item.ReportCheckListTypeId)
                    {
                        case (int)ReportCheckListTypeEnum.Today:
                            listCheckList = data.Where(c => Extension.EqualDate(c.Deadline)).ToList();
                            break;
                        case (int)ReportCheckListTypeEnum.CurrentWeek:
                            listCheckList = data.Where(c => (firstDayOfWeek <= c.Deadline && c.Deadline <= lastDayOfWeek)).ToList();
                            break;
                        case (int) ReportCheckListTypeEnum.CurrentMonth:
                            listCheckList = data.Where(c => (firstDayOfMonth <= c.Deadline && c.Deadline <= lastDayOfMonth)).ToList();
                            break;                           
                       
                    }
                    item.TotalCheckList = listCheckList.Count();
                    item.TotalFinishCheckList = listCheckList.Count(c => c.CheckListStatusId == (int)CheckListStateEnum.ApproveOk);                
                }
            }
            return result;
        }

        public List<SystemDashBoard> GetDashboardBySystem()
        {
            var result = new List<SystemDashBoard>();
            var listSystem = lazyMasterAccess.Value.GetSystemCheckList(0, (int)SystemStatusEnum.Active, string.Empty, 0, false).ToList();
            if (listSystem != null)
            {
                var data = listSystem.Where(c => c.Visible == true).ToList();
                foreach(var sys in data)
                {
                    var system = new SystemDashBoard()
                    {
                        SystemId=sys.SystemId,
                        SystemName=sys.SystemName,
                        TotalCheckList=0,
                        TotalFinish=0,
                        TotalInprocess=0
                    };
                    result.Add(system);
                }                
            }

            var query = lazyOperCheckList.Value.ListOperationCheckList(0, 0, 0, 0, string.Empty, 0, false, true);
            if (query != null)
            {
                var data = query.ToList();
                DateTime dt = DateTime.Now;
                DateTime firstDayOfMonth = Extension.ToFirstDayOfMonth(dt);
                DateTime lastDayOfMonth = Extension.ToLastDayOfMonth(dt);
                DateTime firstDayOfWeek = Extension.FirstDayOfWeek(dt);
                DateTime lastDayOfWeek = Extension.LastDayOfWeek(dt);

                foreach (var item in result)
                {
                    List<CheckListOperation> listCheckList = new List<CheckListOperation>();
                    listCheckList = data.Where(c => c.SystemId == item.SystemId).ToList();
                    item.TotalCheckList = listCheckList.Count();
                    item.TotalFinish = listCheckList.Count(c => c.CheckListStatusId == (int)CheckListStateEnum.ApproveOk);
                    item.TotalInprocess = listCheckList.Count(c => c.CheckListStatusId != (int)CheckListStateEnum.ApproveOk);
                    item.OverDeadline = listCheckList.Count(c => c.CheckListStatusId != (int)CheckListStateEnum.ApproveOk && c.Deadline < dt);
                }
            }
            return result;
        }

     
    }
}