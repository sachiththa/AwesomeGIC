using AwesomeGIC.Infrastructure.DBContext;

namespace AwesomeGIC.Infrastructure.Account.Commands
{
    public class CreateAccount : ICreateAccount
    {
        public Domain.Entities.Account Create(Domain.Entities.Account account)
        {
            Database.Accounts.Add(account);
            return account;
        }
    }
}
