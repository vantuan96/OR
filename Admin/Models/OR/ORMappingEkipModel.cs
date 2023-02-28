using Contract.OR;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Admin.Models.OR
{
    public class ORMappingEkipModel
    {
        public ORMappingEkipModel()
        {
            listEkips = new List<ORUserInfoContract>();
            listPositions = new List<SelectListItem>();
        }
        public long Id { get; set; }
        public int ORAnesthProgessId { get; set; }
        public int TypePageId { get; set; }

        public int PositionId { get; set; }
        public string HospitalCode { get; set; }

        public List<ORUserInfoContract> listEkips { get; set; }
        public List<SelectListItem> listPositions { get; set; }

        public int UIdEkip { get; set; }
        public string NameEkip { get; set; }
        public string EmailEkip { get; set; }
        public string PhoneEkip { get; set; }
        public string PositionEkip { get; set; }


    }

}
