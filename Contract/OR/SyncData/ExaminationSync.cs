using Newtonsoft.Json;
using ProtoBuf;
using System;

namespace Contract.OR.SyncData
{
    [ProtoContract]
    public class ExaminationSync
    {
        public Examination ThongTin { get; set; }
    }
    [ProtoContract]
    public class Examination
    {
        [ProtoMember(1)]
        [JsonProperty("CHAN_DOAN")]
        public string CHAN_DOAN { get; set; }
        [ProtoMember(2)]
        [JsonProperty("NGAY_VAO")]
        public DateTime NGAY_VAO { get; set; }
        [ProtoMember(3)]
        [JsonProperty("NGAY_RA")]
        public DateTime NGAY_RA { get; set; }
        [ProtoMember(4)]
        [JsonProperty("NGAY_TTOAN")]
        public DateTime NGAY_TTOAN { get; set; }
        [ProtoMember(6)]
        [JsonProperty("MAICD")]
        public string MAICD { get; set; }
        [ProtoMember(7)]
        [JsonProperty("LYDOVV")]
        public string LYDOVV { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
