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
    
    public partial class SystemSetting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsAdminConfig { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
