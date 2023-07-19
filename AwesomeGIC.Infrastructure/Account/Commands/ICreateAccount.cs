namespace AwesomeGIC.Infrastructure.Account.Commands
{
    public interface ICreateAccount
    {
        Domain.Entities.Account Create(Domain.Entities.Account account);
    }
}
