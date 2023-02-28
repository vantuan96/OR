using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Device
{
    public class DeviceContract
    {
        public int DeviceId { get; set; }

        public string DeviceImei { get; set; }

        public string DeviceName { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public int LayoutTypeId { get; set; }

        public string LayoutTypeName { get; set; }

        public int QuestionGroupId { get; set; }

        public string QuestionGroupName { get; set; }

        public string Notes { get; set; }

        public byte Status { get; set; }

        public int CreatedBy { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public System.DateTime LastUpdatedDate { get; set; }

        public bool IsOnline{ get; set; }

        public System.DateTime LastHealthcheck { get; set; }

    }
}
