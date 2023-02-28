using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.MasterData
{
    public partial class SubCateSystemContract
    {
        public int SubCateId { get; set; }
        public string SubCateName { get; set; }
        public Boolean Visible { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StateName { get; set; }


    }
}
