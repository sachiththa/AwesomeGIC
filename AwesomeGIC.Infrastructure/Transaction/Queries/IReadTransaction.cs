using AwesomeGIC.Domain.Entities.Transaction;

namespace AwesomeGIC.Infrastructure.Transaction.Queries
{
    public interface IReadTransaction
    {
        List<TransactionInfo> GetAllTransactions();
        List<TransactionInfo> GetTransactionsByAccount(string accountID);
        List<TransactionInfo> GetTransactionsByDate(string date);
        List<TransactionInfo> GetTransactionsByDateRange(DateTime startDate, DateTime endDate);
        List<TransactionInfo> GetTransactionsOfAccountByMonth(string accountID, int month);

    }
}
