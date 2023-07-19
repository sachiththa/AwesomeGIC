using AwesomeGIC.Domain.Entities.Rule;
namespace AwesomeGIC.Application.Rules
{
    public interface IRuleHandler
    {
        RuleValidityResponse IsValidRuleInput(string[] ruleDetails);
        void PrintRules(string ruleId);
    }
}
