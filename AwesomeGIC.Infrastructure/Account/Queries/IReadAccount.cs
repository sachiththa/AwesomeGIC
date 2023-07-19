namespace AwesomeGIC.Infrastructure.Account.Queries
{
    public interface IReadAccount
    {
        List<Domain.Entities.Account> GetAllAccounts();
        Domain.Entities.Account GetAccountByID(string accountId);
    }
}
