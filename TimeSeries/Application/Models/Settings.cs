namespace TimeSeriesRoot.Application.Models
{
    public class Settings
    {
        public BusinessRulesSettings BusinessRules { get; set; }
        public string CollectionPath { get; set; }
    }

    public class BusinessRulesSettings
    {
        public List<string> AllowedMbaValues { get; set; }
    }
}
