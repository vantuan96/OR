using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
   
    public enum ORAnesthManagerEnum
    {
        [Description("Ekip do Quản lý mổ điều phối")]
        SurgeryDoctorManager = 1,
        [Description("Ekip do Quản lý gây mê điều phối")]
        AnesthDoctorManager = 2,      
    }
}
