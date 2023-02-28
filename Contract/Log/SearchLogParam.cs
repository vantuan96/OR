using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Log
{
    public class SearchLogParam
    {
        public long ObjectId { get; set; }
        public ObjectTypeEnum ObjectTypeId { get; set; }
    }
}
