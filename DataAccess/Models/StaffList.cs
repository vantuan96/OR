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
    
    public partial class StaffList
    {
        public int StaffListId { get; set; }
        public string StaffListCode { get; set; }
        public string FullName { get; set; }
        public Nullable<int> General { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public Nullable<int> UnitCodeId { get; set; }
        public Nullable<int> CentreCodeId { get; set; }
        public Nullable<int> DepartmentCodeId { get; set; }
        public Nullable<int> GroupCodeId { get; set; }
        public string OfficeLocation { get; set; }
        public string CityCode { get; set; }
        public Nullable<int> TitleCodeId { get; set; }
        public string LevelCode { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string ManagerCode { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<System.DateTime> JoinCompanyDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<bool> Visible { get; set; }
    }
}
