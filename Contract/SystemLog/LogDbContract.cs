using ProtoBuf;
using System;

namespace Contract.SystemLog
{
    [ProtoContract]
    public class LogDbContract
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }

        [ProtoMember(2)]
        public string LogContent { get; set; }

        [ProtoMember(3)]
        public int UserId { get; set; }

        [ProtoMember(4)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(5)]
        public int LogTypeId { get; set; }

        [ProtoMember(6)]
        public int SourceId { get; set; }

        [ProtoMember(7)]
        public string Title { get; set; }

    }
}
