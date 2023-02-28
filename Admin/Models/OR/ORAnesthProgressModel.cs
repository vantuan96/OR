using Admin.Resource;
using Contract.OR;
using Contract.OR.SyncData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Models.OR
{
    public class ORAnesthProgressModel : ORAnesthProgressBase
    {
        public ORAnesthProgressModel()
        {
            listHpServices = new List<HpServiceContract>();
            listORRooms = new List<ORRoomContract>();
            listORDoctorManagements = new List<ORUserInfoContract>();
            listAnesthManagers = new List<ORUserInfoContract>();

        }
    
        public List<ORRoomContract> listORRooms { get; set; }
        public List<ORUserInfoContract> listORDoctorManagements { get; set; }
        public List<HpServiceContract> listHpServices { get; set; }
        public List<PatientService> Charges { get; set; }
        public string RegDescription { get; set; }
        public string Diagnosis { get; set; }
        public string AdmissionWard { get; set; }
        public string ProjectName { get; set; }
        public int TimeAnesth { get; set; }

        public int UIdSurgeryDoctorManager { get; set; }
        public string NameSurgeryDoctorManager { get; set; }
        public string EmailSurgeryDoctorManager { get; set; }
        public string PhoneSurgeryDoctorManager { get; set; }
        public string PositionSurgeryDoctorManager { get; set; }


        //dieu phoi gay me
        public List<ORUserInfoContract> listAnesthManagers { get; set; }
        public int UIdAnesthManager { get; set; }

        public string NameAnesthManager { get; set; }
        public string EmailAnesthManager { get; set; }
        public string PhoneAnesthManager { get; set; }
        public string PositionAnesthManager { get; set; }

        //ext
        public string ORUnitName { get; set; }
        public string ORUnitEmail { get; set; }

        public int CurrentUserId { get; set; }
        /// <summary>
        /// Bác sĩ phẫu thuật chính
        /// </summary>
        public int UIdPTVMain { get; set; }

    }
}
