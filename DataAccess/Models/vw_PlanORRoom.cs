//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_PlanORRoom
    {
        public int id { get; set; }
        public Nullable<int> Ehos_Iddv { get; set; }
        public string PId { get; set; }
        public string PatientName { get; set; }
        public string id_ehos_room { get; set; }
        public string ServiceName { get; set; }
        public Nullable<System.DateTime> ThoiGianHen_BatDau { get; set; }
        public Nullable<System.DateTime> ThoiGianHen_KetThuc { get; set; }
        public int Typel { get; set; }
    }
}
