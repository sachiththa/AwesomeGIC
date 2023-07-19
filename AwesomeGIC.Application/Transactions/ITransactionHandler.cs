using AwesomeGIC.Domain.Entities.Transaction;

namespace AwesomeGIC.Application.Transactions
{
    public interface ITransactionHandler
    {
        TransactionValidityResponse IsValidTransactionInput(string[] userTxnDetails);
        void PrintTransactions(string accountID);
    }
}
