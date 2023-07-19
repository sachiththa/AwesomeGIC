using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Infrastructure.InterestRule.Commands
{
    public class SaveInterestRule : ISaveInterestRule
    {
        public RuleSavedStatus Save(Rule rule)
        {
            try
            {
                // Save to rules array
                Database.Rules.Add(rule);
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
