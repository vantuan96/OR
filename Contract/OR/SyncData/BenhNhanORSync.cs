using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;

namespace Contract.OR.SyncData
{
    [ProtoContract]
    public class BenhNhanORSync
    {
        [ProtoMember(1)]
        [JsonProperty("DSBenhNhan")]
        public BenhNhanORs DSBenhNhan { get; set; }
    }

    [ProtoContract]
    public class BenhNhanORs
    {
        [ProtoMember(1)]
        [JsonProperty("BenhNhan")]
        public BenhNhanOR BenhNhan { get; set; }
    }

    [ProtoContract]
    public class BenhNhanOR
    {
        [ProtoMember(1)]
        [JsonProperty("MA_BN")]
        public string MA_BN { get; set; }
        [ProtoMember(2)]
        [JsonProperty("HO_TEN")]
        public string HO_TEN { get; set; }
        [ProtoMember(3)]
        [JsonProperty("NGAY_SINH")]
        public DateTime? NGAY_SINH { get; set; }
        [ProtoMember(4)]
        [JsonProperty("GIOI_TINH")]
        public int GIOI_TINH { get; set; }
        [ProtoMember(6)]
        [JsonProperty("QUOC_TICH")]
        public string QUOC_TICH { get; set; }
        [ProtoMember(7)]
        [JsonProperty("DIA_CHI")]
        public string DIA_CHI { get; set; }

        [ProtoMember(8)]
        [JsonProperty("VisitSyncs")]
        public PatientVisits VisitSyncs { get; set; }

        [ProtoMember(9)]
        [JsonProperty("PHONE")]
        public string PHONE { get; set; }
        [ProtoMember(10)]
        [JsonProperty("EMAIL")]
        public string EMAIL { get; set; }
        [ProtoMember(11)]
        [JsonProperty("TUOI")]
        public string TUOI { get; set; }
        [ProtoMember(12)]
        [JsonProperty("ListServices")]
        public PatientServices ListServices { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [ProtoContract]
    public class PatientServices
    {
        public List<PatientService> Services { get; set; }
        public PatientServices()
        {
            Services = new List<PatientService>();
        }


    }
    [ProtoContract]
    public class PatientService
    {
        [ProtoMember(1)]
        [JsonProperty("OrderID")]
        public string OrderID { get; set; }
        [ProtoMember(2)]
        [JsonProperty("ItemCode")]
        public string ItemCode { get; set; }
        [ProtoMember(3)]
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }
        [ProtoMember(4)]
        [JsonProperty("ChargeDetailId")]
        public string ChargeDetailId { get; set; }
        [ProtoMember(5)]
        [JsonProperty("LocationName")]
        public string LocationName { get; set; }
        [ProtoMember(5)]
        [JsonProperty("DepartmentCode")]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 1: PTTT; 2: Gây mê/Anesth
        /// </summary>
        public int ServiceType { get; set; }
        //vutv7
        [ProtoMember(5)]
        [JsonProperty("ChargeDate")]
        public DateTime ChargeDate { get; set; }
        //vutv7
        [ProtoMember(5)]
        [JsonProperty("ChargeBy")]
        public string ChargeBy { get; set; }
    }
}
