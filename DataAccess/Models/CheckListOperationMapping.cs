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
    
    public partial class CheckListOperationMapping
    {
        public long Id { get; set; }
        public long InstanceId { get; set; }
        public int CheckListId { get; set; }
        public int CLItemId { get; set; }
        public int State { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
    
        public virtual CheckListItem CheckListItem { get; set; }
        public virtual CheckListOperation CheckListOperation { get; set; }
    }
}
