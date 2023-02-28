using Contract.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models.Report
{
    public class DashboardModel
    {
        public List<CheckListDashboard> listCheckLists { get; set; }
        public List<SystemDashBoard> listSystems { get; set; }
        public List<CheckListInfoDashboard> listSummarys { get; set; }
    }
}
