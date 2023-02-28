using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs
{
    [ProtoContract]
    public class BaseSearchContract<T>
    {
        public BaseSearchContract() { }
        public BaseSearchContract(int iPage, int iPageSize)
        {
            PageNumber = iPage;
            PageSize = iPageSize;                
        }

        [ProtoMember(1)]
        public int TotalRecords { get; set; }

        [ProtoMember(2)]
        public int PageNumber { get; private set; }

        [ProtoMember(3)]
        public int PageSize { get; private set; }

        [ProtoMember(4)]
        public int TotalPages 
        {
            get
            {
                if (TotalRecords <= 0 && PageSize <= 0) return 0;
                if (TotalRecords <= PageSize) return 1;
                return (TotalRecords / PageSize) + (TotalRecords % PageSize > 0 ? 1 : 0);
            }
        }

        /// <summary>
        /// Danh sách thông tin kết quả tìm kiếm
        /// </summary>
        [ProtoMember(5)]
        public List<T> Data { get; set; }        
    }
}
