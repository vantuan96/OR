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
    
    public partial class BlockTime
    {
        public int id { get; set; }
        public string MaDV { get; set; }
        public string TenDv { get; set; }
        public Nullable<int> CleaningTime { get; set; }
        public Nullable<int> PreparationTime { get; set; }
        public Nullable<int> AnesthesiaTime { get; set; }
        public Nullable<int> OtherTime { get; set; }
        public Nullable<int> Ehos_Iddv { get; set; }
    }
}