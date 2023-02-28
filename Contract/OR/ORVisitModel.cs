using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.ComponentModel;

namespace Contract.OR
{

    [ProtoContract]
    public class ORVisitModel
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
        [JsonProperty("CHANDOAN")]
        public string CHANDOAN { get; set; }
        [ProtoMember(11)]
        [JsonProperty("MAICD")]
        public string MAICD { get; set; }
        [ProtoMember(12)]
        [JsonProperty("LYDOVV")]
        public string LYDOVV { get; set; }
        [ProtoMember(13)]
        [JsonProperty("HO_TEN")]
        public string HO_TEN { get; set; }
        [ProtoMember(14)]
        [JsonProperty("NGAY_SINH")]
        public DateTime NGAY_SINH { get; set; }
        [ProtoMember(15)]
        [JsonProperty("GIOI_TINH")]
        public int GIOI_TINH { get; set; }
        [ProtoMember(16)]
        [JsonProperty("QUOC_TICH")]
        public string QUOC_TICH { get; set; }
        [ProtoMember(17)]
        [JsonProperty("DIA_CHI")]
        public string DIA_CHI { get; set; }

        [ProtoMember(16)]
        [JsonProperty("HospitalCode")]
        public string HospitalCode { get; set; }
        [ProtoMember(17)]
        [JsonProperty("HospitalName")]
        public string HospitalName { get; set; }
        [ProtoMember(18)]
        [DefaultValue("2")]
        [JsonProperty("SourceClientId")]
        public int SourceClientId { get; set; }
        [ProtoMember(19)]
        [JsonProperty("HospitalPhone")]
        public string HospitalPhone { get; set; }
        [ProtoMember(20)]
        [JsonProperty("PatientPhone")]
        public string PatientPhone { get; set; }
        [ProtoMember(21)]
        [JsonProperty("Email")]
        public string Email { get; set; }
        [ProtoMember(22)]
        [JsonProperty("Age")]
        public string Age { get; set; }
        //ext
        [ProtoMember(23)]
        [JsonProperty("Weight")]
        public string Weight { get; set; }
        [ProtoMember(24)]
        [JsonProperty("Height")]
        public string Height { get; set; }
        [ProtoMember(25)]
        [JsonProperty("BMI")]
        public string BMI { get; set; }
        [ProtoMember(26)]
        [JsonProperty("Pulse")]
        public string Pulse { get; set; }
        [ProtoMember(27)]
        [JsonProperty("SatO2")]
        public string SatO2 { get; set; }
        [ProtoMember(28)]
        [JsonProperty("PatientService")]
        public string PatientService { get; set; }
        [ProtoMember(28)]
        [JsonProperty("SurgeryType")]
        public int SurgeryType { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
