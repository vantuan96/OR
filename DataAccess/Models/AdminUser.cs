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
    
    public partial class AdminUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdminUser()
        {
            this.AdminUser_Action_Deny = new HashSet<AdminUser_Action_Deny>();
            this.AdminUser_Location = new HashSet<AdminUser_Location>();
            this.AdminUser_LoginHistory = new HashSet<AdminUser_LoginHistory>();
            this.AdminUser_Microsite = new HashSet<AdminUser_Microsite>();
            this.AdminUser_PlusAction = new HashSet<AdminUser_PlusAction>();
            this.AdminUser_PnL = new HashSet<AdminUser_PnL>();
            this.AdminUser_PnL_Site = new HashSet<AdminUser_PnL_Site>();
            this.AdminUser_Role = new HashSet<AdminUser_Role>();
            this.AdminUserTrackings = new HashSet<AdminUserTracking>();
            this.AdminUser_System = new HashSet<AdminUser_System>();
            this.LogObjects = new HashSet<LogObject>();
            this.AdminUser_PnL_DepartmentSite = new HashSet<AdminUser_PnL_DepartmentSite>();
            this.ORTrackings = new HashSet<ORTracking>();
        }
    
        public int UId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PaswordSalt { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<int> Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequireChangePass { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<System.DateTime> LastActivityDate { get; set; }
        public string LastLoginIp { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }
        public bool IsADAccount { get; set; }
        public int LineManagerId { get; set; }
        public Nullable<int> FailedPasswordAttemptCount { get; set; }
        public string Token { get; set; }
        public Nullable<bool> IsLogon { get; set; }
    
        public virtual AdminUser_Action_Default AdminUser_Action_Default { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_Action_Deny> AdminUser_Action_Deny { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_Location> AdminUser_Location { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_LoginHistory> AdminUser_LoginHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_Microsite> AdminUser_Microsite { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_PlusAction> AdminUser_PlusAction { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_PnL> AdminUser_PnL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_PnL_Site> AdminUser_PnL_Site { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_Role> AdminUser_Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUserTracking> AdminUserTrackings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_System> AdminUser_System { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogObject> LogObjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser_PnL_DepartmentSite> AdminUser_PnL_DepartmentSite { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORTracking> ORTrackings { get; set; }
    }
}