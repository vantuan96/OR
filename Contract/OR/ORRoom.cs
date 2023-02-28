using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.OR
{
    public class ORRoom
    {
        public string id { get; set; }
        public string title { get; set; }
    }
    public class PlanORRoom
    {
        public string id { get; set; }
        public string resourceId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
        public string infopatient { get; set; }
        public string color { get; set; }
    }
    public class Blocktime_view
    {
        public int id { get; set; }
        public string MaDV { get; set; }
        public string TenDv { get; set; }
        public int CleaningTime { get; set; }
        public int PreparationTime { get; set; }
        public int AnesthesiaTime { get; set; }
        public int OtherTime { get; set; }
        public int Ehos_Iddv { get; set; }
    }
}
