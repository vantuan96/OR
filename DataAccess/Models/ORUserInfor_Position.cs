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
    
    public partial class ORUserInfor_Position
    {
        public int UserInforId { get; set; }
        public int PositionId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
    
        public virtual ORPositionType ORPositionType { get; set; }
        public virtual ORUserInfo ORUserInfo { get; set; }
    }
}
