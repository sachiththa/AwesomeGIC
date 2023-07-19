using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Domain.Enums;
using AwesomeGIC.Infrastructure.DBContext;

namespace AwesomeGIC.Infrastructure.Account.Commands
{
    public class UpdateAccount : IUpdateAccount
    {
        public Domain.Entities.Account Update(string accountId, TransactionInfo transactionInfo)
        {
            var accountToUpdate = Database.Accounts?.FindLast(acc => acc.Id.Equals(accountId));
            if (transactionInfo.Type == TransactionType.W)
            {
                accountToUpdate.Balance = accountToUpdate.Balance - transactionInfo.Amount;
            }
            else
            {
                accountToUpdate.Balance = accountToUpdate.Balance + transactionInfo.Amount;
            }
            return accountToUpdate;
        }
    }
}
