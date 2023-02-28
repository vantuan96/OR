using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.MasterData
{
    public class CateSystemContract
    {
      
        public int CateId { get; set; }
        public string CateName { get; set; }
        public bool Visible { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string StateName { get; set; }
        
    }
}
