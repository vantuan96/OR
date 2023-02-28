using Business.Core;
using Contract.OR;
using Contract.QueuePatient;
using Contract.Shared;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Core
{
    public interface IQueuePatientCaching
    {
        QueuePatientSearchResult<Patient> SearchPatients(QueuePatientSearchParam param);
        QueuePatientSearchResult<Patient> ViewPatientsPublic(QueuePatientSearchParam param);
        IEnumerable<PatientState> GetStates();
        IEnumerable<RoomHospital> GetRooms();
        List<Contract.OR.ORRoom> GetORRooms();
        List<PlanORRoom> GetPlanORRooms(DateTime dtstart, DateTime stend);
        List<PlanORRoom> GetPlanORRoomsByID(DateTime dtstart, DateTime stend, int idresource);
        Patient GetPatientById(int patientId);
        Patient InsertUpdatePatient(Patient patient);
        /// <summary>
        /// Cập nhật cấu hình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateSorting(long Id, int value, int userId);
    }
    public class QueuePatientCaching : BaseCaching, IQueuePatientCaching
    {
        private Lazy<IQueuePatientBusiness> _lazyQueuePatientBusiness;
        public QueuePatientCaching()
        {
            _lazyQueuePatientBusiness = new Lazy<IQueuePatientBusiness>(() => new QueuePatientBusiness(appid, uid));
        }

        public Patient GetPatientById(int patientId)
        {
            return _lazyQueuePatientBusiness.Value.GetPatientById(patientId);
        }

        public IEnumerable<RoomHospital> GetRooms()
        {
            return _lazyQueuePatientBusiness.Value.GetRooms();
        }
        public List<Contract.OR.ORRoom> GetORRooms()
        {
            return _lazyQueuePatientBusiness.Value.GetORRooms();
        }
        public List<PlanORRoom> GetPlanORRooms(DateTime dtstart, DateTime stend)
        {
            return _lazyQueuePatientBusiness.Value.GetPlanORRooms(dtstart, stend);
        }
        List<PlanORRoom> IQueuePatientCaching.GetPlanORRoomsByID(DateTime dtstart, DateTime stend, int idresource)
        {
            return _lazyQueuePatientBusiness.Value.GetPlanORRoomsByID(dtstart, stend, idresource);
        }
        public IEnumerable<PatientState> GetStates()
        {
            return _lazyQueuePatientBusiness.Value.GetStates();
        }

        public Patient InsertUpdatePatient(Patient patient)
        {
            return _lazyQueuePatientBusiness.Value.InsertUpdatePatient(patient);
        }

        public QueuePatientSearchResult<Patient> SearchPatients(QueuePatientSearchParam param)
        {
            return _lazyQueuePatientBusiness.Value.SearchPatients(param);
        }

        public CUDReturnMessage UpdateSorting(long Id, int value, int userId)
        {
            return _lazyQueuePatientBusiness.Value.UpdateSorting(Id, value, userId);
        }

        public QueuePatientSearchResult<Patient> ViewPatientsPublic(QueuePatientSearchParam param)
        {
            return _lazyQueuePatientBusiness.Value.ViewPatientsPublic(param);
        }
 
    }
}
