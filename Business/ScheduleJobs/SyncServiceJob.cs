using Business.API;
using Contract.Core;
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
    public class SyncServiceJob
    {
        protected Entities DbContext = new Entities("ConnString.WebPortal");
        protected HpService GetOrCreateService(HISServiceModel item)
        {
            try
            {
                //Chỉ thêm các item là Service
                if (item.ServiceType == Constant.SERVICE_TYPE_SRV && item.ServiceGroupCode.ServiceIsSurgeryOrProcedure(item.ServiceGroupViName,item.ServiceGroupEnName))
                {
                    //var service = unitOfWork.ServiceRepository.FirstOrDefault(e => e.Code == item.ServiceCode);
                    var service = DbContext.HpServices.FirstOrDefault(e => e.Oh_Code == item.ServiceCode);
                    var group = GetOrCreateServiceGroup(item);
                    int serviceType = Constant.ListGroupCodeIsAnesth.Contains(item.ServiceGroupCode) ? 2 : 1;
                    if (service != null)
                    {
                        if (item.ServiceType == Constant.SERVICE_TYPE_PCK)
                        {
                            //Đánh dấu là gói nên cần xóa
                            service.IsDeleted = true;
                        }
                        service.ServiceGroupId = group.Id;
                        service.ServiceType = serviceType;
                        service.Name = item.ServiceViName;
                        return service;
                    }
                    int cleaningTime = 15;
                    int anesthTime = 15;
                    if (item.ServiceGroupViName.ServiceIsSurgery(item.ServiceGroupEnName))
                    {
                        cleaningTime = 30;
                        anesthTime = 30;
                    }
                    service = new HpService
                    {
                        Oh_Code = item.ServiceCode,
                        Name = item.ServiceViName,
                        Sort = 100,
                        Type = 2,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 0,
                        SourceClientId = 2,
                        OtherTime = 0,
                        CleaningTime = cleaningTime,
                        AnesthesiaTime = anesthTime,
                        PreparationTime = 0,
                        IdMapping = "0",
                        IsDeleted = !item.IsActive,
                        ServiceGroupId = group.Id,
                        ServiceType= serviceType
                    };
                    DbContext.HpServices.Add(service);
                    return service;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                CustomLog.intervaljoblog.Info(string.Format("<Sync OH Service> Error: {0}", ex));
                return null;
            }

        }
        protected void UpdateService(HISServiceModel item, HpService service)
        {
            //service.ViName = item.ServiceGroupViName;
            //service.EnName = item.ServiceGroupEnName;
            service.Oh_Code = item?.ServiceCode;
            service.Name = item.ServiceViName;
            service.IsDeleted = !item.IsActive;
            service.UpdatedDate = DateTime.Now;
            service.UpdatedBy = 0;
            #region Gán/bỏ cho các site
            var sites = DbContext.Locations.Where(x => x.LevelNo == 2 && !x.IsDeleted);
            if (sites.Any())
            {
                foreach(var sItem in sites)
                {
                    var entityMaps = DbContext.ORMappingServices.Where(x => x.HospitalCode == sItem.NameEN && x.ObjectId==service.Id);
                    if (entityMaps.Any())
                    {
                        foreach(var xItem in entityMaps)
                        {
                            xItem.IsDeleted = service.IsDeleted;
                        }
                    }
                    else
                    {
                        //Create new
                        var entityMap = new ORMappingService
                        {
                            HospitalCode = sItem.NameEN,
                            ObjectId = service.Id,
                            TypeMappingId = 1,
                            IsDeleted = service.IsDeleted,
                            CreatedBy = 0,
                            CreatedDate = DateTime.Now,
                            UpdateBy = 0,
                            UpdateDate = DateTime.Now,
                        };
                        DbContext.ORMappingServices.Add(entityMap);
                    }
                }
            }
            #endregion
        }
        protected ServiceGroup GetOrCreateServiceGroup(HISServiceModel item)
        {
            using (Entities DbContextLocal = new Entities("ConnString.WebPortal"))
            {
                var group = DbContextLocal.ServiceGroups.FirstOrDefault(e => e.Code == item.ServiceGroupCode);
                if (group != null)
                    return group;

                group = new ServiceGroup
                {
                    Code = item.ServiceGroupCode,
                    ViName = item.ServiceGroupViName,
                    EnName = item.ServiceGroupEnName,
                    CreatedAt = DateTime.Now,
                    CreatedBy = 0,
                    IsDeleted = false
                };
                DbContextLocal.ServiceGroups.Add(group);
                DbContextLocal.SaveChanges();
                return group;
            }
        }
    }

    public class SyncOHServiceJob : SyncServiceJob, IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            if (AppUtils.IsAutoUpdateStatusProcessing)
                return;
            AppUtils.IsAutoUpdateStatusProcessing = true;
            DateTime now = DateTime.Now;
            CustomLog.intervaljoblog.Info($"<Sync OH Service> Start!");
            try
            {
                //var results = OHConnectionAPI.GetService(last_updated, now);
                var results = SyncGateWay.GetService(string.Empty, string.Empty);
                CustomLog.intervaljoblog.Info(string.Format("<Sync OH Service> Total item: {0}", results?.Count()));
                int countItem = 0;
                foreach (HISServiceModel item in results)
                {
                    var service = GetOrCreateService(item);
                    if (service != null)
                    {
                        countItem++;
                        UpdateService(item, service);
                        CustomLog.intervaljoblog.Info(string.Format("<Sync OH Service> Info {0}: [Code: {1} - Name: {2}]", countItem, item.ServiceCode, item.ServiceEnName));
                        //break;
                    }
                }
                DbContext.SaveChanges();
                CustomLog.intervaljoblog.Info($"<Sync OH Service> Success!");
            }
           catch (Exception ex)
            {
                CustomLog.errorlog.Info(string.Format("<Sync OH Service> Error: {0}", ex));
            }
            AppUtils.IsAutoUpdateStatusProcessing = false;
        }
    }
}
