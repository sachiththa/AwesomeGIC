using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Domain.Entities.Statement;
using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Infrastructure.Account.Queries;
using AwesomeGIC.Infrastructure.InterestRule.Queries;
using AwesomeGIC.Infrastructure.Transaction.Queries;
using AwesomeGIC.Infrastructure.Utils;
using System.Globalization;

namespace AwesomeGIC.Application.Statement
{
    public class StatementHandler : IStatementHandler
    {
        private readonly IReadAccount _readAccount;
        private readonly IReadTransaction _readTransaction;
        private readonly IReadInterestRule _readInterestRule;
        private readonly IDateHandler _dateHandler;

        public StatementHandler(IReadAccount readAccount, IReadTransaction readTransaction, IReadInterestRule readInterestRule, IDateHandler dateHandler)
        {
            _readAccount = readAccount;
            _readTransaction = readTransaction;
            _readInterestRule = readInterestRule;
            _dateHandler = dateHandler;
        }

        public StatementInputValidity IsValidPrintStatementInput(string[] input)
        {
            try
            {
                // Check whether the user has entered all the details needed to print the statement
                if (input.Length == 2)
                {
                    // 1. Check whether the month input is correct
                    if (int.TryParse(input[1], out int monthInput))
                    {
                        if (monthInput > 0 && monthInput < 12)
                        {
                            // Check if account exists
                            var userInputAccount = _readAccount.GetAccountByID(input[0]);
                            if (userInputAccount != null)
                            {
                                return new StatementInputValidity
                                {
                                    IsValid = true,
                                    AccountId = userInputAccount.Id,
                                    Month = monthInput,
                                    Error = ""
                                };
                            }
                            else
                            {
                                return new StatementInputValidity
                                {
                                    IsValid = false,
                                    AccountId = "",
                                    Month = monthInput,
                                    Error = $"Account {input[0]} does not exist. Please check and enter again."
                                };
                            }
                        }
                        else
                        {
                            return new StatementInputValidity
                            {
                                IsValid = false,
                                AccountId = "",
                                Month = 0,
                                Error = "Month input is incorrect. Please check and enter again."
                            };
                        }
                    }
                    else
                    {
                        return new StatementInputValidity
                        {
                            IsValid = false,
                            AccountId = "",
                            Month = 0,
                            Error = "Invalid input format. Please check and enter again."
                        };
                    }
                }
                else
                {
                    return new StatementInputValidity
                    {
                        IsValid = false,
                        AccountId = "",
                        Month = 0,
                        Error = "Invalid input format. Please check and enter again."
                    };
                }
            }
            catch (Exception ex)
            {
                return new StatementInputValidity
                {
                    IsValid = false,
                    AccountId = "",
                    Month = 0,
                    Error = "Error : " + ex.ToString()
                };

            }
        }

        public void PrintStatement(string accountId, int month)
        {
            // Print
            List<TransactionInfo> transactionsOfAccount = _readTransaction.GetTransactionsOfAccountByMonth(accountId, month);
            if (transactionsOfAccount.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Account: {accountId}");
                Console.WriteLine($"{"Date",-10} {"| Txn Id",-15} {"| Type",-6} {"| Amount",-15} {"| Balance",-10}");
                foreach (TransactionInfo tran in transactionsOfAccount)
                {
                    Console.WriteLine($"{tran.Date,-10} {"| " + tran.Id,-15} {"| " + tran.Type,-6} {"| " + tran.Amount,-15} {"| " + tran.AccountBalance,-10}");
                }

                // Check whether there are created rules for the month
                if (_readInterestRule.GetAllInterestRules().Count > 0)
                {
                    //Calculate interest
                    var ruleForTheMonth = _readInterestRule.GetInterestRuleForMonth(month);
                    decimal interest = CalculateInterest(transactionsOfAccount.ToArray(), ruleForTheMonth, month);
                    var totalBalanceWithInterest = interest + _readAccount.GetAccountByID(accountId).Balance;

                    Console.WriteLine($"{DateTime.DaysInMonth(DateTime.Now.Year, month),-10} {"",-15} {"| I",-6} {"| " + interest,-15} {"| " + totalBalanceWithInterest,-10} ");
                }
            }
            else
            {
                Console.WriteLine($"No transactions found in the account {accountId} for the month of {new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("us"))}");
            }
        }

        public decimal CalculateInterest(TransactionInfo[] transactionsOfMonth, Rule ruleForTheMonth, int month)
        {
            var transactionDates = transactionsOfMonth.Select(t => t.Date).Distinct().ToList();
            var fromDate = new DateTime(DateTime.Now.Year, month, 1); // Date count starts from the first date of the month
            var rate = ruleForTheMonth.InterestPercentage;
            var toDate = _dateHandler.ConvertDateTime(transactionDates[0], "yyyyMMdd");
            var daysCount = 0;
            var endOfDayBalance = 0.00M;
            var totalInterest = 0.00M;

            for (int i = 0; i < transactionDates.Count; i++)
            {
                if (_dateHandler.ConvertDateTime(transactionDates[i], "yyyyMMdd").Equals(fromDate))
                {
                    i++;
                }

                if (_dateHandler.ConvertDateTime(transactionDates[i], "yyyyMMdd") < _dateHandler.ConvertDateTime(ruleForTheMonth.Date, "yyyyMMdd"))
                {
                    // Get the previous rule rate
                    var previousRule = _readInterestRule.GetAllInterestRules().Where(r => _dateHandler.ConvertDateTime(r.Date, "yyyyMMdd") <
                    _dateHandler.ConvertDateTime(ruleForTheMonth.Date, "yyyyMMdd")).LastOrDefault();
                    rate = previousRule.InterestPercentage;

                    // Set toDate
                    toDate = _dateHandler.ConvertDateTime(transactionDates[i], "yyyyMMdd");

                    // Get the end of day balance from transactions for the day
                    var transactionsOfTheDay = transactionsOfMonth.Where(t => t.Date.Equals(transactionDates[i])).ToList();
                    endOfDayBalance = transactionsOfTheDay.LastOrDefault().AccountBalance;

                }
                else
                {
                    if (fromDate < _dateHandler.ConvertDateTime(ruleForTheMonth.Date, "yyyyMMdd"))
                    {
                        // Get the previous rule rate
                        var previousRule = _readInterestRule.GetAllInterestRules().Where(r => _dateHandler.ConvertDateTime(r.Date, "yyyyMMdd") <
                        _dateHandler.ConvertDateTime(ruleForTheMonth.Date, "yyyyMMdd")).LastOrDefault();
                        rate = previousRule.InterestPercentage;

                        toDate = _dateHandler.ConvertDateTime(ruleForTheMonth.Date, "yyyyMMdd");

                        i--;

                    }
                    else
                    {
                        // Get the rule saved date
                        rate = ruleForTheMonth.InterestPercentage;

                        toDate = _dateHandler.ConvertDateTime(transactionDates[i], "yyyyMMdd");

                        // Get the # of days of the rule saved - +1 is to include the last day
                        daysCount = (int)(toDate - fromDate).TotalDays;
                    }

                    // Get the end of day balance from transactions for the day
                    var transactionsOfTheDay = transactionsOfMonth.Where(t => _dateHandler.ConvertDateTime(t.Date, "yyyyMMdd") < _dateHandler.ConvertDateTime(ruleForTheMonth.Date, "yyyyMMdd")).ToList();
                    endOfDayBalance = transactionsOfTheDay.LastOrDefault().AccountBalance;

                }

                // Get the # of days to calculate the interest
                daysCount = (int)(toDate - fromDate).TotalDays;

                var interest = endOfDayBalance * (rate / 100) * daysCount;
                totalInterest += interest;

                fromDate = toDate;
            }

            // Calculate the interest for the rest of the days in month
            var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);
            if (fromDate < new DateTime(DateTime.Now.Year, month, daysInMonth))
            {
                // Get the rule saved date
                rate = ruleForTheMonth.InterestPercentage;

                toDate = new DateTime(DateTime.Now.Year, month, daysInMonth);

                // Get the # of days of the rule saved - +1 is to include the last day
                daysCount = (int)(toDate - fromDate).TotalDays + 1;

                // Get the end of day balance from transactions for the last transaction
                var transactionsOfTheDay = transactionsOfMonth.Where(t => _dateHandler.ConvertDateTime(t.Date, "yyyyMMdd") < new DateTime(DateTime.Now.Year, month, daysInMonth)).ToList();
                endOfDayBalance = transactionsOfTheDay.LastOrDefault().AccountBalance;

                var interest = endOfDayBalance * (rate / 100) * daysCount;
                totalInterest += interest;
            }

            var interestPerMonth = Math.Round(totalInterest / 365, 2);
            return interestPerMonth;
        }
    }
}
