using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.DBContext;

namespace AwesomeGIC.Infrastructure.InterestRule.Commands
{
    public class ReplaceInterestRule : IReplaceInterestRule
    {
        public RuleSavedStatus Replace(Rule rule)
        {
            try
            {
                Rule ruleToReplace = Database.Rules.Where(r => r.Date.Equals(rule.Date)).First();
                ruleToReplace = rule;
                return new RuleSavedStatus()
                {
                    IsSaved = true,
                    RuleInfo = rule,
                    Error = ""
                };
            }
            catch (Exception ex)
            {
                return new RuleSavedStatus()
                {
                    IsSaved = false,
                    RuleInfo = rule,
                    Error = "Error: " + ex.ToString()
                };
            }
        }
    }
}
