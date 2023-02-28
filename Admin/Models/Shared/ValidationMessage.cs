
namespace Admin.Models.Shared
{
    public class ValidationMessage
    {
        public bool IsValid { get; set; }
        public string UserMessage { get; set; }
        public string TechnicalMessage { get; set; }
    }
}