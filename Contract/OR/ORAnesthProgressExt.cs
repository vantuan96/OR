using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class ORAnesthProgressExtContract
    {
        public ORAnesthProgressExtContract()
        {
            listUsers = new List<ORMappingEkipContract>();
        }

        public int Id { get; set; }
        public string PId { get; set; }

        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int Sex { get; set; }
        public string National { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Ages { get; set; }
        public string PatientPhone { get; set; }


        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string HospitalPhone { get; set; }
        public int HpServiceId { get; set; }
        public int ORRoomId { get; set; }
        public string dtStart { get; set; }
        public string dtEnd { get; set; }
        public string dtOperation { get; set; }
        public Nullable<int> TimeAnesth { get; set; }
        public string RegDescription { get; set; }
        public int State { get; set; }


        public Nullable<int> UIdPTVMain { get; set; }
        public Nullable<int> UIdPTVSub1 { get; set; }
        public Nullable<int> UIdPTVSub2 { get; set; }

        public Nullable<int> UIdPTVSub3 { get; set; }
        public Nullable<int> UIdCECDoctor { get; set; }
        public Nullable<int> UIdNurseTool1 { get; set; }
        public Nullable<int> UIdNurseTool2 { get; set; }
        public Nullable<int> UIdNurseOutRun1 { get; set; }
        public Nullable<int> UIdNurseOutRun2 { get; set; }

        public string SurgeryDescription { get; set; }





        //PTV chính
        public string NamePTVMain { get; set; }
        public string EmailPTVMain { get; set; }
        public string PhonePTVMain { get; set; }
        public string PositionPTVMain { get; set; }
        //PTV phụ 1
        public string NamePTVSub1 { get; set; }
        public string EmailPTVSub1 { get; set; }
        public string PhonePTVSub1 { get; set; }
        public string PositionPTVSub1 { get; set; }

        //PTV phụ 2
        public string NamePTVSub2 { get; set; }
        public string EmailPTVSub2 { get; set; }
        public string PhonePTVSub2 { get; set; }
        public string PositionPTVSub2 { get; set; }

        //PTV phụ 3
        public string NamePTVSub3 { get; set; }
        public string EmailPTVSub3 { get; set; }
        public string PhonePTVSub3 { get; set; }
        public string PositionPTVSub3 { get; set; }

        //Bác sĩ CEC
        public string NameCECDoctor { get; set; }
        public string EmailCECDoctor { get; set; }
        public string PhoneCECDoctor { get; set; }
        public string PositionCECDoctor { get; set; }
        //Điều dưỡng dụng cụ 1
        public string NameNurseTool1 { get; set; }
        public string EmailNurseTool1 { get; set; }
        public string PhoneNurseTool1 { get; set; }
        public string PositionNurseTool1 { get; set; }

        //Điều dưỡng dụng cụ 2
        public string NameNurseTool2 { get; set; }
        public string EmailNurseTool2 { get; set; }
        public string PhoneNurseTool2 { get; set; }
        public string PositionNurseTool2 { get; set; }


        // Điều dưỡng chạy ngoài 1
        public string NameNurseOutRun1 { get; set; }
        public string EmailNurseOutRun1 { get; set; }
        public string PhoneNurseOutRun1 { get; set; }
        public string PositionNurseOutRun1 { get; set; }

        // Điều dưỡng chạy ngoài 2
        public string NameNurseOutRun2 { get; set; }
        public string EmailNurseOutRun2 { get; set; }
        public string PhoneNurseOutRun2 { get; set; }
        public string PositionNurseOutRun2 { get; set; }





        public string AnesthDescription { get; set; }
        public string NameProject { get; set; }
        public string VisitCode { get; set; }

        public string ProjectName { get; set; }

        public string HpServiceName { get; set; }
        public string ORRoomName { get; set; }




        public string NameCreatedBy { get; set; }
        public string EmailCreatedBy { get; set; }
        public string PhoneCreatedBy { get; set; }
        public string PositionCreatedBy { get; set; }
        public int CreatedBy { get; set; }
        public List<ORMappingEkipContract> listUsers { get; set; }


        //ext

        public Nullable<int> UIdMainAnesthDoctor { get; set; }
        public Nullable<int> UIdSubAnesthDoctor { get; set; }
        public Nullable<int> UIdAnesthNurse1 { get; set; }

        public Nullable<int> UIdAnesthNurse2 { get; set; }


        //Bác sĩ gây mê
        public string NameMainAnesthDoctor { get; set; }
        public string EmailMainAnesthDoctor { get; set; }
        public string PhoneMainAnesthDoctor { get; set; }
        public string PositionMainAnesthDoctor { get; set; }
        //Bác sĩ phụ mê
        public string NameSubAnesthDoctor { get; set; }
        public string EmailSubAnesthDoctor { get; set; }
        public string PhoneSubAnesthDoctor { get; set; }
        public string PositionSubAnesthDoctor { get; set; }

        //Điều dưỡng phụ mê 1
        public string NameAnesthNurse1 { get; set; }
        public string EmailAnesthNurse1 { get; set; }
        public string PhoneAnesthNurse1 { get; set; }
        public string PositionAnesthNurse1 { get; set; }

        //Điều dưỡng phụ mê 1
        public string NameAnesthNurse2 { get; set; }
        public string EmailAnesthNurse2 { get; set; }
        public string PhoneAnesthNurse2 { get; set; }
        public string PositionAnesthNurse2 { get; set; }
        //dashboard
        public string AnesthTitle { get; set; }
        public DateTime dtAnesthStart { get; set; }
        public string CleanTitle { get; set; }
        public DateTime dtCleanEnd { get; set; }

        public Boolean IsEmergence { get; set; }

        public string StateName { get; set; }
        public string Color { get; set; }





    }
}
