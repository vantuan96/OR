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
    
    public partial class PnLBuAttribute
    {
        public int PnLBuAttributeId { get; set; }
        public string PnLBuAttributeCode { get; set; }
        public string PnLBuAttributeName { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Visible { get; set; }
        public Nullable<int> PnLAttributeGroupId { get; set; }
        public Nullable<int> PnLListId { get; set; }
        public Nullable<int> PnLBUListId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
