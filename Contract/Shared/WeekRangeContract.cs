using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Contract.Shared
{
    [ProtoContract]
    public class WeekRangeContract
    {
        [ProtoMember(1)]
        public string Range { get; set; }

        [ProtoMember(2)]
        public DateTime StartDate { get; set; }

        [ProtoMember(3)]
        public DateTime EndDate { get; set; }

        [ProtoMember(4)]
        public int Week { get; set; }

        [ProtoMember(5)]
        public int Month { get; set; }

        [ProtoMember(6)]
        public int Year { get; set; }

    }
}
