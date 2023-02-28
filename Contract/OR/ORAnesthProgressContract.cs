using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class ORAnesthProgressContract
    {
        public ORAnesthProgressContract()
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
        public int SurgeryType { get; set; }
        public int TypeRoom { get; set; }
        public Nullable<System.DateTime> dtStart { get; set; }
        public Nullable<System.DateTime> dtEnd { get; set; }
        public Nullable<System.DateTime> dtOperation { get; set; }
        /// <summary>
        /// Thời gian nhập viện (dự kiến)
        /// </summary>
        public Nullable<System.DateTime> dtAdmission { get; set; }
        /// <summary>
        /// Địa điểm nhập viện
        /// </summary>
        public string AdmissionWard { get; set; }
        /// <summary>
        /// Ngày mở rộng/ gia hạn
        /// </summary>
        public Nullable<System.DateTime> dtExtend { get; set; }
        public Nullable<int> TimeAnesth { get; set; }
        public string RegDescription { get; set; }
        /// <summary>
        /// Chẩn đoán
        /// </summary>
        public string Diagnosis { get; set; }
        public int State { get; set; }


        public Nullable<int> UIdPTVMain { get; set; }
        public Nullable<int> UIdPTVSub1 { get; set; }
        public Nullable<int> UIdPTVSub2 { get; set; }

        public Nullable<int> UIdPTVSub3 { get; set; }
        public Nullable<int> UIdPTVSub4 { get; set; }
        public Nullable<int> UIdPTVSub5 { get; set; }
        public Nullable<int> UIdPTVSub6 { get; set; }
        public Nullable<int> UIdPTVSub7 { get; set; }
        public Nullable<int> UIdPTVSub8 { get; set; }
        public Nullable<int> UIdCECDoctor { get; set; }
        public Nullable<int> UIdNurseTool1 { get; set; }
        public Nullable<int> UIdNurseTool2 { get; set; }
        public Nullable<int> UIdNurseOutRun1 { get; set; }
        public Nullable<int> UIdNurseOutRun2 { get; set; }
        public Nullable<int> UIdNurseOutRun3 { get; set; }
        public Nullable<int> UIdNurseOutRun4 { get; set; }
        public Nullable<int> UIdNurseOutRun5 { get; set; }
        public Nullable<int> UIdNurseOutRun6 { get; set; }
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

        //PTV phụ 4
        public string NamePTVSub4 { get; set; }
        public string EmailPTVSub4 { get; set; }
        public string PhonePTVSub4 { get; set; }
        public string PositionPTVSub4 { get; set; }

        //PTV phụ 5
        public string NamePTVSub5 { get; set; }
        public string EmailPTVSub5 { get; set; }
        public string PhonePTVSub5 { get; set; }
        public string PositionPTVSub5 { get; set; }

        //PTV phụ 6
        public string NamePTVSub6 { get; set; }
        public string EmailPTVSub6 { get; set; }
        public string PhonePTVSub6 { get; set; }
        public string PositionPTVSub6 { get; set; }

        //PTV phụ 7
        public string NamePTVSub7 { get; set; }
        public string EmailPTVSub7 { get; set; }
        public string PhonePTVSub7 { get; set; }
        public string PositionPTVSub7 { get; set; }

        //PTV phụ 8
        public string NamePTVSub8 { get; set; }
        public string EmailPTVSub8 { get; set; }
        public string PhonePTVSub8 { get; set; }
        public string PositionPTVSub8 { get; set; }



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

        // Điều dưỡng chạy ngoài 3
        public string NameNurseOutRun3 { get; set; }
        public string EmailNurseOutRun3 { get; set; }
        public string PhoneNurseOutRun3 { get; set; }
        public string PositionNurseOutRun3 { get; set; }

        // Điều dưỡng chạy ngoài 4
        public string NameNurseOutRun4 { get; set; }
        public string EmailNurseOutRun4 { get; set; }
        public string PhoneNurseOutRun4 { get; set; }
        public string PositionNurseOutRun4 { get; set; }

        // Điều dưỡng chạy ngoài 5
        public string NameNurseOutRun5 { get; set; }
        public string EmailNurseOutRun5 { get; set; }
        public string PhoneNurseOutRun5 { get; set; }
        public string PositionNurseOutRun5 { get; set; }

        // Điều dưỡng chạy ngoài 6
        public string NameNurseOutRun6 { get; set; }
        public string EmailNurseOutRun6 { get; set; }
        public string PhoneNurseOutRun6 { get; set; }
        public string PositionNurseOutRun6 { get; set; }

        public string AnesthDescription { get; set; }
        public string NameProject { get; set; }
        public string VisitCode { get; set; }

        public string ProjectName { get; set; }

        public string HpServiceCode { get; set; }
        public string HpServiceName { get; set; }
        public string ORRoomName { get; set; }




        public string NameCreatedBy { get; set; }
        public string ADCreatedBy { get; set; }
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
        public Nullable<int> UIdAnesthNurseRecovery { get; set; }


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

        //Điều dưỡng phụ mê 2
        public string NameAnesthNurse2 { get; set; }
        public string EmailAnesthNurse2 { get; set; }
        public string PhoneAnesthNurse2 { get; set; }
        public string PositionAnesthNurse2 { get; set; }

        //Điều dưỡng phụ hồi tỉnh
        public string NameAnesthNurseRecovery { get; set; }
        public string EmailAnesthNurseRecovery { get; set; }
        public string PhoneAnesthNurseRecovery { get; set; }
        public string PositionAnesthNurseRecovery { get; set; }
        //dashboard
        public string AnesthTitle { get; set; }
        public DateTime dtAnesthStart { get; set; }
        public string CleanTitle { get; set; }
        public DateTime dtCleanEnd { get; set; }
        public Boolean IsEmergence { get; set; }
        //sorting
        public int Sorting { get; set; }

        public string OrderID { get; set; }
        public string ChargeDetailId { get; set; }
        public string DepartmentCode { get; set; }
        public int? ServiceType { get; set; }
        //vutv7
        public DateTime? ChargeDate { get; set; }
        public string ChargeDateStr{ get; set; }
        public string ChargeBy { get; set; }
        public Nullable<int> UIdKTVSubSurgery { get; set; }
        public Nullable<int> UIdKTVDiagnose { get; set; }
        public Nullable<int> UIdKTVCEC { get; set; }
        public Nullable<int> UIdDoctorDiagnose { get; set; }
        public Nullable<int> UIdDoctorNewBorn { get; set; }
        public Nullable<int> UIdMidwives { get; set; }
        public Nullable<int> UIdAnesthesiologist { get; set; }
        public Nullable<int> UIdSubAnesthDoctor2 { get; set; }
        // KTV phụ mổ
        public string NameKTVSubSurgery { get; set; }
        public string EmailKTVSubSurgery { get; set; }
        public string PhoneKTVSubSurgery { get; set; }
        public string PositionKTVSubSurgery { get; set; }
        // KTV CĐHA
        public string NameKTVDiagnose { get; set; }
        public string EmailKTVDiagnose { get; set; }
        public string PhoneKTVDiagnose { get; set; }
        public string PositionKTVDiagnose { get; set; }
        // KTV chạy máy CEC
        public string NameKTVCEC { get; set; }
        public string EmailKTVCEC { get; set; }
        public string PhoneKTVCEC { get; set; }
        public string PositionKTVCEC { get; set; }
        // BS CĐHA
        public string NameDoctorDiagnose { get; set; }
        public string EmailDoctorDiagnose { get; set; }
        public string PhoneDoctorDiagnose { get; set; }
        public string PositionDoctorDiagnose { get; set; }
        // BS sơ sinh
        public string NameDoctorNewBorn { get; set; }
        public string EmailDoctorNewBorn { get; set; }
        public string PhoneDoctorNewBorn { get; set; }
        public string PositionDoctorNewBorn { get; set; }
        // Nữ hộ sinh
        public string NameMidwives { get; set; }
        public string EmailMidwives { get; set; }
        public string PhoneMidwives { get; set; }
        public string PositionMidwives { get; set; }
        // BS khám gây mê
        public string NameAnesthesiologist { get; set; }
        public string EmailAnesthesiologist { get; set; }
        public string PhoneAnesthesiologist { get; set; }
        public string PositionAnesthesiologist { get; set; }
        // Bác sĩ phụ mê 2
        public string NameSubAnesthDoctor2 { get; set; }
        public string EmailSubAnesthDoctor2 { get; set; }
        public string PhoneSubAnesthDoctor2 { get; set; }
        public string PositionSubAnesthDoctor2 { get; set; }
    }
}
