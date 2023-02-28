using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.MasterData
{
    public class ItemContract
    {
        public int CLItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Boolean Visible { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int Sort { get; set; }
        public string StateName { get; set; }
    }
    public class SearchItem
    {
        [ProtoMember(1)]
        public List<ItemContract> Data { get; set; }
        [ProtoMember(21)]
        public int TotalRows { get; set; }
    }

}
