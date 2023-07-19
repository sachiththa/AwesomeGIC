using AwesomeGIC.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Infrastructure.InterestRule.Commands
{
    public interface ISaveInterestRule
    {
        public RuleSavedStatus Save(Rule rule);
    }
}
