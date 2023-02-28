using System;
using System.Collections.Generic;

namespace Contract.QueuePatient
{

    public class PatientContract
    {
        public PatientContract()
        {

        }
        public long Id { get; set; }
        public string  StartDate { get; set; }
        public string EndDate { get; set; }
        public string PId { get; set; }
        public int Age { get; set; }
        public string AreaName { get; set; }
        public string EkipName { get; set; }
        public string TypeName { get; set; }
        public string ServiceName { get; set; }
        public int State { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Sorting { get; set; }
        public int RoomId { get; set; }
        public string PatientName { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeKcbId { get; set; }
        public int Sex { get; set; }
        public DateTime IntendTime { get; set; }
        public int LichHen_Id { get; set; }
        public bool IsEmergence { get; set; }
        public string EKipAnesth { get; set; }
        public string RoomHospitalName { get; set; }
        public string Statename { get; set; }
        public string Color { get; set; }


      
    }
    public class PatientContractModel
    {
        public string kw { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<PatientContract> listData { get; set; }   
        public List<int> listRoom { get; set; }
        public int LastPage { get; set; }
        public string HtmlPaging { get; set; }


    }
}
