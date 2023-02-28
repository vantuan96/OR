using ProtoBuf;
using System.Collections.Generic;

namespace Contract.SystemLog
{
    [ProtoContract]
    public class LogFileContract
    {
        [ProtoMember(1)]
        public int x_isset { get; set; }
        [ProtoMember(2)]
        public List<string> x_listlogfile { get; set; }
        [ProtoMember(3)]
        public string x_content { get; set; }
        [ProtoMember(4)]
        public string x_filename { get; set; }
        [ProtoMember(5)]
        public int x_id { get; set; }
        [ProtoMember(6)]
        public string x_message { get; set; }
        [ProtoMember(7)]
        public string x_foldername { get; set; }
    }
}
