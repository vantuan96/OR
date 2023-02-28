using System.Collections.Generic;

namespace Admin.Models.Shared
{
    public class SearchObjectModel<TObject>
    {
        public int CurrentPage { get; set; }
        public int CountShowPage { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<string> Params { get; set; }
        
        public TObject DispalyObject { get; set; }
    }
}