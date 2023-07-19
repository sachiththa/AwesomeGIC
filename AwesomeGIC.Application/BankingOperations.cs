using AwesomeGIC.Application.Rules;
using AwesomeGIC.Application.Statement;
using AwesomeGIC.Application.Transactions;
using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Domain.Enums;
using AwesomeGIC.Infrastructure.Account.Commands;
using AwesomeGIC.Infrastructure.Account.Queries;
using AwesomeGIC.Infrastructure.DBContext;
using AwesomeGIC.Infrastructure.InterestRule.Commands;
using AwesomeGIC.Infrastructure.InterestRule.Queries;
using AwesomeGIC.Infrastructure.Transaction.Commands;
using System.Data;

namespace AwesomeGIC.Application
{
    public class BankingOperations : IBankingOperations
    {

        private readonly ITransactionHandler _transactionHandler;
        private readonly ISaveTransaction _saveTransaction;
        private readonly ICreateAccount _createAccount;

        private readonly IRuleHandler _ruleHandler;
        private readonly ISaveInterestRule _saveInterestRule;
        private readonly IReplaceInterestRule _replaceInterestRule;
        private readonly IReadInterestRule _readInterestRule;

        private readonly IStatementHandler _statementHandler;

        private readonly IUpdateAccount _updateAccount;
        private readonly IReadAccount _readAccount;

        public BankingOperations(ITransactionHandler transactionHandler, ISaveTransaction saveTransaction,
            ICreateAccount createAccount, IRuleHandler ruleHandler, ISaveInterestRule saveInterestRule, IReplaceInterestRule replaceInterestRule,
            IReadInterestRule readInterestRule, IUpdateAccount updateAccount, IReadAccount readAccount, IStatementHandler statementHandler)
        {
            _transactionHandler = transactionHandler;
            _saveTransaction = saveTransaction;
            _createAccount = createAccount;
            _ruleHandler = ruleHandler;
            _saveInterestRule = saveInterestRule;
            _replaceInterestRule = replaceInterestRule;
            _readInterestRule = readInterestRule;
            _updateAccount = updateAccount;
            _readAccount = readAccount;
            _statementHandler = statementHandler;
        }

        public void LaunchApp()
        {
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            ShowBankingOptions();
        }

        public void ShowBankingOptions()
        {
            // Show the options to the user            
            Console.WriteLine("[I]nput transactions");
            Console.WriteLine("[D]efine interest rules");
            Console.WriteLine("[P]rint Statement");
            Console.WriteLine("[Q]uit");

            // Read the user's input
            string? userChoice = Console.ReadLine()?.Trim().ToUpper();

            if (!String.IsNullOrEmpty(userChoice))
            {
                this.HandleUserInput(userChoice);
            }
            else
            {
                Console.WriteLine("No option selected.");
                Console.WriteLine();
                ShowBankingOptions();
            }

        }

        public void HandleUserInput(string userChoice)
        {
            switch (userChoice)
            {
                case "I":
                    // Ask user to enter transaction details
                    Console.WriteLine("Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format \n(or enter blank to go back to main menu):");
                    string? transaction = Console.ReadLine();
                    if (!String.IsNullOrEmpty(transaction))
                    {
                        this.ValidateTransactionInput(transaction);
                    }
                    else
                    {
                        Console.WriteLine();
                        ShowBankingOptions();   // Redirect to the main menu
                    }
                    break;
                case "D":
                    Console.WriteLine("Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format \r\n(or enter blank to go back to main menu):");
                    string? rule = Console.ReadLine();
                    if (!String.IsNullOrEmpty(rule))
                    {
                        this.ValidateRuleInput(rule);
                    }
                    else
                    {
                        Console.WriteLine();
                        ShowBankingOptions();
                    }
                    break;
                case "P":
                    //Print statement
                    Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month>\n(or enter blank to go back to main menu):");
                    string? printStatementInput = Console.ReadLine();
                    if (!String.IsNullOrEmpty(printStatementInput))
                    {
                        this.ValidateStatementInput(printStatementInput);
                    }
                    else
                    {
                        Console.WriteLine();
                        ShowBankingOptions();
                    }
                    break;
                case "Q":
                    //Exit from menu
                    Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\nHave a nice day!");
                    break;
                default:
                    //Go to the main menu
                    Console.WriteLine();
                    ShowBankingOptions();
                    break;
            }
        }

        private void ValidateStatementInput(string printStatementInput)
        {
            try
            {
                string[] inputDetails = printStatementInput.Trim().Split("|");
                var response = _statementHandler.IsValidPrintStatementInput(inputDetails);

                if (response.IsValid)
                {
                    // Print statement
                    _statementHandler.PrintStatement(response.AccountId, response.Month);
                    Console.WriteLine();
                    Console.WriteLine("Is there anything else you would like to do?");
                    ShowBankingOptions();
                }
                else
                {
                    Console.WriteLine(response.Error?.ToString());
                    Console.WriteLine();
                    this.HandleUserInput("P");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
                Console.WriteLine();
                this.ShowBankingOptions();
            }
        }

        private void ValidateRuleInput(string rule)
        {
            try
            {
                string[] ruleDetails = rule.Trim().Split("|");
                RuleValidityResponse response = _ruleHandler.IsValidRuleInput(ruleDetails);
                if (response != null)
                {
                    if (response.IsValid)
                    {
                        var savedResponse = this.SaveRule(response.RuleInfo);
                        if (savedResponse.IsSaved)
                        {
                            // Showing the transactions of the account
                            _ruleHandler.PrintRules(savedResponse.RuleInfo.Id);
                            Console.WriteLine();
                            Console.WriteLine("Is there anything else you would like to do?");
                            ShowBankingOptions();
                        }
                        else
                        {
                            Console.WriteLine($"Transaction cannot be done : {savedResponse.Error}");
                            Console.WriteLine();
                            Console.WriteLine("Is there anything else you would like to do?");
                            ShowBankingOptions();
                        }

                    }
                    else
                    {
                        Console.WriteLine(response.Error?.ToString());
                        Console.WriteLine();
                        this.HandleUserInput("D");
                    }
                }
                else
                {
                    Console.WriteLine("Something went wrong. Please Try again.");
                    Console.WriteLine();
                    Console.WriteLine("Is there anything else you would like to do?");
                    this.ShowBankingOptions();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
                Console.WriteLine();
                this.ShowBankingOptions();
            }
        }

        private RuleSavedStatus SaveRule(Domain.Entities.Rule.Rule ruleInfo)
        {
            // Check if a rule exists for the date
            if (_readInterestRule.RuleExistsForDate(ruleInfo.Date))
            {
                // Replace current rule
                return _replaceInterestRule.Replace(ruleInfo);
            }
            else
            {
                // Create new rule
                return _saveInterestRule.Save(ruleInfo);
            }
        }

        private void ValidateTransactionInput(string transaction)
        {
            try
            {
                string[] transactionDetails = transaction.Trim().Split("|");
                TransactionValidityResponse response = _transactionHandler.IsValidTransactionInput(transactionDetails);
                if (response != null)
                {
                    if (response.IsValid)
                    {
                        var savedResponse = this.SaveTransaction(response.TransactionInfo);
                        if (savedResponse.IsSaved)
                        {
                            // Showing the transactions of the account
                            _transactionHandler.PrintTransactions(savedResponse.TransactionInfo.AccountId);
                            Console.WriteLine();
                            Console.WriteLine("Is there anything else you would like to do?");
                            ShowBankingOptions();
                        }
                        else
                        {
                            Console.WriteLine($"Transaction cannot be done : {savedResponse.Error}");
                            Console.WriteLine();
                            Console.WriteLine("Is there anything else you would like to do?");
                            ShowBankingOptions();
                        }

                    }
                    else
                    {
                        Console.WriteLine(response.Error?.ToString());
                        Console.WriteLine();
                        this.HandleUserInput("I");
                    }
                }
                else
                {
                    Console.WriteLine("Something went wrong. Please Try again.");
                    Console.WriteLine();
                    Console.WriteLine("Is there anything else you would like to do?");
                    this.ShowBankingOptions();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
                Console.WriteLine();
                this.ShowBankingOptions();
            }
        }

        private TransactionSavedStatus SaveTransaction(TransactionInfo transactionToSave)
        {
            //Validation - Withdrawal is allowed only if the account exists and have a balance more than the withdrawal amount
            var accountOfTransaction = _readAccount.GetAccountByID(transactionToSave.AccountId);
            if (accountOfTransaction != null)
            {
                if (transactionToSave.Type == TransactionType.W)
                {
                    if (transactionToSave.Amount > accountOfTransaction.Balance)
                    {
                        return new TransactionSavedStatus() { IsSaved = false, TransactionInfo = transactionToSave, Error = $"Account balance is insufficient. Cannot withdraw {transactionToSave.Amount}." };
                    }
                    else
                    {
                        var savedStatus = _saveTransaction.Save(transactionToSave); // Save transaction
                        if (savedStatus.IsSaved)
                        {
                            var updatedAccount = _updateAccount.Update(accountOfTransaction.Id, savedStatus.TransactionInfo); // Update account balance
                            var transactionRecord = Database.TransactionInfos.Where(t => t.Id.Equals(savedStatus.TransactionInfo.Id)).First();
                            transactionRecord.AccountBalance = updatedAccount.Balance;
                        }

                        return savedStatus;
                    }
                }
                else
                {
                    var savedStatus = _saveTransaction.Save(transactionToSave); // Save transaction
                    if (savedStatus.IsSaved)
                    {
                        var updatedAccount = _updateAccount.Update(accountOfTransaction.Id, savedStatus.TransactionInfo); // Update account balance
                        var transactionRecord = Database.TransactionInfos.Where(t => t.Id.Equals(savedStatus.TransactionInfo.Id)).First();
                        transactionRecord.AccountBalance = updatedAccount.Balance;

                    }

                    return savedStatus;
                }
            }
            else
            {
                // If the account does not exist the first transaction should be a Deposit
                if (transactionToSave.Type == TransactionType.W)
                {
                    return new TransactionSavedStatus() { IsSaved = false, TransactionInfo = transactionToSave, Error = "You should make a deposit before you can withdraw." };
                }
                else
                {
                    var newAccount = new Domain.Entities.Account() { Id = transactionToSave.AccountId, Balance = transactionToSave.Amount };
                    transactionToSave.AccountBalance = transactionToSave.Amount;
                    var accountCreationResponse = _createAccount.Create(newAccount); // Create account
                    var savedStatus = _saveTransaction.Save(transactionToSave); // Save transaction
                    if (savedStatus.IsSaved)
                    {
                        var transactionRecord = Database.TransactionInfos.Where(t => t.Id.Equals(savedStatus.TransactionInfo.Id)).First(); // Update account in transaction
                        transactionRecord.AccountBalance = savedStatus.TransactionInfo.Amount;

                    }
                    return savedStatus;
                }
            }

        }

    }
}
