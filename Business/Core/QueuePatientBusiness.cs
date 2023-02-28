using Contract.OR;
using Contract.QueuePatient;
using Contract.Shared;
using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core
{
    public interface IQueuePatientBusiness
    {
        QueuePatientSearchResult<Patient> SearchPatients(QueuePatientSearchParam param);
        QueuePatientSearchResult<Patient> ViewPatientsPublic(QueuePatientSearchParam searchParam);
        IEnumerable<PatientState> GetStates();
        IEnumerable<RoomHospital> GetRooms();
        List<Contract.OR.ORRoom> GetORRooms();
        List<PlanORRoom> GetPlanORRooms(DateTime dtstart, DateTime stend);
        List<PlanORRoom> GetPlanORRoomsByID(DateTime dtstart, DateTime stend, int idresource);
        Patient GetPatientById(int patientId);
        Patient InsertUpdatePatient(Patient patient);
        CUDReturnMessage UpdateSorting(long Id, int value, int userId);
    }
    public class QueuePatientBusiness : IQueuePatientBusiness
    {
        private Lazy<IQueuePatientDataAccess> _lazyDataAccess;
        public QueuePatientBusiness(string appid, int uid)
        {
            _lazyDataAccess = new Lazy<IQueuePatientDataAccess>(() => new QueuePatientDataAccess(appid, uid));
        }

        public Patient GetPatientById(int patientId)
        {
            return _lazyDataAccess.Value.GetPatientById(patientId);
        }

        public IEnumerable<RoomHospital> GetRooms()
        {
            return _lazyDataAccess.Value.GetRooms();
        }
        public List<Contract.OR.ORRoom> GetORRooms()
        {
            return _lazyDataAccess.Value.GetORRooms();
        }
        public List<PlanORRoom> GetPlanORRooms(DateTime dtstart, DateTime stend)
        {
            return _lazyDataAccess.Value.GetPlanORRooms(dtstart, stend);
        }
        public List<PlanORRoom> GetPlanORRoomsByID(DateTime dtstart, DateTime stend, int idresource)
        {
            return _lazyDataAccess.Value.GetPlanORRoomsByID(dtstart, stend, idresource);
        }
        public IEnumerable<PatientState> GetStates()
        {
            return _lazyDataAccess.Value.GetStates();
        }

        public Patient InsertUpdatePatient(Patient patient)
        {
            return _lazyDataAccess.Value.InsertUpdatePatient(patient);
        }

        public QueuePatientSearchResult<Patient> SearchPatients(QueuePatientSearchParam param)
        {
            return _lazyDataAccess.Value.SearchPatients(param);
        }
        public QueuePatientSearchResult<Patient> ViewPatientsPublic(QueuePatientSearchParam searchParam)
        {
            return _lazyDataAccess.Value.ViewPatientsPublic(searchParam);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateSorting(long Id, int value, int userId)
        {
            var data = _lazyDataAccess.Value.UpdateSorting(Id, value, userId);

            return data;
        }
    }
}
