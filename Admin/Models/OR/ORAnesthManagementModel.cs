using Contract.OR;
using Contract.OR.SyncData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.OR
{
    public class ORAnesthManagementModel : ORAnesthProgressBase
    {
        public ORAnesthManagementModel()
        {
            listSurgeryDoctors = new List<ORUserInfoContract>();
            listToolTechnicals = new List<ORUserInfoContract>();
            listEkips = new List<ORUserInfoContract>();
            listUserEkips = new List<ORMappingEkipContract>();
            Charges = new List<PatientService>();
            ChargesReplace = new List<PatientService>();
        }
        public List<ORUserInfoContract> listEkips { get; set; }
        public List<ORUserInfoContract> listSurgeryDoctors { get; set; }
        public List<ORUserInfoContract> listToolTechnicals { get; set; }
        public List<ORMappingEkipContract> listUserEkips { get; set; }
        public List<PatientService> Charges { get; set; }
        //vutv7 
        public List<PatientService> ChargesReplace { get; set; }

        public int UIdPTVMain { get; set; }
        public int UIdPTVSub1 { get; set; }
        public int UIdPTVSub2 { get; set; }

        public int UIdPTVSub3 { get; set; }
        public int UIdPTVSub4 { get; set; }
        public int UIdPTVSub5 { get; set; }
        public int UIdPTVSub6 { get; set; }
        public int UIdPTVSub7 { get; set; }
        public int UIdPTVSub8 { get; set; }
        public int UIdCECDoctor { get; set; }
        public int UIdNurseTool1 { get; set; }
        public int UIdNurseTool2 { get; set; }
        public int UIdNurseOutRun1 { get; set; }
        public int UIdNurseOutRun2 { get; set; }
        public int UIdNurseOutRun3 { get; set; }
        public int UIdNurseOutRun4 { get; set; }
        public int UIdNurseOutRun5 { get; set; }
        public int UIdNurseOutRun6 { get; set; }
        public int UIdKTVSubSurgery { get; set; }
        public int UIdKTVDiagnose { get; set; }
        public int UIdKTVCEC { get; set; }
        public int UIdDoctorDiagnose { get; set; }
        public int UIdDoctorNewBorn { get; set; }
        public int UIdMidwives { get; set; }

        public string SurgeryDescription { get; set; }
        public string Diagnosis { get; set; }
        public string AdmissionWard { get; set; }


        public int TimeAnesth { get; set; }
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

        //test 
        public string UIdSurgeryDoctorManager { get; set; }

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
        public string NameKTVCEC{ get; set; }
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

    }
}
