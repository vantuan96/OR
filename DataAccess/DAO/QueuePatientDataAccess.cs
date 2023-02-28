using Contract.OR;
using Contract.QueuePatient;
using Contract.Shared;
using DataAccess;
using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Common;
using ORRoom = Contract.OR.ORRoom;

namespace DataAccess.DAO
{
    public interface IQueuePatientDataAccess
    {
        QueuePatientSearchResult<Patient> SearchPatients(QueuePatientSearchParam searchParam);
        QueuePatientSearchResult<Patient> ViewPatientsPublic(QueuePatientSearchParam searchParam);
        IEnumerable<PatientState> GetStates();
        IEnumerable<RoomHospital> GetRooms();
        List<ORRoom> GetORRooms();
        List<PlanORRoom> GetPlanORRooms(DateTime dtstart, DateTime stend);
        List<PlanORRoom> GetPlanORRoomsByID(DateTime dtstart, DateTime stend, int idresource);
        Patient GetPatientById(int patientId);
        Patient InsertUpdatePatient(Patient patient);
        CUDReturnMessage UpdateSorting(long Id, int value, int userId);

    }
    public class QueuePatientDataAccess : BaseDataAccess, IQueuePatientDataAccess
    {
        public QueuePatientDataAccess(string appid, int uid) : base(appid, uid)
        {

        }
        public Patient GetPatientById(int patientId)
        {
            return DbContext.Patients.Find(patientId);
        }
        public IEnumerable<RoomHospital> GetRooms()
        {
            return DbContext.RoomHospitals.Where(h => !(bool)h.IsDeleted);
        }
        public List<ORRoom> GetORRooms()
        {
            var listORRooms = (from room in DbContext.RoomHospitals
                               where room.isdisplay == "1"
                               select new ORRoom
                               {
                                   id = room.id_ehos,
                                   title = room.name_ehos
                               });
            return listORRooms.OrderByDescending(n => n.id).ToList();
        }
        public List<PlanORRoom> GetPlanORRooms(DateTime dtstart, DateTime stend)
        {
            var listPlanORRooms = (from plan in DbContext.vw_PlanORRoom
            //var listPlanORRooms = (from plan in DbContext.tblLichHen_Temp
                                   where plan.ThoiGianHen_BatDau >= dtstart && plan.ThoiGianHen_KetThuc <= stend
                                   //&& new List<string>() { "21079922" }.Contains(plan.PId)
                                   select new
                                   {
                                       id = plan.id.ToString(),
                                       resourceId = plan.id_ehos_room.ToString(),
                                       start = plan.ThoiGianHen_BatDau.ToString(),
                                       end = plan.ThoiGianHen_KetThuc.ToString(),
                                       title = plan.ServiceName,
                                       eventColor = plan.Typel.ToString(),
                                       infopatient = plan.PId.ToString() + " " + plan.PatientName.ToString()
                                   }).ToList().Select(o => new PlanORRoom
                                   {
                                       //id = o.id,
                                       resourceId = o.resourceId,
                                       start = Convert.ToDateTime(o.start).ToString("o"),
                                       end = Convert.ToDateTime(o.end).ToString("o"),
                                       title = o.title,
                                       color = o.eventColor == "1" ? "green" : "red",
                                       infopatient = o.infopatient
                                    }).OrderByDescending(n => n.resourceId).ToList();
                                   //}).OrderByDescending(n => n.id).ToList();
            return listPlanORRooms;
        }
        public List<PlanORRoom> GetPlanORRoomsByID(DateTime dtstart, DateTime stend, int idresource)
        {
            var listPlanORRooms = (from plan in DbContext.vw_PlanORRoom
                                   where plan.id_ehos_room.Equals(idresource) && plan.ThoiGianHen_BatDau >= dtstart && plan.ThoiGianHen_KetThuc <= stend
                                   select new
                                   {
                                       id = plan.id.ToString(),
                                       resourceId = plan.id_ehos_room.ToString(),
                                       start = plan.ThoiGianHen_BatDau.ToString(),
                                       end = plan.ThoiGianHen_KetThuc.ToString(),
                                       title = plan.ServiceName,
                                       eventColor = plan.Typel.ToString(),
                                       infopatient = plan.PId.ToString() + " " + plan.PatientName.ToString()
                                   }).ToList().Select(o => new PlanORRoom
                                   {
                                       id = o.id,
                                       resourceId = o.resourceId,
                                       start = Convert.ToDateTime(o.start).ToString("o"),
                                       end = Convert.ToDateTime(o.end).ToString("o"),
                                       title = o.title,
                                       color = o.eventColor == "1" ? "green" : "red",
                                       infopatient = o.infopatient
                                   }).OrderByDescending(n => n.resourceId).ToList();
            return listPlanORRooms;
        }

        public IEnumerable<PatientState> GetStates()
        {
            return DbContext.PatientStates.Where(s => !s.IsDeleted);
        }
        public Patient InsertUpdatePatient(Patient patient)
        {
            if (patient.Id == 0)
            {
                DbContext.Patients.Add(patient);
                DbContext.SaveChanges();
                return patient;
            }

            var dbPatient = DbContext.Patients.Find(patient.Id);
            if (dbPatient != null)
            {
                dbPatient.StartDate = patient.StartDate;
                dbPatient.EndDate = patient.EndDate;
                dbPatient.PId = patient.PId;
                dbPatient.Age = patient.Age;
                dbPatient.AreaName = patient.AreaName;
                dbPatient.EkipName = patient.EkipName;
                dbPatient.TypeName = patient.TypeName;
                dbPatient.ServiceName = patient.ServiceName;
                dbPatient.State = patient.State;
                dbPatient.Description = patient.Description;

                dbPatient.UpdatedBy = patient.UpdatedBy;
                dbPatient.UpdatedDate = patient.UpdatedDate;
                dbPatient.Sorting = patient.Sorting;
                dbPatient.RoomId = patient.RoomId;
                dbPatient.PatientName = patient.PatientName;
                dbPatient.TypeKcbId = patient.TypeKcbId;
                dbPatient.Sex = patient.Sex;
                dbPatient.IntendTime = patient.IntendTime;
                dbPatient.EKipAnesth = patient.EKipAnesth;
                dbPatient.IsEmergence = patient.IsEmergence;
                DbContext.SaveChanges();

                return dbPatient;
            }

            return patient;
        }
        public QueuePatientSearchResult<Patient> SearchPatients(QueuePatientSearchParam searchParam)
        {
            if (searchParam.Keyword == null)
                searchParam.Keyword = string.Empty;
            searchParam.FromDate = searchParam.FromDate.AddTimeToTheStartOfDay();
            searchParam.ToDate = searchParam.ToDate.AddTimeToTheEndOfDay();

            int state = searchParam.StateId;
            var query = DbContext.Patients.Where(p => (searchParam.Keyword.Trim() == string.Empty
                                                || p.PatientName.Contains(searchParam.Keyword.Trim())
                                                || p.PId == searchParam.Keyword.Trim())
                                                && (state == 0 || p.State == state)
                                                 && (searchParam.RoomId == 0 || p.RoomId == searchParam.RoomId)
                                                && p.StartDate >= searchParam.FromDate
                                                && p.StartDate <= searchParam.ToDate
                                                && !p.IsDeleted);

            int totals = 0;
            var patients = new List<Patient>();
            if (query != null && query.Any())
            {
                var page = query.OrderBy(s => s.Sorting).ThenBy(p => p.State)
                            .Skip((searchParam.p - 1) * searchParam.PageSize).Take(searchParam.PageSize)
                            .GroupBy(p => new { Total = query.Count() })
                            .First();

                if (page != null && page.Any())
                {
                    totals = page.Key.Total;
                    patients = page.Select(p => p).ToList();
                }
            }

            var result = new QueuePatientSearchResult<Patient> { TotalRecords = totals, Data = patients };
            return result;
        }
        public QueuePatientSearchResult<Patient> ViewPatientsPublic(QueuePatientSearchParam searchParam)
        {
            if (searchParam.Keyword == null)
                searchParam.Keyword = string.Empty;
            searchParam.FromDate = searchParam.FromDate.AddTimeToTheStartOfDay();
            searchParam.ToDate = searchParam.ToDate.AddTimeToTheEndOfDay();

            int state = searchParam.StateId;
            var query = DbContext.Patients.Where(p => (searchParam.Keyword.Trim() == string.Empty
                                                || p.PatientName.Contains(searchParam.Keyword.Trim())
                                                || p.PId == searchParam.Keyword.Trim())
                                                && ((p.State > 0 && p.State != (int)PatientStateEnum.Moitao) && (state == 0 || p.State == state)) // ko lay benh nhan ở trạng thái mới tạo hoặc mặc định
                                                && p.StartDate >= searchParam.FromDate
                                                && p.StartDate <= searchParam.ToDate
                                                );

            int totals = 0;
            var patients = new List<Patient>();
            if (query != null && query.Any())
            {

                //var page = query.OrderBy(s => s.RoomId).ThenBy(p => p.Sorting).ThenBy(d => d.State)
                //            .Skip((searchParam.p - 1) * searchParam.PageSize).Take(searchParam.PageSize)
                //            .GroupBy(p => new { Total = query.Count() })
                //            .First();
                var page = query.OrderBy(s => s.RoomId).ThenBy(dt=>dt.EndDate).ThenBy(p => p.Sorting).ThenBy(d => d.State)
                            .Skip((searchParam.p - 1) * searchParam.PageSize).Take(searchParam.PageSize)
                            .GroupBy(p => new { Total = query.Count() })
                            .First();
                if (page != null && page.Any())
                {


                    totals = page.Key.Total;
                    patients = page.Select(p => p).ToList();

                    var queryRoom = DbContext.RoomHospitals.Where(r => r.IsDeleted == false);
                    if (queryRoom != null && queryRoom.Any())
                    {

                    }
                }
            }

            var result = new QueuePatientSearchResult<Patient> { TotalRecords = totals, Data = patients };
            return result;
        }


        /// <summary>
        /// Cập nhật cấu hình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateSorting(long Id, int value, int userId)
        {
            var setting = DbContext.Patients.SingleOrDefault(r => r.Id == Id && r.IsDeleted != true);

            if (setting != null)
            {
                setting.Sorting = value;
                setting.UpdatedBy = userId;
                setting.UpdatedDate = DateTime.Now;

                int affectedRow = DbContext.SaveChanges();
                if (affectedRow > 0)
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.Successed,
                        Message = ""
                    };
                }
                else
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.NoChanged,
                        Message = ""
                    };
                }
            }
            else
            {
                return new CUDReturnMessage()
                {
                    Id = (int)ResponseCode.KeyNotExist,
                    Message = ""
                };
            }
        }
    }


}
