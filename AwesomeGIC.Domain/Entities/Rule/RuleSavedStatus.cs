namespace AwesomeGIC.Domain.Entities.Rule
{
    public class RuleSavedStatus
    {
        public bool IsSaved { get; set; }
        public Rule RuleInfo { get; set; }
        public string Error { get; set; }
    }
}
