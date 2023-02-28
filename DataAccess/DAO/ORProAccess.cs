using Contract.OR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Contract.Enum;
using Contract.Shared;
using VG.Common;
using DataAccess.Shared;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DataAccess.DAO
{
    public interface IORProAccess
    {
        //linhht them username
        List<ORAnesthProgressContract> FindAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, int IsDashboard, string username = "");
        List<ORAnesthProgressContract> FindAnesthPublicInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId);
        IEnumerable<HpServiceSite> SearchHpServiceInfo(int State, string kw, int p, int ps, int HpServiceId, string siteId, int sourceClientId = -1);


    }

    public class ORProAccess : BaseExec, IORProAccess
    {
        public ORProAccess(string appid, int uid) : base()
        {
        }
        //linhht
        public List<ORAnesthProgressContract> FindAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, int IsDashboard, string username = "")
        {
            try
            {
                var parameters = new object[]
                {
                        FromDate,
                        ToDate,
                        State,
                        kw,
                        ORRoomId,
                        HpServiceId,
                        siteId,
                        sourceClientId,
                        CurrentUserId,
                        IsDashboard,
                        username
                };
                IRowMapper<ORAnesthProgressContract> rowMapper = MapBuilder<ORAnesthProgressContract>.MapNoProperties()
                #region data
                    .MapByName(x => x.Id)
                .MapByName(x => x.PId)
                .MapByName(x => x.HoTen)
                .MapByName(x => x.NgaySinh)

                .MapByName(x => x.Sex)
                .MapByName(x => x.National)
                .MapByName(x => x.Email)
                .MapByName(x => x.Address)
                 .MapByName(x => x.Ages)
                .MapByName(x => x.PatientPhone)

                .MapByName(x => x.HospitalCode)
                .MapByName(x => x.HospitalName)
                 .MapByName(x => x.HospitalPhone)
                .MapByName(x => x.HpServiceId)
                .MapByName(x => x.ORRoomId)
                .MapByName(x=>x.SurgeryType)
                .MapByName(x => x.dtStart)
                 .MapByName(x => x.dtEnd)
                .MapByName(x => x.dtOperation)
                .MapByName(x => x.TimeAnesth)
                .MapByName(x => x.RegDescription)
                 .MapByName(x => x.State)

                .MapByName(x => x.UIdPTVMain)
                .MapByName(x => x.UIdPTVSub1)
                .MapByName(x => x.UIdPTVSub2)
                .MapByName(x => x.UIdPTVSub3)
                .MapByName(x => x.UIdCECDoctor)
                .MapByName(x => x.UIdNurseTool1)
                .MapByName(x => x.UIdNurseTool2)
                .MapByName(x => x.UIdNurseOutRun1)
                .MapByName(x => x.UIdNurseOutRun2)

                .MapByName(x => x.UIdMainAnesthDoctor)
                .MapByName(x => x.UIdSubAnesthDoctor)
                .MapByName(x => x.UIdAnesthNurse1)
                .MapByName(x => x.UIdAnesthNurse2)




                .MapByName(x => x.NamePTVMain)
                .MapByName(x => x.PhonePTVMain)
                .MapByName(x => x.PositionPTVMain)
                .MapByName(x => x.EmailPTVMain)

                .MapByName(x => x.NamePTVSub1)
                .MapByName(x => x.EmailPTVSub1)
                .MapByName(x => x.PhonePTVSub1)
                .MapByName(x => x.PositionPTVSub1)

                .MapByName(x => x.NamePTVSub2)
                .MapByName(x => x.EmailPTVSub2)
                .MapByName(x => x.PhonePTVSub2)
                .MapByName(x => x.PositionPTVSub2)

                .MapByName(x => x.NamePTVSub3)
                .MapByName(x => x.EmailPTVSub3)
                .MapByName(x => x.PhonePTVSub3)
                .MapByName(x => x.PositionPTVSub3)

                .MapByName(x => x.NameCECDoctor)
                .MapByName(x => x.EmailCECDoctor)
                .MapByName(x => x.PhoneCECDoctor)
               .MapByName(x => x.PositionCECDoctor)


                .MapByName(x => x.NameNurseTool1)
                .MapByName(x => x.EmailNurseTool1)
                .MapByName(x => x.PhoneNurseTool1)
                .MapByName(x => x.PositionNurseTool1)


                .MapByName(x => x.NameNurseTool2)
                .MapByName(x => x.EmailNurseTool2)
                .MapByName(x => x.PhoneNurseTool2)
                .MapByName(x => x.PositionNurseTool2)

                .MapByName(x => x.NameNurseOutRun1)
                .MapByName(x => x.EmailNurseOutRun1)
                .MapByName(x => x.PhoneNurseOutRun1)
                .MapByName(x => x.PositionNurseOutRun1)




                .MapByName(x => x.NameNurseOutRun2)
                .MapByName(x => x.EmailNurseOutRun2)
                .MapByName(x => x.PhoneNurseOutRun2)
                .MapByName(x => x.PositionNurseOutRun2)


                .MapByName(x => x.NameMainAnesthDoctor)
                .MapByName(x => x.EmailMainAnesthDoctor)
                .MapByName(x => x.PhoneMainAnesthDoctor)
                .MapByName(x => x.PositionMainAnesthDoctor)

                .MapByName(x => x.NameSubAnesthDoctor)
                .MapByName(x => x.EmailSubAnesthDoctor)
                .MapByName(x => x.PhoneSubAnesthDoctor)
                .MapByName(x => x.PositionSubAnesthDoctor)

                .MapByName(x => x.NameAnesthNurse1)
                .MapByName(x => x.EmailAnesthNurse1)
                .MapByName(x => x.PhoneAnesthNurse1)
                .MapByName(x => x.PositionAnesthNurse1)

                .MapByName(x => x.NameAnesthNurse2)
                .MapByName(x => x.EmailAnesthNurse2)
                .MapByName(x => x.PhoneAnesthNurse2)
                .MapByName(x => x.PositionAnesthNurse2)



                .MapByName(x => x.CreatedBy)
                .MapByName(x=> x.ADCreatedBy)
                .MapByName(x => x.NameCreatedBy)
                .MapByName(x => x.EmailCreatedBy)
                .MapByName(x => x.PhoneCreatedBy)
                .MapByName(x => x.PositionCreatedBy)

                .MapByName(x => x.AnesthDescription)
                .MapByName(x => x.VisitCode)
                .MapByName(x => x.NameProject)

                .MapByName(x => x.HpServiceName)
                .MapByName(x => x.HpServiceCode)
                .MapByName(x => x.ORRoomName)

                .MapByName(x => x.SurgeryDescription)
                //dashboard
                .MapByName(x => x.AnesthTitle)
                .MapByName(x => x.dtAnesthStart)
                .MapByName(x => x.CleanTitle)
                .MapByName(x => x.dtCleanEnd)
                .MapByName(x => x.OrderID)
                .MapByName(x => x.ChargeDetailId)
                .MapByName(x => x.dtExtend)
                .MapByName(x=>x.ServiceType)
                //vutv7
                .MapByName(x => x.ChargeDate)
                .MapByName(x=>x.ChargeBy)

                #endregion
                    .Build();
                var result = ExecStoredProc<ORAnesthProgressContract>("OR_Get_AnesthProgress", parameters, rowMapper);
                if (result == null || !result.Any())
                    return new List<ORAnesthProgressContract>();
                return result.ToList();
            }
            catch (Exception ex)
            {
                return new List<ORAnesthProgressContract>();
            }
        }

        public List<ORAnesthProgressContract> FindAnesthPublicInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId)
        {
            try
            {

                FromDate = FromDate.AddTimeToTheStartOfDay();
                ToDate = ToDate.AddTimeToTheEndOfDay();

                var parameters = new object[]
                {
                        FromDate,
                        ToDate,
                        State,
                        kw,
                        ORRoomId,
                        HpServiceId,
                        siteId,
                        sourceClientId,
                        CurrentUserId
                };
                IRowMapper<ORAnesthProgressContract> rowMapper = MapBuilder<ORAnesthProgressContract>.MapNoProperties()
                #region data
                    .MapByName(x => x.Id)
                .MapByName(x => x.PId)
                .MapByName(x => x.HoTen)
                .MapByName(x => x.NgaySinh)

                .MapByName(x => x.Sex)
                .MapByName(x => x.National)
                .MapByName(x => x.Email)
                .MapByName(x => x.Address)
                 .MapByName(x => x.Ages)
                .MapByName(x => x.PatientPhone)

                .MapByName(x => x.HospitalCode)
                .MapByName(x => x.HospitalName)
                 .MapByName(x => x.HospitalPhone)
                .MapByName(x => x.HpServiceId)
                .MapByName(x => x.ORRoomId)
                .MapByName(x => x.TypeRoom)
                .MapByName(x => x.dtStart)
                 .MapByName(x => x.dtEnd)
                .MapByName(x => x.dtOperation)
                .MapByName(x => x.TimeAnesth)
                .MapByName(x => x.RegDescription)
                 .MapByName(x => x.State)

                .MapByName(x => x.UIdPTVMain)
                .MapByName(x => x.UIdPTVSub1)
                .MapByName(x => x.UIdPTVSub2)
                .MapByName(x => x.UIdPTVSub3)
                .MapByName(x => x.UIdCECDoctor)
                .MapByName(x => x.UIdNurseTool1)
                .MapByName(x => x.UIdNurseTool2)
                .MapByName(x => x.UIdNurseOutRun1)
                .MapByName(x => x.UIdNurseOutRun2)

                .MapByName(x => x.UIdMainAnesthDoctor)
                .MapByName(x => x.UIdSubAnesthDoctor)
                .MapByName(x => x.UIdAnesthNurse1)
                .MapByName(x => x.UIdAnesthNurse2)




                .MapByName(x => x.NamePTVMain)
                .MapByName(x => x.PhonePTVMain)
                .MapByName(x => x.PositionPTVMain)
                .MapByName(x => x.EmailPTVMain)

                .MapByName(x => x.NamePTVSub1)
                .MapByName(x => x.EmailPTVSub1)
                .MapByName(x => x.PhonePTVSub1)
                .MapByName(x => x.PositionPTVSub1)

                .MapByName(x => x.NamePTVSub2)
                .MapByName(x => x.EmailPTVSub2)
                .MapByName(x => x.PhonePTVSub2)
                .MapByName(x => x.PositionPTVSub2)

                .MapByName(x => x.NamePTVSub3)
                .MapByName(x => x.EmailPTVSub3)
                .MapByName(x => x.PhonePTVSub3)
                .MapByName(x => x.PositionPTVSub3)

                .MapByName(x => x.NameCECDoctor)
                .MapByName(x => x.EmailCECDoctor)
                .MapByName(x => x.PhoneCECDoctor)
               .MapByName(x => x.PositionCECDoctor)


                .MapByName(x => x.NameNurseTool1)
                .MapByName(x => x.EmailNurseTool1)
                .MapByName(x => x.PhoneNurseTool1)
                .MapByName(x => x.PositionNurseTool1)


                .MapByName(x => x.NameNurseTool2)
                .MapByName(x => x.EmailNurseTool2)
                .MapByName(x => x.PhoneNurseTool2)
                .MapByName(x => x.PositionNurseTool2)

                .MapByName(x => x.NameNurseOutRun1)
                .MapByName(x => x.EmailNurseOutRun1)
                .MapByName(x => x.PhoneNurseOutRun1)
                .MapByName(x => x.PositionNurseOutRun1)




                .MapByName(x => x.NameNurseOutRun2)
                .MapByName(x => x.EmailNurseOutRun2)
                .MapByName(x => x.PhoneNurseOutRun2)
                .MapByName(x => x.PositionNurseOutRun2)


                .MapByName(x => x.NameMainAnesthDoctor)
                .MapByName(x => x.EmailMainAnesthDoctor)
                .MapByName(x => x.PhoneMainAnesthDoctor)
                .MapByName(x => x.PositionMainAnesthDoctor)

                .MapByName(x => x.NameSubAnesthDoctor)
                .MapByName(x => x.EmailSubAnesthDoctor)
                .MapByName(x => x.PhoneSubAnesthDoctor)
                .MapByName(x => x.PositionSubAnesthDoctor)

                .MapByName(x => x.NameAnesthNurse1)
                .MapByName(x => x.EmailAnesthNurse1)
                .MapByName(x => x.PhoneAnesthNurse1)
                .MapByName(x => x.PositionAnesthNurse1)

                .MapByName(x => x.NameAnesthNurse2)
                .MapByName(x => x.EmailAnesthNurse2)
                .MapByName(x => x.PhoneAnesthNurse2)
                .MapByName(x => x.PositionAnesthNurse2)

                .MapByName(x => x.CreatedBy)
                .MapByName(x=>x.ADCreatedBy)
                .MapByName(x => x.NameCreatedBy)
                .MapByName(x => x.EmailCreatedBy)
                .MapByName(x => x.PhoneCreatedBy)
                .MapByName(x => x.PositionCreatedBy)

                .MapByName(x => x.AnesthDescription)
                .MapByName(x => x.VisitCode)
                .MapByName(x => x.NameProject)

                .MapByName(x => x.HpServiceName)
                .MapByName(x => x.ORRoomName)

                .MapByName(x => x.SurgeryDescription)
                //dashboard
                .MapByName(x => x.AnesthTitle)
                .MapByName(x => x.dtAnesthStart)
                .MapByName(x => x.CleanTitle)
                .MapByName(x => x.dtCleanEnd)
                .MapByName(x => x.IsEmergence)
                .MapByName(x => x.Sorting)
                .MapByName(x => x.dtExtend)
                #endregion
                    .Build();
                var result = ExecStoredProc<ORAnesthProgressContract>("OR_Get_AnesthProgress_PublicInfo", parameters, rowMapper);
                if (result == null || !result.Any())
                    return new List<ORAnesthProgressContract>();
                return result.ToList();
            }
            catch (Exception ex)
            {
                return new List<ORAnesthProgressContract>();
            }
        }

        public IEnumerable<HpServiceSite> SearchHpServiceInfo(int State, string kw, int p, int ps, int HpServiceId, string siteId, int sourceClientId = -1)
        {
            try
            {
                var parameters = new object[]
                {
                        State,
                        kw,
                        HpServiceId,
                        siteId,
                        sourceClientId,
                        p,
                        ps
                };
                IRowMapper<HpServiceSite> rowMapper = MapBuilder<HpServiceSite>.MapNoProperties()
                .MapByName(x => x.Id)
                .MapByName(x => x.Oh_Code)
                .MapByName(x => x.Name)
                .MapByName(x => x.SourceClientId)
                .MapByName(x => x.CleaningTime)
                .MapByName(x => x.PreparationTime)
                .MapByName(x => x.AnesthesiaTime)
                .MapByName(x => x.OtherTime)
                .MapByName(x => x.Description)
                .MapByName(x => x.Sort)
                .MapByName(x => x.IdMapping)
                .MapByName(x => x.TotalRecords)
                .Map(x => x.listSites).WithFunc(dr => XmlToObject<List<SiteShortContract>>(dr, "listSites"))
                .Build();
                var result = ExecStoredProc<HpServiceSite>("OR_Get_HpService_GetInfo", parameters, rowMapper);
                if (result == null || !result.Any())
                    return new List<HpServiceSite>();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}



