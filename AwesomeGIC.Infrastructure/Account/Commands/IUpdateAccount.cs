using AwesomeGIC.Domain.Entities.Transaction;

namespace AwesomeGIC.Infrastructure.Account.Commands
{
    public interface IUpdateAccount
    {
        Domain.Entities.Account Update(string accountId, TransactionInfo transactionInfo);
    }
}
