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
    
    public partial class SystemDataChangeLog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemDataChangeLog()
        {
            this.SystemDataChangeLogDetails = new HashSet<SystemDataChangeLogDetail>();
        }
    
        public int LdcId { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public Nullable<int> ChangedObjectId { get; set; }
        public string Description { get; set; }
        public Nullable<int> ChangedBy { get; set; }
        public string DB_User { get; set; }
        public string ComputerName { get; set; }
        public System.DateTime ChangedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemDataChangeLogDetail> SystemDataChangeLogDetails { get; set; }
    }
}