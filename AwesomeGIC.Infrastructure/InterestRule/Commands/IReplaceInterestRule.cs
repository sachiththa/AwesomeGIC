using AwesomeGIC.Domain.Entities.Rule;

namespace AwesomeGIC.Infrastructure.InterestRule.Commands
{
    public interface IReplaceInterestRule
    {
        public RuleSavedStatus Replace(Rule rule);
    }
}
