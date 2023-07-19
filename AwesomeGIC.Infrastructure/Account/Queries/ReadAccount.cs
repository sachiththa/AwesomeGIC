using AwesomeGIC.Infrastructure.DBContext;

namespace AwesomeGIC.Infrastructure.Account.Queries
{
    public class ReadAccount : IReadAccount
    {
        public List<Domain.Entities.Account> GetAllAccounts()
        {
            return Database.Accounts.ToList();
        }

        public Domain.Entities.Account GetAccountByID(string accountId)
        {
            if (Database.Accounts.Any(acc => acc.Id.Equals(accountId)))
            {
                return Database.Accounts.Where(acc => acc.Id.Equals(accountId)).First();
            }
            return null;
        }
    }
}
