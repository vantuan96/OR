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
    
    public partial class Device
    {
        public int DeviceId { get; set; }
        public string DeviceImei { get; set; }
        public string DeviceName { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> LayoutTypeId { get; set; }
        public Nullable<int> QuestionGroupId { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<byte> Status { get; set; }
        public System.DateTime LastHealthcheck { get; set; }
    
        public virtual Location Location { get; set; }
    }
}
