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
    
    public partial class Basis
    {
        public int BasisId { get; set; }
        public string BasisCode { get; set; }
        public string BasisName { get; set; }
        public Nullable<int> BasisGroupId { get; set; }
        public Nullable<int> PnLListId { get; set; }
        public Nullable<int> PnLBUListId { get; set; }
        public string Description { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public Nullable<int> WardId { get; set; }
        public string RefCode { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string FullName { get; set; }
        public string StatusDescription { get; set; }
        public Nullable<System.DateTime> OpeningDate { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
        public string ManagerPhone { get; set; }
        public string SitePhone { get; set; }
        public string SiteEmail { get; set; }
        public string AreaManager { get; set; }
        public string AreaManagerPhone { get; set; }
        public string AreaManagerEmail { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<bool> Visible { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
