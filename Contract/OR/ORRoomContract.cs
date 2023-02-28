using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
  
     public class ORRoomContract
    {
        public ORRoomContract()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Sorting { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Id_Mapping { get; set; }
        public string Name_Mapping { get; set; }     
        public string IsDisplay { get; set; }
        public string HospitalCode { get; set; }
        public int SourceClientId { get; set; }
        public int TypeRoom { get; set; }
    }
}
