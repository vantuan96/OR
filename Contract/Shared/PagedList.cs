using ProtoBuf;
using System.Collections.Generic;

namespace Contract.Shared
{
    [ProtoContract]
    public class PagedList<T>
    {
        [ProtoMember(1)]
        public int Count { get; set; }

        [ProtoMember(2)]
        public List<T> List { get; set; }

        public PagedList()
        {

        }

        public PagedList(int count)
        {
            this.Count = count;
            this.List = new List<T>();
        }
    }
}
