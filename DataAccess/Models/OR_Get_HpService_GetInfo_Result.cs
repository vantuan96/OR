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
    
    public partial class OR_Get_HpService_GetInfo_Result
    {
        public int Id { get; set; }
        public string Oh_Code { get; set; }
        public string Name { get; set; }
        public int SourceClientId { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> CleaningTime { get; set; }
        public Nullable<int> PreparationTime { get; set; }
        public Nullable<int> AnesthesiaTime { get; set; }
        public Nullable<int> OtherTime { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public string Description { get; set; }
        public Nullable<int> Sort { get; set; }
        public string IdMapping { get; set; }
        public string listSites { get; set; }
        public Nullable<int> TotalRecords { get; set; }
    }
}
