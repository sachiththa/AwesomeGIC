using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.InterestRule.Queries;
using AwesomeGIC.Infrastructure.Utils;

namespace AwesomeGIC.Application.Rules
{
    public class RuleHandler : IRuleHandler
    {
        private readonly IDateHandler _dateHandler;
        private readonly IReadInterestRule _readInterestRule;

        public RuleHandler(IDateHandler dateHandler, IReadInterestRule readInterestRule)
        {
            _dateHandler = dateHandler;
            _readInterestRule = readInterestRule;
        }

        /* Takes the input from the user and validates it
         * Validations:
         * 1. Date format : YYYYMMdd
         * 2. Interest rate should be greater than 0 and less than 100
        */
        public RuleValidityResponse IsValidRuleInput(string[] ruleDetails)
        {
            try
            {
                // Check whether the user has entered all the details of the transaction
                if (ruleDetails.Length == 3)
                {
                    // 1. Check for the date format
                    if (_dateHandler.isDateFormatValid(ruleDetails[0]))
                    {
                        // 2. Checking whether the interest rate is greater than 0 and less than 100
                        if (isValidInterestRate(ruleDetails[2]))
                        {
                            // Valid rule
                            return new RuleValidityResponse()
                            {
                                IsValid = true,
                                RuleInfo = new Rule
                                {
                                    Id = ruleDetails[1],
                                    Date = ruleDetails[0],
                                    InterestPercentage = Decimal.Parse(ruleDetails[2])
                                },
                                Error = ""
                            };
                        }
                        else
                        {
                            return new RuleValidityResponse()
                            {
                                IsValid = false,
                                RuleInfo = null,
                                Error = "Invalid interest rate. Please check and enter again."
                            };
                        }
                    }
                    else
                    {
                        return new RuleValidityResponse()
                        {
                            IsValid = false,
                            Error = "Invalid date format. Please check and enter again.",
                            RuleInfo = null
                        };
                    }
                }
                else
                {
                    return new RuleValidityResponse()
                    {
                        IsValid = false,
                        Error = "Invalid input format. Please check and enter again.",
                        RuleInfo = null
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new RuleValidityResponse()
                {
                    IsValid = false,
                    Error = "Error : " + ex.ToString(),
                    RuleInfo = null
                };
            }
        }

        public void PrintRules(string ruleId)
        {
            List<Rule> rulesList = _readInterestRule.GetAllInterestRules();
            if (rulesList.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Interest rules: ");
                Console.WriteLine($"{"Date",-10} {"| RuleID",-10} {"| Rate %",-8}");
                foreach (Rule rule in rulesList)
                {
                    Console.WriteLine($"{rule.Date,-10} {"| " + rule.Id,-10} {"| " + rule.InterestPercentage,-8}");
                }
            }
        }

        private bool isValidInterestRate(string interestRate)
        {
            if (Decimal.TryParse(interestRate, out Decimal rate))
            {
                if (rate > 0 && rate < 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
