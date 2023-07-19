using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Domain.Enums;
using AwesomeGIC.Infrastructure.Transaction.Queries;
using AwesomeGIC.Infrastructure.Utils;

namespace AwesomeGIC.Application.Transactions
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly IDateHandler _dateHandler;
        private readonly IReadTransaction _readTransaction;

        public TransactionHandler(IDateHandler dateHandler, IReadTransaction readTransaction)
        {
            _dateHandler = dateHandler;
            _readTransaction = readTransaction;
        }

        /* Takes the input from the user and validates it
         * Validations:
         * 1. Date format : YYYYMMdd
         * 2. Type should be D or W (D-Deposit, W-Withdrawal)
         * 3. Amount must be >0 and if there are decimal points, only 2 decimal points allowed
        */
        public TransactionValidityResponse IsValidTransactionInput(string[] userTxnDetails)
        {
            try
            {
                // Check whether the user has entered all the details of the transaction
                if (userTxnDetails.Length == 4)
                {
                    // 1. Check for the date format
                    if (_dateHandler.isDateFormatValid(userTxnDetails[0]))
                    {
                        // 2. Check for the transaction type
                        if (Enum.TryParse(typeof(TransactionType), userTxnDetails[2].ToUpper(), out object? type))
                        {
                            // 3. Check whether the amount is >0
                            if (Decimal.Parse(userTxnDetails[3]) > 0)
                            {
                                var transactionToSave = new TransactionInfo()
                                {
                                    Id = this.GenerateTransactionID(userTxnDetails),
                                    //Account = new Domain.Entities.Account() { Id = userTxnDetails[1] },
                                    AccountId = userTxnDetails[1],
                                    Date = userTxnDetails[0],
                                    Type = (TransactionType)Enum.Parse(typeof(TransactionType), userTxnDetails[2].ToUpper()),
                                    Amount = Math.Round(Decimal.Parse(userTxnDetails[3]), 2)
                                };

                                return new TransactionValidityResponse()
                                {
                                    IsValid = true,
                                    Error = "",
                                    TransactionInfo = transactionToSave
                                };
                            }
                            else
                            {
                                return new TransactionValidityResponse()
                                {
                                    IsValid = false,
                                    Error = "Amount should be greater than 0",
                                    TransactionInfo = null
                                };
                            }
                        }
                        else
                        {
                            return new TransactionValidityResponse()
                            {
                                IsValid = false,
                                Error = "Invalid transaction type.",
                                TransactionInfo = null
                            };
                        }
                    }
                    else
                    {
                        return new TransactionValidityResponse()
                        {
                            IsValid = false,
                            Error = "Invalid date format.",
                            TransactionInfo = null
                        };
                    }

                }
                else
                {
                    return new TransactionValidityResponse()
                    {
                        IsValid = false,
                        Error = "Invalid input format.",
                        TransactionInfo = null
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new TransactionValidityResponse()
                {
                    IsValid = false,
                    Error = "Error : " + ex.ToString(),
                    TransactionInfo = null
                };
            }
        }

        public void PrintTransactions(string accountID)
        {
            List<TransactionInfo> transactionsOfAccount = _readTransaction.GetTransactionsByAccount(accountID);
            if (transactionsOfAccount.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Account: {accountID}");
                Console.WriteLine($"{"Date",-10} {"| Txn Id",-15} {"| Type",-6} {"| Amount",-15}");
                foreach (TransactionInfo tran in transactionsOfAccount)
                {
                    Console.WriteLine($"{tran.Date,-10} {"| " + tran.Id,-15} {"| " + tran.Type,-6} {"| " + tran.Amount,-15}");
                }
            }
        }

        public string GenerateTransactionID(string[] transactionDetails)
        {
            var id = transactionDetails[0] + "-" + string.Format("{0:00}",
                _readTransaction.GetTransactionsByDate(transactionDetails[0]).Count + 1);
            return id;
        }
    }
}
