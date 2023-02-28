using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.Qcs.EvaluationCategoryGroups;
using BMS.Contract.Qcs.SiteZones;

namespace BMS.Contract.Qcs.EvaluationSiteZones
{
    [ProtoContract]
    public class EvaluationGroupContract
    {
        [ProtoMember(1)]
        public EvaluationCateGroupItemContract CateGroup { get; set; }

        [ProtoMember(2)]
        public List<EvaluationZoneGroupContract> ListSiteZone { get; set; }

        public int EvaluatedCount
        {
            get
            {
                if (ListSiteZone == null || ListSiteZone.Count == 0) return 0;

                return ListSiteZone.Sum(r => r.EvaluatedCount);
            }
        }
        public int NeedToEvaluate
        {
            get
            {
                if (ListSiteZone == null || ListSiteZone.Count == 0) return 0;

                return ListSiteZone.Sum(r => r.NeedToEvaluate);
            }
        }

        public EvaluationGroupContract()
        {
        }

        public EvaluationGroupContract(EvaluationCateGroupItemContract cateGroup)
        {
            this.CateGroup = cateGroup;
            this.ListSiteZone = new List<EvaluationZoneGroupContract>();
        }
    }

    [ProtoContract]
    public class EvaluationZoneGroupContract
    {
        [ProtoMember(1)]
        public ZoneItemContract SiteZone { get; set; }

        [ProtoMember(2)]
        public int EvaluatedCount { get; set; }

        [ProtoMember(3)]
        public int NeedToEvaluate { get; set; }
    }
}
