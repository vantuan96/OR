using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Models.OR
{
    public class ORAnesthCUDProgressModel : ORAnesthProgressBase
    {
        public ORAnesthCUDProgressModel()
        {
            listStates = new List<SelectListItem>();
        }
        public int UIdMainAnesthDoctor { get; set; }
        public int UIdSubAnesthDoctor { get; set; }
        public int UIdAnesthNurse1 { get; set; }

        public int UIdAnesthNurse2 { get; set; }

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

        public string AnesthDescription { get; set; }


        public int TimeAnesth { get; set; }
        //anesth doctor
        public string PhoneCreatedBy { get; set; }
        public string PositionCreatedBy { get; set; }

        public List<SelectListItem> listStates { get; set; }
        


    }

}
