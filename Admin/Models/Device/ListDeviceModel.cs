using Contract.Device;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Admin.Models.Device
{
    public class ListDeviceModel
    {
        public List<DeviceContract> Devices { get; set; }

        public SelectList Locations { get; set; }

        public SelectList Status { get; set; }

        public SelectList Onlines { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }        

        public string SearchText { get; set; }
    }
}