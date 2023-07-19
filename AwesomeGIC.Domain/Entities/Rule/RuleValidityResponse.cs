namespace AwesomeGIC.Domain.Entities.Rule
{
    public class RuleValidityResponse
    {
        public bool IsValid { get; set; }
        public string? Error { get; set; }
        public Rule? RuleInfo { get; set; }
    }
}
