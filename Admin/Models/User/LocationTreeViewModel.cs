using Contract.User;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Models.User
{
    public class LocationTreeViewModel
    {
        public string key { get; set; }

        public string title { get; set; }

        public string layout_type { get; set; }

        public string question_group { get; set; }

        public int question_group_id { get; set; }

        public bool expanded { get; set; }

        public bool folder { get; set; }

        public bool selected { get; set; }
        public string code { get; set; }
        public string parentcode { get; set; }
        public string type { get; set; }

        public List<LocationTreeViewModel> children { get; set; }

        public LocationTreeViewModel()
        {

        }

        public string title_en { get; set; }

        public string answer_request { get; set; }

        public string answer_request_en { get; set; }
        

        public int level { get; set; }

        public string icon { get; set; }

        public string extraClasses { get; set; }

        public int id { get; set; }
    }

    public class DvhcTreeViewModel
    {
        public string key { get; set; }
        public string title { get; set; }
        public bool expanded { get; set; }
        public bool folder { get; set; }
        public bool selected { get; set; }
        public string parentcode { get; set; }
        public string prefix { get; set; }
        public List<DvhcTreeViewModel> children { get; set; }
        public DvhcTreeViewModel()
        {

        }
    }
}
