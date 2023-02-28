using Contract.OR;
using Contract.OR.SyncData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.OR
{
    public class ORAnesthesiaModel : ORAnesthProgressBase
    {
        public ORAnesthesiaModel()
        {
            listAnesthDoctors = new List<ORUserInfoContract>();
            listAnesthNurses = new List<ORUserInfoContract>();
            listUserEkips = new List<ORMappingEkipContract>();
            listEkips = new List<ORUserInfoContract>();
            Charges = new List<PatientService>();
            ChargesReplace = new List<PatientService>();
        }
        public List<ORUserInfoContract> listAnesthDoctors { get; set; }
        public List<ORUserInfoContract> listAnesthNurses { get; set; }
        public List<ORUserInfoContract> listEkips { get; set; }
        public List<PatientService> Charges { get; set; }
        public string AdmissionWard { get; set; }

        public List<ORMappingEkipContract> listUserEkips { get; set; }
        //vutv7 
        public List<PatientService> ChargesReplace { get; set; }
        public int UIdMainAnesthDoctor { get; set; }
        public int UIdSubAnesthDoctor { get; set; }
        public int UIdAnesthNurse1 { get; set; }
        public int UIdAnesthNurse2 { get; set; }
        public int UIdAnesthNurseRecovery { get; set; }
        public int UIdSubAnesthDoctor2 { get; set; }
        public int UIdAnesthesiologist { get; set; }

        //Bác sĩ gây mê
        public string NameMainAnesthDoctor { get; set; }
        public string EmailMainAnesthDoctor { get; set; }
        public string PhoneMainAnesthDoctor { get; set; }
        public string PositionMainAnesthDoctor { get; set; }
        //Bác sĩ phụ mê 1
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

        //Điều dưỡng hồi tỉnh
        public string NameAnesthNurseRecovery { get; set; }
        public string EmailAnesthNurseRecovery { get; set; }
        public string PhoneAnesthNurseRecovery { get; set; }
        public string PositionAnesthNurseRecovery { get; set; }

        public string AnesthDescription { get; set; }

    
        public int TimeAnesth { get; set; }
        //anesth doctor

      
        public string PhoneCreatedBy { get; set; }
        public string PositionCreatedBy { get; set; }

        //Bác sĩ phụ mê 2
        public string NameSubAnesthDoctor2 { get; set; }
        public string EmailSubAnesthDoctor2 { get; set; }
        public string PhoneSubAnesthDoctor2 { get; set; }
        public string PositionSubAnesthDoctor2 { get; set; }

        //BS khám gây mê
        public string NameAnesthesiologist { get; set; }
        public string EmailAnesthesiologist { get; set; }
        public string PhoneAnesthesiologist { get; set; }
        public string PositionAnesthesiologist { get; set; }
    }
}
