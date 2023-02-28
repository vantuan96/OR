using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Shared
{
    public static class AdminKeyDictionary
    {
        /// <summary>
        /// Danh sách tất cả key của gallery
        /// </summary>
        public static List<string> GalleryKey = new List<string>() {
            "Homepage_Gallery",
            "Homepage_VirtualTour",
            "Homepage_Entertainments",
            "Page_Entertainment_Gallery",
            "Page_FoodBeverage_Gallery",
            "Page_BusinessMeeting_Gallery",
            "Page_FoodBeverage_Gallery2",
            "Page_Wedding_Gallery1",
            "Page_SpaHealthCare_Center",
            "Page_BusinessMeeting_Gallery2",
            "Page_Wedding_Gallery2",
            "MicrositeGallery"
        };

        /// <summary>
        /// Danh sách tất cả key của bài viết
        /// </summary>
        public static List<string> ArticleKey = new List<string>()
        {
            "Page_BusinessMeeting",
            "Page_Entertainment",
            "Page_Loyalty",
            "Page_SpaHealthcare"            
        };
    }
}
