using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.DBContext;
using AwesomeGIC.Infrastructure.Utils;

namespace AwesomeGIC.Infrastructure.InterestRule.Queries
{
    public class ReadInterestRule : IReadInterestRule
    {
        private readonly IDateHandler _dateHandler;

        public ReadInterestRule(IDateHandler dateHandler)
        {
            _dateHandler = dateHandler;
        }

        public bool RuleExistsForDate(string date)
        {
            if (Database.Rules.Any(r => r.Date.Equals(date)))
            {
                return true;
            }

            return false;
        }

        public List<Rule> GetAllInterestRules()
        {
            return Database.Rules.ToList();
        }

        public Rule? GetInterestRuleForMonth(int month)
        {
            if (Database.Rules.Any(r => _dateHandler.ConvertDateTime(r.Date, "yyyyMMdd").Month == month))
            {
                return Database.Rules.FirstOrDefault(r => _dateHandler.ConvertDateTime(r.Date, "yyyyMMdd").Month == month);
            }

            return null;
        }

        public List<Rule> GetSortedInterestRulesByDate()
        {
            return Database.Rules.OrderBy(r => _dateHandler.ConvertDateTime(r.Date, "yyyyMMdd")).ToList();
        }
    }
}
