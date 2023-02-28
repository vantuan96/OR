
using System.Collections.Generic;

namespace Admin.Models.Shared
{
    public class ValidationModel
    {
        public bool IsValidModel { get; set; }
        public Dictionary<string, string> InValidProperties { get; set; }

        public ValidationModel()
        {
            IsValidModel = true;
            InValidProperties = new Dictionary<string, string>();
        }
    }
}