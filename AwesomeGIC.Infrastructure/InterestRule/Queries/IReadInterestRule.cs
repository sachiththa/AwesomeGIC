using AwesomeGIC.Domain.Entities.Rule;

namespace AwesomeGIC.Infrastructure.InterestRule.Queries
{
    public interface IReadInterestRule
    {
        List<Rule> GetAllInterestRules();
        bool RuleExistsForDate(string date);
        Rule? GetInterestRuleForMonth(int month);

        List<Rule> GetSortedInterestRulesByDate();

    }
}
