using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Contract.OR.SyncData
{
    public class ListPatientVist
    {
        public PatientVisits VisitSyncs { get; set; }
    }

    [ProtoContract]
    public class PatientVisits
    {
        public List<PatientVisit> VisitSync { get; set; }
        public PatientVisits()
        {
            VisitSync = new List<PatientVisit>();
        }

        
    }
    [ProtoContract]
    public class PatientVisit
    {
        [ProtoMember(1)]
        [JsonProperty("MA_BN")]
        public string MA_BN { get; set; }
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
        [JsonProperty("MA_LOAI_KCB")]
        public string MA_LOAI_KCB { get; set; }
        [ProtoMember(7)]
        [JsonProperty("MA_KHOA")]
        public string MA_KHOA { get; set; }
        [ProtoMember(8)]
        [JsonProperty("TEN_KHOA")]
        public string TEN_KHOA { get; set; }

        [ProtoMember(9)]
        [JsonProperty("VISIT_CODE")]
        public string VISIT_CODE { get; set; }

        [ProtoMember(10)]
        [JsonProperty("MA_BENH_VIEN")]
        public string MA_BENH_VIEN { get; set; }
        //ext
        [DefaultValue("Null")]
        [ProtoMember(12)]
        [JsonProperty("Weight")]
        public string Weight { get; set; }
        [DefaultValue("Null")]
        [ProtoMember(13)]
        [JsonProperty("Height")]
        public string Height { get; set; }
        [DefaultValue("Null")]
        [ProtoMember(14)]
        [JsonProperty("BMI")]
        public string BMI { get; set; }
        [DefaultValue("Null")]
        [ProtoMember(15)]
        [JsonProperty("Pulse")]
        public string Pulse { get; set; }
        [DefaultValue("Null")]
        [ProtoMember(16)]
        [JsonProperty("SatO2")]
        public string SatO2 { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
