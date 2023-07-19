using AwesomeGIC.Domain.Entities.Transaction;

namespace AwesomeGIC.Infrastructure.Transaction.Commands
{
    public interface ISaveTransaction
    {
        TransactionSavedStatus Save(TransactionInfo transactionToSave);
    }
}
